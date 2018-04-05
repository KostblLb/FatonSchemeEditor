using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ontology;

namespace HelloForms
{
    public class OntologyBuilder
    {
        public static List<OntologyNode> fromXml(XElement ontology)
        {
            OntologyClass currentClass = null;
            List<OntologyNode> result = new List<OntologyNode>();
            var classes = from x in ontology.Elements()
                          where x.Name.LocalName == "class"
                          select x;
            var domains = from x in ontology.Elements()
                          where x.Name.LocalName == "domain"
                          select x;
            foreach (XElement c in classes)
            {
                currentClass = new OntologyClass(c.Attribute("name").Value);
                var classAttrs = from x in c.Elements()
                                 where x.Name.LocalName == "attr"
                                 select x;
                var classParents = from x in c.Elements()
                                   where x.Name.LocalName == "base"
                                   select x;
                foreach (XElement attrElement in classAttrs)
                {
                    var attrName = ((XText)attrElement.FirstNode).Value; //get inner text of <attr>
                    var attrTypeStr = attrElement.Attribute("type").Value;
                    var attrClassName = attrElement.Attribute("class")?.Value;
                    var attrDomName = attrElement.Attribute("domain")?.Value;

                    OntologyNode.Attribute.AttributeType attrType = (OntologyNode.Attribute.AttributeType)Enum.Parse(typeof(OntologyNode.Attribute.AttributeType), attrTypeStr.ToUpper());
                    var attr = new OntologyNode.Attribute(attrType, attrName);
                    if (attrType == OntologyNode.Attribute.AttributeType.OBJECT && attrClassName != null)
                        attr.Opt = attrClassName;
                    if (attrType == OntologyNode.Attribute.AttributeType.DOMAIN && attrDomName != null)
                        attr.Opt = attrDomName;
                    currentClass.OwnAttributes.Add(attr);
                }
                if (classParents.Any())
                {
                    foreach (XElement parentElement in classParents)
                    {
                        OntologyClass parentClass = null;
                        foreach(OntologyClass rootClass in result)
                        {
                            parentClass = rootClass.FindChild(parentElement.Value);
                            if (parentClass != null) break;
                        }
                        if (parentClass == null)
                            throw new Exception(String.Format(Locale.ERR_ONTOLOGY_NOPARENT, parentElement.Value, currentClass.Name));
                        parentClass.AddChild(currentClass);
                        currentClass.AddParent(parentClass);
                    }
                }
                else
                    result.Add(currentClass);
            }

            Ontology.Ontology.Domains = new Dictionary<string, List<string>>();
            foreach (XElement d in domains) {
                var valueList = new List<string>();
                foreach (XElement el in d.Elements())
                    valueList.Add(el.Value);
                Ontology.Ontology.Domains.Add(d.Attribute("name").Value, valueList);
            }

            Ontology.Ontology.Classes = result;
            return result;
        }
        public static List<OntologyNode> fromXml(Stream istream)
        {
            StreamReader sr = new StreamReader(istream);
            string xmlString = sr.ReadToEnd();
            XDocument doc = XDocument.Parse(xmlString);
            var ontology = doc.Element("ontology"); // aka Root

            return fromXml(ontology);
        }
    }
}
