using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public partial class FactScheme
    {

        public enum ConditionType { Semantic, Morphologic, Syntactic }
        public enum ConditionPosition { Any, Preposition, Postposition }
        public enum ConditionContact { Any, Absolute, Object, Group, ObjectGroup }
        public enum ComparisonType { Equal, NotEqual };
        public partial class Condition //TODO divie argument and location conditions
        {
            Argument _parentArgument;
            ConditionType _type;
            string _attribute;
            ComparisonType _comparison;
            string _value;

            public ConditionType ConditionType
            {
                get { return _type; }
                set { _type = value; }
            }

            public string Attribute
            {
                get { return _attribute; }
                set { _attribute = value; }
            }

            public ComparisonType ComparisonType
            {
                get { return _comparison; }
                set { _comparison = value; }
            }

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public Condition(ConditionType myCondType, string myAttr, ComparisonType myComparType, string myValue)
            {
                _type = myCondType;
                _attribute = myAttr;
                _comparison = myComparType;
                _value = myValue;
            }
            public Condition(Argument arg, string attrName = "")
            {
                _type = ConditionType.Morphologic;
                _attribute = attrName;
                _comparison = ComparisonType.Equal;
                _value = "";
                _parentArgument = arg;
            }
        }
    }
}
