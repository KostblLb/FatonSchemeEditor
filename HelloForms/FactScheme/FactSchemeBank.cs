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
    [XmlRoot(ElementName = FatonConstants.XML_BANK_TAG)]
    public class FactSchemeBank
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement(ElementName = FatonConstants.XML_SCHEME_TAG)]
        public List<Scheme> Schemes { get; private set; }

        public FactSchemeBank()
        {
            Schemes = new List<Scheme>();
        }

        public FactSchemeBank(string name = "new_bank") : this()
        {
            Name = name;
        }

        public XDocument ToXml()
        {
            XDocument doc = new XDocument();
            XElement xbank = new XElement(FatonConstants.XML_BANK_TAG);
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
                Scheme scheme = new Scheme(xscheme.Attribute(FatonConstants.XML_SCHEME_NAME).Value);
                var arguments = from x in xscheme.Elements()
                              where x.Name.LocalName == FatonConstants.XML_ARGUMENT_TAG
                              select x;
                var results = from x in xscheme.Elements()
                              where x.Name.LocalName == FatonConstants.XML_RESULT_TAG
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
                            argKlass = klass.FindChild(xarg.Attribute(FatonConstants.XML_ARGUMENT_CLASSNAME).Value);
                            if (argKlass == null)
                                continue;
                            arg = scheme.AddArgument(argKlass);
                            break;
                        }
                    }
                    arg.Inheritance = bool.Parse(xarg.Attribute(FatonConstants.XML_ATTR_ARG_INHERITANCE).Value);
                    arg.Order = uint.Parse(xarg.Attribute(FatonConstants.XML_ARGUMENT_ORDER).Value);
                    foreach(XElement xcond in xarg.Elements(FatonConstants.XML_ARGUMENT_CONDITION_TAG))
                    {
                        var condition = new Argument.ArgumentCondition();
                        var attrName = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ATTRNAME).Value;
                        var type = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE).Value;
                        var comparType = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION).Value;
                        var value = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA).Value;
                        condition.ComparType = (ArgumentConditionOperation) Enum.Parse(typeof(ArgumentConditionOperation), comparType);
                        condition.CondType = (ArgumentConditionType)Enum.Parse(typeof(ArgumentConditionType), type);
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
                    cond.ID = uint.Parse(xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ID).Value);
                    cond.Type = (ConditionType)Enum.Parse(typeof(ConditionType), xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE).Value);
                    cond.Operation = (ConditionOperation)Enum.Parse(typeof(ConditionOperation), xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION).Value);

                    var arg1 = scheme.Arguments.Find(x =>
                       x.Order == uint.Parse(xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ARG1).Value));
                    var arg2 = scheme.Arguments.Find(x =>
                       x.Order == uint.Parse(xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ARG2).Value));
                    cond.Arg1 = arg1;
                    cond.Arg2 = arg2;
                    cond.Data = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA).Value;
                }

                bank.Schemes.Add(scheme);
            }

            return bank; 
        }
    }
}
