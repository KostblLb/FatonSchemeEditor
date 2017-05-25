using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ontology;
using KlanVocabularyExtractor;

namespace HelloForms
{
    public class OntologyBuilder
    {
        public static List<OntologyNode> fromXml(Stream istream, List<VocTheme> themes)
        {
            List<OntologyNode> result = new List<OntologyNode>();
            StreamReader sr = new StreamReader(istream);
            string xmlString = sr.ReadToEnd();

            OntologyClass currentClass = null;

            XDocument doc = XDocument.Parse(xmlString);
            var ontology = doc.Element("ontology"); // aka Root
            var classes = from x in ontology.Elements()
                          where x.Name.LocalName == "class"
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
                    string attrName = ((XText)attrElement.FirstNode).Value; //get inner text of <attr>
                    string attrTypeStr = attrElement.Attribute("type").Value;
                    OntologyNode.Attribute.AttributeType attrType = (OntologyNode.Attribute.AttributeType)Enum.Parse(typeof(OntologyNode.Attribute.AttributeType), attrTypeStr.ToUpper());
                    if (attrType == OntologyNode.Attribute.AttributeType.TERMIN)
                    {
                        string themeStr = attrElement.Attribute("theme").Value;
                        var theme = themes.Find(x => x.name.Equals(themeStr));
                        currentClass.OwnAttributes.Add(new OntologyNode.Attribute(theme, attrName));
                    }
                    else
                        currentClass.OwnAttributes.Add(new OntologyNode.Attribute(currentClass, attrType, attrName));
                }
                if (classParents.Any())
                {
                    foreach (XElement parentElement in classParents)
                    {
                        OntologyClass parentClass = (OntologyClass)result.Find(x => x.Name == ((XText)parentElement.FirstNode).Value);
                        if (parentClass == null)
                            throw new Exception(String.Format("base class {0} not defined for {1}", parentElement.Attribute("name").Value, currentClass.Name));
                        parentClass.AddChild(currentClass);
                        currentClass.AddParent(parentClass);
                    }
                }
                else
                    result.Add(currentClass);
            }
            return result;
        }
    }
}
