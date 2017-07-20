using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using FactScheme;
using Faton;
using Ontology;
using Shared;
using System.Xml;

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
                xbank.Add(scheme.ToXml());
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
                var conditions = from x in xscheme.Elements()
                              where x.Name.LocalName == "Condition"
                              select x;
                var functors = from x in xscheme.Elements()
                              where x.Name.LocalName == "Functor"
                              select x;
                foreach (XElement xarg in arguments)
                {
                    Argument arg = null;
                    if (xarg.Attribute(FatonConstants.XML_ATTR_ARG_TYPE).Value == FatonConstants.XML_ATTR_ARG_TYPE_TERMIN)
                    {
                        //var theme = themes.Find(x => x.name == xarg.Attribute("ClassName").Value);
                        //arg = scheme.AddArgument(theme);
                    }
                    else
                    {
                        OntologyClass argKlass;
                        foreach (OntologyClass klass in ontology)
                        {
                            argKlass = klass.FindChild(xarg.Attribute("ClassName").Value);
                            if (argKlass == null)
                                continue;
                            arg = scheme.AddArgument(argKlass);
                            break;
                        }
                    }
                    arg.Inheritance = bool.Parse(xarg.Attribute(FatonConstants.XML_ATTR_ARG_INHERITANCE).Value);
                    arg.Order = uint.Parse(xarg.Attribute("Order").Value);
                    foreach(XElement xcond in xarg.Elements("Condition"))
                    {
                        var condition = new Argument.ArgumentCondition();
                        var attrName = xcond.Attribute("Attribute").Value;
                        var type = xcond.Attribute("Type").Value;
                        var comparType = xcond.Attribute("ComparType").Value;
                        var value = xcond.Attribute("Value").Value;
                        condition.ComparType = (Argument.ArgumentCondition.ComparisonType) Enum.Parse(typeof(Argument.ArgumentCondition.ComparisonType), comparType);
                        condition.CondType = (Argument.ArgumentCondition.ConditionType)Enum.Parse(typeof(Argument.ArgumentCondition.ConditionType), type);
                        condition.Value = value;
                        var attr = arg.Attributes.Find(x => x.Name.Equals(attrName));
                        arg.Conditions[attr].Add(condition);
                    }
                }
                foreach (XElement xres in results)
                {
                    OntologyClass resKlass = null;
                    Result result;
                    foreach(OntologyClass klass in ontology)
                    {
                        resKlass = klass.FindChild(xres.Attribute("ClassName").Value);
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
                            OntologyNode.Attribute attr = result.Reference.AllAttributes.Find(x => x.Name == xrul.Attribute("Attribute").Value);
                            OntologyNode.Attribute inputAttr = null;
                            if (attr.AttrType != OntologyNode.Attribute.AttributeType.OBJECT)
                            {
                                inputAttr = arg.Attributes.Find(x => x.Name == xrul.Attribute("AttrFrom").Value);
                            }
                            result.AddRule(ruleType, attr, arg, inputAttr);
                        }
                        
                        if(ruleType == Result.RuleType.FUNC)
                        {

                        }
                    }
                    if (xres.Attribute("ArgEdit") != null)
                        result.EditObject = scheme.Arguments.Find(x => x.Order == int.Parse(xres.Attribute("ArgEdit").Value));                    
                    if (xres.Attribute("ResultEdit") != null)
                        result.EditObject = scheme.Results.Find(x => x.Name == xres.Attribute("ArgEdit").Value);

                }

                foreach(var xcond in conditions)
                {
                    var cond = scheme.AddCondition();
                    cond.ID = uint.Parse(xcond.Attribute("ID").Value);
                    cond.Type = (Condition.ConditionType)Enum.Parse(typeof(Condition.ConditionType), xcond.Attribute("Type").Value);
                    cond.ComparType = (Condition.ComparisonType)Enum.Parse(typeof(Condition.ComparisonType), xcond.Attribute("Operation").Value);

                    var arg1 = scheme.Arguments.Find(x =>
                       x.Order == uint.Parse(xcond.Attribute("Arg1").Value));
                    var arg2 = scheme.Arguments.Find(x =>
                       x.Order == uint.Parse(xcond.Attribute("Arg2").Value));
                    cond.Args[0] = arg1;
                    cond.Args[1] = arg2;
                    switch (cond.Type){
                        case Condition.ConditionType.CONTACT:
                            cond.Contact = (Condition.ConditionContact)Enum.Parse(typeof(Condition.ConditionType), xcond.Attribute("Contact").Value);
                            break;
                        case Condition.ConditionType.MORPH:
                            cond.MorphAttr = xcond.Attribute("GramtabAttr").Value;
                            break;
                        case Condition.ConditionType.POS:
                            cond.Position = (Condition.ConditionPosition)Enum.Parse(typeof(Condition.ConditionPosition), xcond.Attribute("Position").Value);
                            break;
                        case Condition.ConditionType.SEG:
                            cond.Segment = xcond.Attribute("Segment").Value;
                            break;
                        case Condition.ConditionType.SEM:
                            cond.SemAttrs[0] = arg1.Attributes.Find(x => x.Name.Equals(xcond.Attribute("AttrName1").Value));
                            cond.SemAttrs[1] = arg1.Attributes.Find(x => x.Name.Equals(xcond.Attribute("AttrName2").Value));
                            break;
                        case Condition.ConditionType.SYNT:
                            cond.ActantNames[0] = xcond.Attribute("ActantName").Value;
                            //cond.ActantNames[1] = xcond.Attribute("ActantName2").Value;
                            break;
                    }
                }

                bank.Schemes.Add(scheme);
            }

            return bank; 
        }
    }
}
