using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Obisoft.HSharp
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
        public static HDoc DeserializeHtml(Uri Url)
        {
            var Document = new HDoc(Url);
            return Document;
        }
    }
}
