using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HelloForms
{
    /// <summary>
    /// using this class fixes wpf floating point exceptions when using external dll's
    /// </summary>
    public class FloatingPointReset
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _fpreset();


        public static void Action()
        {
            // Reset the Floating Point (When called from External Application there was an Overflow exception)
            _fpreset();
        }
    }
}
