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
            string _name;
            OntologyClass _klass;
            bool _useInheritance;
            protected uint _order; 
            List<Condition> _conditions;

            public OntologyClass Klass
            {
                get { return _klass; }
            }
            public string Name
            {
                get { return _name; }
                set { _name = value; }
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

            public Argument(OntologyClass klass, string name = null, bool inherit = true)
            {
                _klass = klass;
                _name = name != null ? name : klass.Name; // default name
                _useInheritance = inherit;
                //_tag = tag;
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
