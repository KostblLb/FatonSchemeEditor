using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ontology;
using Faton;

namespace FactScheme
{
    //public partial class FactScheme
    //{
    public enum ResultType { CREATE, EDIT }
    public enum RuleResourceType { ARG, RES, FUN}

    public partial class Result : ISchemeComponent
    {
        public enum RuleType { DEF, FUNC, ATTR }
        public class Rule
        {
            private OntologyNode.Attribute _attribute;
            private ISchemeComponent _reference;
            [XmlIgnore]
            public string ReferenceName { get; private set; }
            [XmlIgnore]
            public string InputAttributeName { get; private set; }

            [XmlAttribute]
            public RuleResourceType ResourceType { get; set; }

            [XmlAttribute(AttributeName = FatonConstants.XML_RESULT_RULE_TYPE)]
            public RuleType Type { get; set; }

            [XmlIgnore]
            public ISchemeComponent Reference //arg or result or functor
            {
                get { return _reference; }
                set { _reference = value; }
            }
            [XmlAttribute(AttributeName = FatonConstants.XML_RESULT_RULE_RESOURCE)]
            public string XMLReference
            {
                get
                {
                    if (ResourceType == RuleResourceType.ARG)
                        return ((Argument)Reference).Order.ToString();
                    else if (ResourceType == RuleResourceType.RES)
                        return ((Result)Reference).Name;
                    else { return ""; }
                }
                set { ReferenceName = value; }
            }

            [XmlIgnore]
            public OntologyNode.Attribute Attribute {
                get { return _attribute; }
                set { _attribute = value; }
            }
            [XmlAttribute(AttributeName = FatonConstants.XML_RESULT_RULE_ATTR)]
            public string XMLAttribute
            {
                get { return _attribute.Name; }
                set {
                    if (ResourceType == RuleResourceType.ARG)
                        _attribute = ((Argument)Reference).Klass.AllAttributes.Find(x => x.Name.Equals(value));
                    else if (ResourceType == RuleResourceType.RES)
                        _attribute = ((Result)Reference).Reference.AllAttributes.Find(x => x.Name.Equals(value));
                    else { } //functor block
                }
            }

            [XmlIgnore]
            public OntologyNode.Attribute InputAttribute { get; set; }
            [XmlAttribute(AttributeName = FatonConstants.XML_RESULT_RULE_ATTRFROM)]
            public string XMLInputAttribute {
                get { return InputAttribute?.Name; }
                set { InputAttributeName = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attr"></param>
            /// <param name="reference"></param>
            /// <param name="inputAttr">may be null if 'reference' is a functor</param>
            public Rule(OntologyNode.Attribute attr, ISchemeComponent reference, OntologyNode.Attribute inputAttr)
            {
                if (reference is Argument)
                    Type = RuleType.ATTR;
                else if (reference is Functor)
                    Type = RuleType.FUNC;
                else
                    Type = RuleType.DEF;
                Attribute = attr;
                Reference = reference;
                InputAttribute = inputAttr;
            }
            public Rule()
            {
                Type = RuleType.DEF;
            }
        }

        OntologyClass _reference;
        List<Rule> _rules;
        ResultType _type;
        String _name;

        public Result()
        {
            _name = "newresult";
            _type = ResultType.CREATE;
            _rules = new List<Rule>();
            _reference = null;
        }

        public Result(String name, ResultType type, OntologyClass reference = null)
        {
            _name = name;
            _type = type;
            _rules = new List<Rule>();
            _reference = reference;
        }

        [XmlAttribute]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlAttribute]
        public ResultType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlIgnore]
        public ISchemeComponent EditObject;

        [XmlIgnore]
        public OntologyClass Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        [XmlAttribute(AttributeName = FatonConstants.XML_RESULT_CLASSNAME)]
        public string XMLReference
        {
            get { return _reference.Name; }
        }

        [XmlElement(ElementName = Faton.FatonConstants.XML_RESULT_RULE_TAG)]
        public List<Rule> Rules
        {
            get { return _rules; }
        }

        //why not just Rules.Add()?
        //FUG
        public Rule AddRule(RuleType type, OntologyNode.Attribute attr, ISchemeComponent reference, OntologyNode.Attribute inputAttr)//, string value)
        {
            Rule rule = new Rule(attr, reference, inputAttr);
            _rules.Add(rule);
            return rule;
        }

        #region ISchemeComponent implementation
        public List<ISchemeComponent> Up()
        {
            var components = new List<ISchemeComponent>();
            foreach (Rule rule in Rules)
            {
                components.Add(rule.Reference as ISchemeComponent);
            }
            return components;
        }

        public void RemoveUpper(ISchemeComponent upper)
        {
            var rulesCopy = new List<Rule>();
            rulesCopy.AddRange(Rules);
            foreach (var rule in rulesCopy)
                if (rule.Reference == upper)
                    Rules.Remove(rule);
            if (upper == EditObject)
                EditObject = null;
        }

        public void Free(object attribute)
        {
            //find rule with specified attribute and remove it
            var attr = attribute as OntologyNode.Attribute;
            if (attr == null)
                return;
            var rule = Rules.Find(x => x.Attribute == attr);
            Rules.Remove(rule);
        }
        #endregion
    }
    //}
}
