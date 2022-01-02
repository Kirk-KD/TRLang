using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLang.src.Interpreter
{
    class InterpreterVisitResult
    {
        public bool HasReturn { get; private set; } = false;
        public int? IntValue { get; private set; } = null;
        public float? FloatValue { get; private set; } = null;

        public InterpreterVisitResult() { }

        public InterpreterVisitResult(int intValue)
        {
            this.HasReturn = true;
            this.IntValue = intValue;
        }

        public InterpreterVisitResult(float floatValue)
        {
            this.HasReturn = true;
            this.FloatValue = floatValue;
        }

        public bool HasValue<T>(T value) => this.HasReturn && value != null;

        public override string ToString() => $"VisitResult({(this.HasReturn ? (this.IntValue ?? this.FloatValue).ToString() : "<Empty>")})";
    }
}
