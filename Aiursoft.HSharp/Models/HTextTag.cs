namespace Aiursoft.HSharp.Models
{
    public class HTextTag : HTag
    {
        public string InnerContent { get; set; }
        public HTextTag() : base(string.Empty)
        {
        }
        public HTextTag(string innerContent) : base(string.Empty)
        {
            InnerContent = innerContent;
        }
        public override string GenerateHtml() => InnerContent+" ";
        public override string ToString() => InnerContent;

    }
}
