using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Error
{
    class ParserError : Error
    {
        public ParserError(string message) : base(message) { }
    }
}
