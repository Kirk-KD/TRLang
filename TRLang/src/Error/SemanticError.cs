using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Error
{
    class SemanticError : Error
    {
        public SemanticError(string message) : base(message) { }
    }
}
