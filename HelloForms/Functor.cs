using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloForms
{
    public class Functor
    {
        protected string _id;
        public string ID { get { return _id; } }
        public List<object> Inputs;
        public object Output;
        protected int _numArgs; //args number, -1 for infinite
        public int NumArgs { get { return _numArgs; } }
        public List<string> Params;
        protected object _defaultValue;
        public object DefaultValue { get { return _defaultValue; } }

        public Functor()
        {
            _id = "F_BASE";
            Inputs = new List<object>();
            Params = new List<string>();
            Output = null;
            _numArgs = -1;
            _defaultValue = null;
        }
    }

    public class FuncIncrement : Functor
    {
        public FuncIncrement() : base()
        {
            _id = "F_INC";
            _numArgs = 1;
            _defaultValue = 0;
        }
    }

    public class FunctorCat : Functor
    {
        public FunctorCat() : base()
        {
            _id = "F_CONCAT";
            _numArgs = 2;
            for (int i = 0; i < _numArgs; i++)
                Inputs.Add(null);
            _defaultValue = "";
        }
    }
}
