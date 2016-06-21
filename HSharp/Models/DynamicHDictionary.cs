using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Obisoft.HSharp.Models
{
    class DynamicHDictionary : DynamicObject
    {
        private readonly Func<Dictionary<string, dynamic>> _HDicThunk;

        public DynamicHDictionary(Func<Dictionary<string, dynamic>> HDataThunk)
        {
            _HDicThunk = HDataThunk;
        }
        public override IEnumerable<string> GetDynamicMemberNames() => this.HData.Keys;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = HData[binder.Name];
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.HData[binder.Name] = value as dynamic;
            return true;
        }
        private Dictionary<string, dynamic> HData => this._HDicThunk();
    }
}
