using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Error
{
    class InterpreterError : Error
    {
        public InterpreterError(string message) : base(message) { }
    }
}
