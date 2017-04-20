using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactScheme
{
    public struct Connection
    {
        public object src;
        public object dst;
        public Connection(object mysrc, object mydst)
        {
            src = mysrc; dst = mydst;
        }
    }
    public interface ISchemeComponent
    {
        //go up in objects tree
        List<ISchemeComponent> Up();

        void RemoveUpper(ISchemeComponent component);
    }
}
