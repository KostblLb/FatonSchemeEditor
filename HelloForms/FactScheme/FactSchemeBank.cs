using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FactScheme;
using Faton;
using Ontology;

namespace HelloForms
{ 
    public class FactSchemeBank
    {
        public string Name;
        public List<Scheme> Schemes;

        public FactSchemeBank(string name = "new_bank")
        {
            Schemes = new List<Scheme>();
            Name = name;
        }

        public XDocument ToXml()
        {
            XDocument doc = new XDocument();
            XElement xbank = new XElement(FatonConstants.XML_BANK_NAME);
            foreach (Scheme scheme in Schemes)
            {
                xbank.Add(scheme.ToXml().Root);
            }
            doc.Add(xbank);

            return doc;
        }

        public static FactSchemeBank FromXml(XElement root, List<OntologyNode> ontology)
        {
            FactSchemeBank bank = new FactSchemeBank();
            foreach(XElement xscheme in root.Elements())
            {
                Scheme scheme = new Scheme(xscheme.Name.LocalName);
                var arguments = from x in xscheme.Elements()
                              where x.Name.LocalName == "Argument"
                              select x;
                var results = from x in xscheme.Elements()
                              where x.Name.LocalName == "Result"
                              select x;
                var functors = from x in xscheme.Elements()
                              where x.Name.LocalName == "Functor"
                              select x;
                var relations = from x in xscheme.Elements()
                              where x.Name.LocalName == "Relation"
                              select x;
                foreach (XElement xarg in arguments)
                {
                    OntologyClass argKlass;
                    //scheme.AddArgument()
                    foreach(OntologyClass klass in ontology)
                    {
                        argKlass = klass.Search(xarg.Attribute("ClassName").Value);
                        if (argKlass == null)
                            break;
                        FactScheme.Argument arg = scheme.AddArgument(argKlass);
                        arg.Inheritance = bool.Parse(xarg.Attribute(FatonConstants.XML_ATTR_ARG_INHERITANCE).Value);
                    }
                }

                foreach (XElement xres in results)
                {
                    scheme.AddResult();
                }

                bank.Schemes.Add(scheme);
            }

            return bank; 
        }
    }
}
