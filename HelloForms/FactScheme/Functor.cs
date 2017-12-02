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
    public static class FunctorFactory
    {
        public static Functor Build(string fid)
        {
            switch (fid)
            {
                case "F_CONCAT":
                    return new FunctorCat();
                case "F_BASE":
                default:
                    return new Functor();
            }
        }
    }
    public class Functor : ISchemeComponent
    {
        public class FunctorInput
        {
            public string name; //public name for GUI
            public object source;
            public OntologyNode.Attribute value;
            public object param;

            public FunctorInput(string myName)
            {
                name = myName;
            }
            
            public void Set(OntologyNode.Attribute attr, ISchemeComponent attrSource)
            {
                this.value = attr;
                this.source = attrSource;
            }
        }

        protected string _name;
        public string Name { get { return _name; } }
        public uint CID { get; set; } //component id
        List<FunctorInput> _inputs;
        public OntologyNode.Attribute Output;
        protected int _numArgs; //args number, -1 for variable
        protected int _minArgs; //minimal present args number (some may be empty)

        public int NumArgs { get { return _numArgs; } }
        public int MinArgs { get { return _minArgs; } }
        public List<string> Params;
        protected object _defaultValue; //default output value
        public object DefaultValue { get { return _defaultValue; } }
        public List<FunctorInput> Inputs { get { return _inputs; } }

        public Functor()
        {
            CID = UID.Get();
            _name = "F_BASE";
            _inputs = new List<FunctorInput>();
            Params = new List<string>();
            Output = new OntologyNode.Attribute(OntologyNode.Attribute.AttributeType.STRING, "output");
            _numArgs = -1;
            _minArgs = 0;
            _defaultValue = null;
        }

        public Functor(int numArgs, int minArgs, string defaultInputName = "input") : this()
        {
            _numArgs = numArgs;
            _minArgs = minArgs;
            for (int i = 0; i < minArgs; i++)
                _inputs.Add(new FunctorInput(defaultInputName));
        }

        public List<ISchemeComponent> Up()
        {
            var components = new List<ISchemeComponent>();
            foreach (FunctorInput input in Inputs)
                components.Add(input.source as ISchemeComponent); //need to guarantee that sources are always IschemeComponents
            return components;
        }

        public void RemoveUpper(ISchemeComponent component)
        {
            throw new NotImplementedException();
        }

        public void Free(object attribute)
        {
            throw new NotImplementedException();
        }
    }
}
public class FunctorIncrement : FactScheme.Functor
{
    public FunctorIncrement() : base(1, 1)
    {
        _name = "F_INC";
        _defaultValue = 0;
    }
}

public class FunctorCat : FactScheme.Functor
{
    public FunctorCat() : base(-1, 2)
    {
        _name = "F_CONCAT";
        _defaultValue = "";
    }
    //}
}
