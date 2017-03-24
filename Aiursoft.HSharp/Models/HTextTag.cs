using System.Collections.Generic;

namespace Aiursoft.HSharp.Models
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
        public override string GenerateHTML() => InnerContent+" ";
        public override string ToString() => InnerContent;
        public override Dictionary<string, dynamic> DynamicData()
        {
            var Base = base.DynamicData();
            Base.Add(nameof(InnerContent), InnerContent);
            return Base;
        }
    }
}
