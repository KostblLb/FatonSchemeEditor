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
using Vocabularies;
using System.Xml;
using System.ComponentModel;

namespace HelloForms
{
    [XmlRoot(ElementName = FatonConstants.XML_BANK_TAG)]
    public class FactSchemeBank
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement(ElementName = FatonConstants.XML_SCHEME_TAG)]
        public BindingList<Scheme> Schemes { get; private set; }

        public FactSchemeBank()
        {
            Schemes = new BindingList<Scheme>();
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

        public static FactSchemeBank FromXml(XElement root, List<OntologyNode> ontology, Vocabularies.Vocabulary themes)
        {
            FactSchemeBank bank = new FactSchemeBank();
            foreach (XElement xscheme in root.Elements())
            {
                Scheme scheme = new Scheme(xscheme.Attribute(FatonConstants.XML_SCHEME_NAME).Value);
                scheme.Segment = xscheme.Attribute(FatonConstants.XML_SCHEME_SEGMENT)?.Value;
                if (scheme.Segment == null)
                    scheme.Segment = "";
                var arguments = from x in xscheme.Elements()
                                where x.Name.LocalName == FatonConstants.XML_ARGUMENT_TAG
                                select x;
                var results = from x in xscheme.Elements()
                              where x.Name.LocalName == FatonConstants.XML_RESULT_TAG
                              select x;
                var conditionComplexes = from x in xscheme.Elements()
                                         where x.Name.LocalName == FatonConstants.XML_CONDITIONCOMPLEX_TAG
                                         select x;
                var functors = from x in xscheme.Elements()
                               where x.Name.LocalName == "Functor"
                               select x;
                foreach (XElement xarg in arguments)
                {
                    Argument arg = null;
                    if (xarg.Attribute(FatonConstants.XML_ARGUMENT_OBJECTTYPE).Value.Equals(ArgumentType.TERMIN.ToString()))
                    {
                        Termin term;
                        string termName = xarg.Attribute("ClassName").Value;
                        if (termName.StartsWith("#"))
                            term = DiglexFunctions.LexFunctions.First(x => x.Name == termName);
                        else
                            term = themes[termName];
                        arg = scheme.AddArgument(term);
                        var varattrs = from x in xarg.Elements() where x.Name == "VarAttr" select x;
                        foreach (var attr in varattrs)
                            arg.Attributes.Add(new OntologyNode.Attribute(OntologyNode.Attribute.AttributeType.STRING, attr.Attribute("Name").Value, true));
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
                    arg.Order = uint.Parse(xarg.Attribute(FatonConstants.XML_ARGUMENT_ORDER).Value);
                    foreach (XElement xcond in xarg.Elements(FatonConstants.XML_ARGUMENT_CONDITION_TAG))
                    {
                        var condition = new Argument.ArgumentCondition();
                        var attrName = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ATTRNAME).Value;
                        var type = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE).Value;
                        var comparType = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION).Value;
                        var value = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA).Value;
                        condition.Operation = (ArgumentConditionOperation)Enum.Parse(typeof(ArgumentConditionOperation), comparType);
                        condition.CondType = (ArgumentConditionType)Enum.Parse(typeof(ArgumentConditionType), type);
                        condition.Data = value;
                        var attr = arg.Attributes.Find(x => x.Name.Equals(attrName));
                        arg.Conditions[attr].Add(condition);
                    }
                }
                foreach (XElement xres in results)
                {
                    OntologyClass resKlass = null;
                    Result result;
                    foreach (OntologyClass klass in ontology)
                    {
                        resKlass = klass.FindChild(xres.Attribute(FatonConstants.XML_RESULT_CLASSNAME).Value);
                        if (resKlass != null)
                            break;
                    }
                    if (resKlass == null)
                        continue;
                    result = scheme.AddResult(resKlass, xres.Attribute(FatonConstants.XML_RESULT_NAME).Value);
                    foreach (XElement xrul in xres.Elements())
                    {
                        Result.RuleType ruleType = (Result.RuleType)Enum.Parse(typeof(Result.RuleType),
                            xrul.Attribute(FatonConstants.XML_RESULT_RULE_TYPE).Value);
                        OntologyNode.Attribute attr = result.Reference.AllAttributes.Find(x => x.Name == xrul.Attribute(FatonConstants.XML_RESULT_RULE_ATTR).Value);
                        if (ruleType == Result.RuleType.ATTR)
                        {
                            Argument arg = scheme.Arguments.Find(x => x.Order == int.Parse(xrul.Attribute(FatonConstants.XML_RESULT_RULE_RESOURCE).Value));
                            OntologyNode.Attribute inputAttr = null;
                            if (attr.AttrType != OntologyNode.Attribute.AttributeType.OBJECT)
                            {
                                inputAttr = arg.Attributes.Find(x => x.Name == xrul.Attribute(FatonConstants.XML_RESULT_RULE_ATTRFROM).Value);
                            }
                            var rule = result.AddRule(ruleType, attr, arg, inputAttr);
                            rule.ResourceType = (RuleResourceType)Enum.Parse(typeof(RuleResourceType),
                                xrul.Attribute(FatonConstants.XML_RESULT_RULE_RESOURCETYPE).Value);
                            if (xrul.Attribute("Default") != null) rule.Default = xrul.Attribute("Default").Value;
                        }

                        if (ruleType == Result.RuleType.FUNC)
                        {
                            Functor fun = FunctorFactory.Build(xrul.Attribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_NAME).Value);
                            fun.CID = UID.Take(uint.Parse(xrul.Attribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_ID).Value));
                            var inputs = xrul.Elements(FatonConstants.XML_RESULT_RULE_FUNCTOR_INPUT);
                            fun.Inputs.Clear();
                            foreach (var xinput in inputs)
                            {
                                var resourceType = (RuleResourceType)Enum.Parse(typeof(RuleResourceType),
                                    xinput.Attribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_RESOURCETYPE).Value);
                                ISchemeComponent resource = null;
                                OntologyNode.Attribute value = null;
                                if (resourceType == RuleResourceType.ARG)
                                {
                                    resource = scheme.Arguments.Find(x => x.Order == int.Parse(xinput.Attribute(FatonConstants.XML_RESULT_RULE_RESOURCE).Value));
                                    value = ((Argument)resource).Attributes.Find(x => x.Name == xinput.Attribute(FatonConstants.XML_RESULT_RULE_FUNCTOR_ATTRFROM).Value);
                                }
                                else { };
                                var input = new Functor.FunctorInput("input");
                                input.Set(value, resource);
                                fun.Inputs.Add(input);
                            }
                            scheme.Components.Add(fun);
                            var rule = result.AddRule(ruleType, attr, fun, fun.Output);
                            if (xrul.Attribute("Default") != null) rule.Default = xrul.Attribute("Default").Value;
                        }
                    }
                    if (xres.Attribute(FatonConstants.XML_RESULT_ARGEDIT) != null)
                    {
                        result.Type = ResultType.EDIT;
                        result.EditObject = scheme.Arguments.Find(x => x.Order == int.Parse(xres.Attribute(FatonConstants.XML_RESULT_ARGEDIT).Value));
                    }
                    
                }

                foreach (var xcomplex in conditionComplexes)
                {
                    var argName1 = uint.Parse(xcomplex.Attribute("Arg1").Value);
                    var argName2 = uint.Parse(xcomplex.Attribute("Arg2").Value);
                    var conditions = xcomplex.Elements();
                    foreach (var xcond in conditions)
                    {
                        var cond = scheme.AddCondition();
                        cond.ID = uint.Parse(xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_ID).Value);
                        cond.Type = (ConditionType)Enum.Parse(typeof(ConditionType), xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_TYPE).Value);
                        cond.Operation = (ConditionOperation)Enum.Parse(typeof(ConditionOperation), xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_OPERATION).Value);

                        var arg1 = scheme.Arguments.Find(x => x.Order == argName1);
                        var arg2 = scheme.Arguments.Find(x => x.Order == argName2);
                        cond.Arg1 = arg1;
                        cond.Arg2 = arg2;
                        cond.Data = xcond.Attribute(FatonConstants.XML_ARGUMENT_CONDITION_DATA).Value;
                    }
                }
                bank.Schemes.Add(scheme);
            }

            return bank;
        }
    }
}
