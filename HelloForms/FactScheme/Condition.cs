using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactScheme
{
    public class Condition : ISchemeComponent
    {
        public enum ConditionPosition { ANY, POSTPOSITION, PREPOSITION }
        public enum ConditionContact { ANY, ABS, OBJ, GROUP, OBJGROUP }
        public enum ComparisonType { EQ, NEQ };

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
