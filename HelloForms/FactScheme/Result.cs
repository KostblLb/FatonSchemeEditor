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
        public partial class Result
        {
            public enum RuleType { Define, Function }
            public class Rule
            {
                RuleType _type;
                OntologyNode.Attribute _attr;
                OntologyNode.Attribute _inputAttr;
                object _ref;
                //string _value; //int, enum also
                public RuleType Type { get { return _type; } }
                public OntologyNode.Attribute Attribute { get { return _attr; } }
                public object Reference { get { return _ref; } }
                public OntologyNode.Attribute InputAttribute { get { return _inputAttr; } }
                
                /// <summary>
                /// 
                /// </summary>
                /// <param name="type"></param>
                /// <param name="attr"></param>
                /// <param name="reference"></param>
                /// <param name="inputAttr">may be null if 'reference' is a functor</param>
                public Rule(RuleType type, OntologyNode.Attribute attr, Object reference, OntologyNode.Attribute inputAttr)
                {
                    _type = type;
                    _attr = attr;
                    _ref = reference;
                    _inputAttr = inputAttr;
                    //_value = value;
                }
            }

            Object _reference;
            List<Rule> _rules;
            ResultType _type;
            String _name;

            public Result(String name, ResultType type, Object reference = null)
            {
                _name = name;
                _type = type;
                _rules = new List<Rule>();
                if(reference == null)
                {
                    reference = OntologyNode.Ontology.First(x => x.type == OntologyNode.Type.Class);
                }
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
            }

            public Object Reference
            {
                get { return _reference; }
                set { _reference = value; }
            }

            public List<Rule> Rules
            {
                get { return _rules; }
            }

            public void AddRule(RuleType type, OntologyNode.Attribute attr, Object reference, OntologyNode.Attribute inputAttr)//, string value)
            {
                Rule rule = new Rule(type, attr, reference, inputAttr);
                _rules.Add(rule);
            }
        }
    //}
}
