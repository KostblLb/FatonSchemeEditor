using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public partial class FactScheme
    {
        public enum ResultType { Create, Edit }
        public partial class Result
        {
            public enum RuleType { Define, Function }
            class Rule
            {
                RuleType _type;
                OntologyNode.Attribute _attr; // mb OntologyNode.Attribute?
                Argument _argFrom;
                string _value; //int, enum also

                public Rule(RuleType type, OntologyNode.Attribute attr, Argument argFrom, string value)
                {
                    _type = type;
                    _attr = attr;
                    _argFrom = argFrom;
                    _value = value;
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

            public void AddRule(RuleType type, OntologyNode.Attribute attr, Argument argFrom, string value)
            {
                Rule rule = new Rule(type, attr, argFrom, value);
                _rules.Add(rule);
            }
        }
    }
}
