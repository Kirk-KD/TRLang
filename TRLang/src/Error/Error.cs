using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Error
{
    class Error : Exception
    {
        public override string Message { get; }

        public Error(string message)
        {
            this.Message = $"{this.GetType().Name}: {message}";
        }
    }
}
