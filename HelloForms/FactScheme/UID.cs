using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactScheme
{
    public static class UID
    {
        static uint _global_id;
        static UID() { _global_id = 0; }
        public static uint Get() { return ++_global_id; }
        public static uint Take(uint id) { if (_global_id < id) _global_id = id; return id; }
    }
}
