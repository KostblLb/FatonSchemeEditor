using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology;

namespace FactScheme
{
    //public partial class FactScheme
    //{
    public enum ResultType { Create, Edit }
    public partial class Result : ISchemeComponent
    {
        public enum RuleType { DEF, FUNC }
        public class Rule
        {
            RuleType _type;
            OntologyNode.Attribute _attr;
            OntologyNode.Attribute _inputAttr;
            ISchemeComponent _ref;
            //string _value; //int, enum also
            public RuleType Type { get { return _type; } }
            public OntologyNode.Attribute Attribute { get { return _attr; } }
            public ISchemeComponent Reference { get { return _ref; } }
            public OntologyNode.Attribute InputAttribute { get { return _inputAttr; } }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attr"></param>
            /// <param name="reference"></param>
            /// <param name="inputAttr">may be null if 'reference' is a functor</param>
            public Rule(RuleType type, OntologyNode.Attribute attr, ISchemeComponent reference, OntologyNode.Attribute inputAttr)
            {
                _type = type;
                _attr = attr;
                _ref = reference;
                _inputAttr = inputAttr;
                //_value = value;
            }
        }

        OntologyClass _reference;
        List<Rule> _rules;
        ResultType _type;
        String _name;

        public Result(String name, ResultType type, OntologyClass reference = null)
        {
            _name = name;
            _type = type;
            _rules = new List<Rule>();
            _reference = reference;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ResultType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public ISchemeComponent EditObject;

        public OntologyClass Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public List<Rule> Rules
        {
            get { return _rules; }
        }

        public void AddRule(RuleType type, OntologyNode.Attribute attr, ISchemeComponent reference, OntologyNode.Attribute inputAttr)//, string value)
        {
            Rule rule = new Rule(type, attr, reference, inputAttr);
            _rules.Add(rule);
        }

        #region ISchemeComponent implementation
        public List<ISchemeComponent> Up()
        {
            var components =  new List<ISchemeComponent>();
            foreach(Rule rule in Rules)
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
