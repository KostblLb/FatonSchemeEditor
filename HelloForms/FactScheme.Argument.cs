using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public partial class FactScheme
    {
		public partial class Argument
        {
            string _objectName;
            bool _useInheritance;
            protected uint _order; //just in case
            OntologyNode _tag; //attach real object to argument
            List<Condition> _conditions;

            public string TypeString
            {
                get { return _tag.TypeString; }
            }

            public string Name
            {
                get { return _objectName; }
            }

            public bool Inheritance
            {
                get { return _useInheritance; }
                set { _useInheritance = value; }
            }

            public uint Order
            {
                get { return _order; }
                set { _order = value; }
            }

            public List<Condition> Conditions
            {
                get { return _conditions; }
            }

            /// <summary>
            /// Get Ontology Class, Domain or Relation attached to this object
            /// </summary>
            public OntologyNode Tag
            {
                get { return _tag; }
            }

            public Argument(string myName, bool myInherit, OntologyNode myTag = null)
            {
                _objectName = myName;
                _useInheritance = myInherit;
                _tag = myTag;
                _conditions = new List<Condition>();
            }
			
			public Condition AddContition(string attrName = "")
            {
                Condition cond = new Condition(this, attrName);
                _conditions.Add(cond);
                return cond;
            }
        }
    }
}
