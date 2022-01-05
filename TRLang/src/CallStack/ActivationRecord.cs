using System;
using System.Collections.Generic;

namespace TRLang.src.CallStack
{
    public class ActivationRecord
    {
        public string Name { get; private set; }
        public ARType Type { get; private set; }
        public int Level { get; private set; }

        private readonly Dictionary<string, object> _members = new Dictionary<string, object>();

        public ActivationRecord(string name, ARType type, int level)
        {
            this.Name = name;
            this.Type = type;
            this.Level = level;
        }

        public void Set(string key, object val)
        {
            this._members[key] = val;
        }

        public object Get(string key) => this._members[key];

        public bool ContainsKey(string key) => this._members.ContainsKey(key);

        public object GetOrNull(string key) => this.ContainsKey(key) ? this._members[key] : null;

        public override string ToString() => $"ActivationRecord(Name={this.Name}, Type={this.Type}, Level={this.Level})";

        public string GetContents()
        {
            string res = $"{this} contents:";
            foreach (KeyValuePair<string, object> entry in this._members) res += $"\n  {entry.Key} = {entry.Value} ({entry.Value.GetType()})";
            return res;
        }
    }
}
