using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology;

namespace FactScheme
{
    //public partial class FactScheme
    //{
    public class Argument : ISchemeComponent, INotifyPropertyChanged
    {
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

        OntologyClass _klass;
        bool _useInheritance;
        protected uint _order;
        public List<ArgumentCondition> Сonditions { get; set; }

        public OntologyClass Klass
        {
            get { return _klass; }
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
            _klass = klass;
            _useInheritance = inherit;
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
