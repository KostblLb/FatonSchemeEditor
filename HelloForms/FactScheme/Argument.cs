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
    public enum ArgumentCompareType { EQUAL, PLUS_CHILD }
    public enum ArgumentConditionType { SEM, SEG, MORH }
    public enum ArgumentConditionOperation { EQ, NEQ };
    public class Argument : ISchemeComponent, INotifyPropertyChanged
    {

        public class ArgumentCondition
        {
            [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CONDITION_TYPE)]
            public ArgumentConditionType CondType { get; set; }
            [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CONDITION_OPERATION)]
            public ArgumentConditionOperation Operation { get; set; }
            [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CONDITION_DATA)]
            public string Data { get; set; }

            public ArgumentCondition()
            {
                Data = "";
                //Attribute = null;
            }
        }

        OntologyClass _klass;
        VocTheme _theme;
        List<OntologyNode.Attribute> _attrs;
        protected uint _order;

        [XmlIgnore]
        public string ClassName { get; private set; }

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

        [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_COMPARETYPE)]
        public ArgumentCompareType CompareType { get; set; }

        [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_OBJECTTYPE)]
        public ArgumentType ArgType { get; set; }
        [XmlIgnore]
        public OntologyClass Klass
        {
            get { return _klass; }
        }
        [XmlAttribute(AttributeName = FatonConstants.XML_ARGUMENT_CLASSNAME)]
        public string XMLClassName
        {
            get { return _klass.Name; }
            set { ClassName = value; }
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
            CompareType = ArgumentCompareType.EQUAL;
        }

        public Argument(OntologyClass klass, string name = null, bool inherit = true) : this()
        {
            ArgType = ArgumentType.IOBJECT;
            _klass = klass;

            _attrs = klass.AllAttributes;
            foreach (var attr in _attrs)
            {
                Conditions.Add(attr, new List<ArgumentCondition>());
            }
        }

        public Argument(VocTheme theme) : this()
        {
            ArgType = ArgumentType.TERMIN;
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
