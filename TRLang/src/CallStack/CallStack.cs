using System;
using System.Collections.Generic;

namespace TRLang.src.CallStack
{
    public class CallStack
    {
        private readonly List<ActivationRecord> _records = new List<ActivationRecord>();

        public void Push(ActivationRecord ar)
        {
            Log($"Push: {ar}");
            this._records.Add(ar);
        }

        public ActivationRecord Pop()
        {
            ActivationRecord last = this.Peek();
            Log($"Pop: {last}");

            this._records.RemoveAt(this._records.Count - 1);
            return last;
        }

        public ActivationRecord Peek()
        {
            ActivationRecord ar = this._records[^1];
            Log($"Peek: {ar}");

            return ar;
        }

        public override string ToString()
        {
            string res = "CallStack contents:";
            for (int i = 0; i < this._records.Count; i++) res += $"\n  {this._records[i].GetContents().Replace("\n", "\n  ")}";

            return res;
        }

        private static void Log(string message)
        {
            if (Flags.LogCallStack) Console.WriteLine($"CallStack: {message}");
        }
    }
}
