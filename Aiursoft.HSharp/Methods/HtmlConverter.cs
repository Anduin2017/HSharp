using System;
using Aiursoft.HSharp.Models;

namespace Aiursoft.HSharp
{
    public static class HtmlConvert
    {
        public static string SerializeHtml(HDoc Document)
        {
            return Document.GenerateHTML();
        }
        public static HDoc DeserializeHtml(string HTML)
        {
            var Document = new HDoc(HTML);
            return Document;
        }
        public static dynamic DeserializeHtmlDynamic(string HTML)
        {
            var Document = new HDoc(HTML);
            return Document.Root;
        }
    }
}
