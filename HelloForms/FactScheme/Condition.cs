using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ontology;
using Faton;

namespace FactScheme
{

    public enum ConditionType { SEG, POS, CONTACT, SEM, SYNT, MORPH }
    public enum ConditionPosition { ANY, PRE_FORCED, PRE_PRIOR, POST_FORCED, POST_PRIOR }
    public enum ConditionContact { ANY, ABS, OBJ, GROUP, OBJECT_GROUP }
    public enum ConditionOperation { EQ, NEQ };

    public class Condition : ISchemeComponent
    {

        //various condition properties
        public uint ID { get; set; }
        public ConditionType Type { get; set; }
        public ConditionOperation Operation { get; set; }
        public string Data { get; set; }
        public Argument Arg1 { get; set; }
        public Argument Arg2 { get; set; }

        public List<ISchemeComponent> Up()
        {
            var components = new List<ISchemeComponent>();
            if (Arg1 != null)
                components.Add(Arg1);
            if (Arg2 != null)
                components.Add(Arg2);
            return components;
        }

        public void RemoveUpper(ISchemeComponent component)
        {
            if (Arg1 == component)
                Arg1 = null;
            else if (Arg2 == component)
                Arg2 = null;
        }

        public void Free(object attribute)
        {
            throw new NotImplementedException();
        }
    }
}
