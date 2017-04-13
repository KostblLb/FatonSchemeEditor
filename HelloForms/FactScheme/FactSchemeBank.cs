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
                    foreach(OntologyClass klass in ontology)
                    {
                        argKlass = klass.Search(xarg.Attribute("ClassName").Value);
                        if (argKlass == null)
                            continue;
                        Argument arg = scheme.AddArgument(argKlass);
                        arg.Inheritance = bool.Parse(xarg.Attribute(FatonConstants.XML_ATTR_ARG_INHERITANCE).Value);
                        arg.Order = uint.Parse(xarg.Attribute("Order").Value);
                        break;
                    }
                }
                foreach (XElement xres in results)
                {
                    OntologyClass resKlass = null;
                    Result result;
                    foreach(OntologyClass klass in ontology)
                    {
                        resKlass = klass.Search(xres.Attribute("ClassName").Value);
                        if (resKlass != null)
                            break;
                    }
                    if (resKlass == null)
                        continue;
                    result = scheme.AddResult(resKlass, xres.Attribute("Name").Value);
                    foreach(XElement xrul in xres.Elements())
                    {
                        Result.RuleType ruleType = (Result.RuleType) Enum.Parse(typeof(Result.RuleType),
                            xrul.Attribute("Type").Value);
                        if (ruleType == Result.RuleType.DEF)
                        {
                            Argument arg = scheme.Arguments.Find(x => x.Order == int.Parse(xrul.Attribute("ArgFrom").Value));
                            OntologyNode.Attribute inputAttr = arg.Klass.AllAttributes.Find(x => x.Name == xrul.Attribute("AttrFrom").Value);
                            OntologyNode.Attribute attr = result.Reference.AllAttributes.Find(x => x.Name == xrul.Attribute("Attribute").Value);
                            result.AddRule(ruleType, attr, arg, inputAttr);
                        }
                        
                        if(ruleType == Result.RuleType.FUNC)
                        {

                        }

                    }
                }

                bank.Schemes.Add(scheme);
            }

            return bank; 
        }
    }
}
