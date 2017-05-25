using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology;
using KlanVocabularyExtractor;

namespace FactScheme
{
    //public partial class FactScheme
    //{
    public class Argument : ISchemeComponent, INotifyPropertyChanged
    {

        public enum ArgumentType { IOBJECT, TERMIN };

        public class ArgumentCondition
        {
            public enum ConditionType { SEM, SEG, MORPH, SYNT }
            public enum ComparisonType { EQ, NEQ };
            
            public ConditionType CondType { get; set; }
            public string Attribute { get; set; }
            public ComparisonType ComparType { get; set; }
            public string Value { get; set; }

            public ArgumentCondition()
            {
                Value = "";
                Attribute = null;
            }
        }

        ArgumentType _argType;
        OntologyClass _klass;
        VocTheme _theme;
        string _name;
        bool _useInheritance;
        protected uint _order;
        public List<ArgumentCondition> Сonditions { get; set; }

        public ArgumentType ArgType
        {
            get { return _argType; }
        }
        public OntologyClass Klass
        {
            get { return _klass; }
        }
        public VocTheme Theme
        {
            get { return _theme; }
        }
        public string Name
        {
            get
            {
                if (ArgType == ArgumentType.IOBJECT)
                    return Klass.Name;
                else
                    return Theme.name;
            }
        }
        public bool Inheritance
        {
            get { return _useInheritance; }
            set { _useInheritance = value; }
        }

        public uint Order
        {
            get { return _order; }
            set {
                _order = value;
                NotifyPropertyChanged("Order");
            }
        }

        public Argument(OntologyClass klass, string name = null, bool inherit = true)
        {
            _argType = ArgumentType.IOBJECT;
            _klass = klass;
            _useInheritance = inherit;
            Сonditions = new List<ArgumentCondition>();
        }

        public Argument(VocTheme theme)
        {
            _argType = ArgumentType.TERMIN;
            _theme = theme;
            Сonditions = new List<ArgumentCondition>();
        }

        #region ISchemeComponent implementation
        public List<ISchemeComponent> Up()
        {
            return new List<ISchemeComponent>(); // return empty list for arguments cant have inputs
        }
        public void RemoveUpper(ISchemeComponent upper) { }
        public List<Connection> Connections(ISchemeComponent other)
        {
            return new List<Connection>();
        }
        public void Free(object attr)
        {
            return;
        }
        #endregion
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    //}
}
