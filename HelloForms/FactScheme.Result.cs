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
            enum RuleType { }
            class Rule
            {
                string _attr; // mb OntologyNode.Attribute?
                Argument _argFrom;
                string _value;
            }

            ResultType _type;
            List<Rule> _rules;
        }
    }
}
