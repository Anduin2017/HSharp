using Aiursoft.HSharp.Models;

namespace Aiursoft.HSharp.Methods
{
    public static class HtmlConvert
    {
        public static string SerializeHtml(HDoc document)
        {
            return document.GenerateHtml();
        }
        public static HDoc DeserializeHtml(string html)
        {
            return new HDoc(html);
        }
    }
}
