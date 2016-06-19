using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Obisoft.HSharp
{
    public class HTextTag : HTag
    {
        public string InnerContent { get; set; }
        public HTextTag() : base(string.Empty)
        {
        }
        public HTextTag(string InnerContent) : base(string.Empty)
        {
            this.InnerContent = InnerContent;
        }
        public override string GenerateHTML() => InnerContent;
        public override string ToString() => InnerContent;
    }
}
