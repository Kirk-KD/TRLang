using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src
{
    class Utilities
    {
        public static string SmartCharToString(char c)
        {
            return c switch
            {
                '\t' => "\\t",
                '\n' => "\\n",
                '\r' => "\\r",
                '\0' => "\\0",
                _ => c.ToString()
            };
        }
    }
}
