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
        public class Functor
        {
            public class FunctorInput
            {
                //public object defaultValue;
                public object value;
                public object source;
                public object param;
            }

            protected string _id;
            public string ID { get { return _id; } }
            List<FunctorInput> _inputs;
            public OntologyNode.Attribute Output;
            protected int _numArgs; //args number, -1 for variable
            //protected int _countArgs; //number of added args
            public int NumArgs { get { return _numArgs; } }
            public List<string> Params;
            protected object _defaultValue; //default output value
            public object DefaultValue { get { return _defaultValue; } }
            public List<FunctorInput> Inputs { get { return _inputs; } }

            public Functor()
            {
                _id = "F_BASE";
                _inputs = new List<FunctorInput>();
                Params = new List<string>();
                Output = null;
                _numArgs = -1;
                _defaultValue = null;
            }

            public Functor(int numArgs) : base()
            {
                _numArgs = numArgs;
                for (int i = 0; i < numArgs; i++)
                    _inputs.Add(new FunctorInput());
            }

            public void SetInput(object attr, object attrSource, FunctorInput input)
            {
                input.value = attr;
                input.source = attrSource;
            }
        }
    }
    public class FunctorIncrement : FactScheme.Functor
    {
        public FunctorIncrement() : base(1)
        {
            _id = "F_INC";
            _defaultValue = 0;
        }
    }

    public class FunctorCat : FactScheme.Functor
    {
        public FunctorCat() : base()
        {
            _id = "F_CONCAT";
            _defaultValue = "";
        }
    //}
}
