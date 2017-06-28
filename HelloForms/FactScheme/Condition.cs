using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology;

namespace FactScheme
{
    public class Condition : ISchemeComponent
    {
        public enum ConditionType { SEG, POS, CONTACT, SEM, SYNT, MORPH}
        public enum ConditionPosition { ANY, PRE_FORCED, PRE_PRIOR, POST_FORCED, POST_PRIOR}
        public enum ConditionContact { ANY, ABS, OBJ, GROUP, OBJECT_GROUP }
        public enum ComparisonType { EQ, NEQ };

        //various condition properties
        public ConditionType Type { get; set; }
        public string Segment { get; set; } //SEG
        public OntologyNode.Attribute[] SemAttrs; //SEM
        public string[] ActantNames; //SYNT
        public string MorphAttr; //MORPH
        public ConditionPosition Position { get; set; }
        public ConditionContact Contact { get; set; }
        public ComparisonType ComparType { get; set; }

        public Argument[] Args { get; }

        public Condition()
        {
            Args = new Argument[2];
            SemAttrs = new OntologyNode.Attribute[2];
            ActantNames = new string[2];
        }

        public List<ISchemeComponent> Up()
        {
            var components = new List<ISchemeComponent>();
            foreach (var arg in Args)
            {
                if (arg != null)
                    components.Add(arg);
            }
            return components;
        }

        public void RemoveUpper(ISchemeComponent component)
        {
            for (int i = 0; i < Args.Length; i++)
                if (Args[i] == component)
                {
                    Args[i] = null;
                    return;
                }
        }

        public void Free(object attribute)
        {
            throw new NotImplementedException();
        }
    }
}
