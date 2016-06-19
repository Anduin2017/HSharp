using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Obisoft.HSharp
{
    public class HProp
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public HProp(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}
