using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Faton;

namespace FactScheme
{
    public class ConditionComplex
    {
        [XmlAttribute]
        public uint Arg1 { get; set; }
        [XmlAttribute]
        public uint Arg2 { get; set; }
        [XmlElement(ElementName = FatonConstants.XML_CONDITIONCOMPLEX_CONDITION_TAG)]
        public List<Condition> Conditions { get; set; }

        public ConditionComplex()
        {
            Conditions = new List<Condition>();
        }
        public ConditionComplex(Argument a1, Argument a2) : this()
        {
            Arg1 = a1.Order;
            Arg2 = a2.Order;
        }
    }
}
