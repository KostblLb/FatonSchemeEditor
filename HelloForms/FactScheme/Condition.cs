using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactScheme
{
    public class Condition : ISchemeComponent
    {
        public enum ConditionPosition { ANY, PRE_FORCED, PRE_PRIOR, POST_FORCED, POST_PRIOR}
        public enum ConditionContact { ANY, ABS, OBJ, GROUP, OBJECT_GROUP }
        public enum ComparisonType { EQ, NEQ };

        public ConditionPosition CPosition { get; set; }
        public ConditionContact CContact { get; set; }

        public Argument Arg1 { get; set; }
        public Argument Arg2 { get; set; }

        public List<ISchemeComponent> Up()
        {
            throw new NotImplementedException();
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
