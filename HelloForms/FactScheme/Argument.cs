using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Ontology;
using Faton;
using Shared;

namespace FactScheme
{
    public enum ArgumentType { IOBJECT, TERMIN };
    public enum ArgumentConditionType { SEM, SEG, MORPH }
    public enum ArgumentConditionOperation { EQ, NEQ, GT, LT, GEQ, LEQ };
    public class Argument : ISchemeComponent, INotifyPropertyChanged
    {

        public class ArgumentCondition
        {
            [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CONDITION_TYPE)]
            public ArgumentConditionType CondType { get; set; }
            [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CONDITION_OPERATION)]
            public ArgumentConditionOperation ComparType { get; set; }
            public string Value { get; set; }

            public ArgumentCondition()
            {
                Value = "";
                //Attribute = null;
            }
        }

        ArgumentType _argType;
        OntologyClass _klass;
        VocTheme _theme;
        List<OntologyNode.Attribute> _attrs;
        //string _name;
        bool _useInheritance;
        protected uint _order;

        [XmlIgnore]
        public Dictionary<OntologyNode.Attribute, List<ArgumentCondition>> Conditions { get; set; }
        [System.Xml.Serialization.XmlElement("Condition")]
        public List<ArgumentCondition> XMLConditions
        {
            get
            {
                var list = new List<ArgumentCondition>();
                foreach (var entry in Conditions)
                    list.AddRange(entry.Value);
                return list;
            }
        }

        [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_OBJECTTYPE)]
        public ArgumentType ArgType
        {
            get { return _argType; }
        }
        [XmlIgnore]
        public OntologyClass Klass
        {
            get { return _klass; }
        }
        [XmlIgnore]
        public VocTheme Theme
        {
            get { return _theme; }
        }
        [XmlIgnore]
        public List<OntologyNode.Attribute> Attributes
        {
            get { return _attrs; }
        }
        [XmlIgnore]
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

        [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_GROUPTYPE)] //correct?
        public bool Inheritance
        {
            get { return _useInheritance; }
            set { _useInheritance = value; }
        }

        [XmlAttribute]
        public uint Order
        {
            get { return _order; }
            set
            {
                _order = value;
                NotifyPropertyChanged("Order");
            }
        }

        public Argument()
        {
            Conditions = new Dictionary<OntologyNode.Attribute, List<ArgumentCondition>>();
        }

        public Argument(OntologyClass klass, string name = null, bool inherit = true) : this()
        {
            _argType = ArgumentType.IOBJECT;
            _klass = klass;
            _useInheritance = inherit;

            _attrs = klass.AllAttributes;
            foreach (var attr in _attrs)
            {
                Conditions.Add(attr, new List<ArgumentCondition>());
            }
        }

        public Argument(VocTheme theme) : this()
        {
            _argType = ArgumentType.TERMIN;
            _theme = theme;
            Conditions = new Dictionary<OntologyNode.Attribute, List<ArgumentCondition>>();
            _attrs = new List<OntologyNode.Attribute>();
            _attrs.Add(new OntologyNode.Attribute(OntologyNode.Attribute.AttributeType.STRING, theme.name));
            Conditions.Add(_attrs[0], new List<ArgumentCondition>());
        }

        #region ISchemeComponent implementation
        public List<ISchemeComponent> Up()
        {
            return new List<ISchemeComponent>(); // return empty list for arguments cant have inputs
        }
        public void RemoveUpper(ISchemeComponent upper) { }
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
