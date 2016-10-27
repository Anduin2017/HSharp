using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Obisoft.HSharp.Models
{
    class DynamicHDictionary : DynamicObject
    {
        private readonly Func<Dictionary<string, dynamic>> _HDicThunk;
        private Dictionary<string, dynamic> HData => _HDicThunk();
        public DynamicHDictionary(Func<Dictionary<string, dynamic>> HDataThunk)
        {
            _HDicThunk = HDataThunk;
        }
        public override IEnumerable<string> GetDynamicMemberNames() => HData.Keys;
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = HData[binder.Name];
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            HData[binder.Name] = value as dynamic;
            return true;
        }
    }
}