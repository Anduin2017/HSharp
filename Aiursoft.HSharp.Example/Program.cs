using Aiursoft.HSharp.Methods;
using Aiursoft.HSharp.Models;
using System;
using System.IO;

namespace Aiursoft.HSharp.Example
{
    static class Program
    {
        private static void Example1()
        {
            var document = new HDoc(DocumentOptions.BasicHtml);

            document["html"]["body"].AddChild("div");
            document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
            document["html"]["body"]["div"].AddChild("table");
            document["html"]["body"]["div"]["table"].AddChildren(
                 new HTag("tr"),
                 new HTag("tr", new HTag(tagName: "td", innerContent: "SomeText")),
                 new HTag("tr")
            );
            var result = HtmlConvert.SerializeHtml(document);

            Console.WriteLine(result);
        }

        private static void Example2()
        {
            string exampleHtml = $@"
        <html>
            <head>
                <meta charset=""utf-8"">
                <meta name=""viewport"">
                <title>Example</title>
            </head>
            <body>
                Some Text
                <table>
                    <tr>OneLine</tr>
                    <tr>TwoLine</tr>
                    <tr>ThreeLine</tr>
                </table>
                Other Text
            </body>
        </html>";

            var parsedDocument = HtmlConvert.DeserializeHtml(exampleHtml);

            Console.WriteLine(parsedDocument["html"]["head"]["meta", 0].Properties["charset"]);
            Console.WriteLine(parsedDocument["html"]["head"]["meta", 1].Properties["name"]);

            foreach (var element in parsedDocument["html"]["body"]["table"])
            {
                Console.WriteLine(element.Son);
                Console.WriteLine(element.Son as HTextTag);
                Console.WriteLine(element.Parent.TagName);
            }
        }

        public static void Example3()
        {
            var real = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(real + Path.DirectorySeparatorChar + "test.html");
            var parsedDocument = HtmlConvert.DeserializeHtml(file);
        }

        public static void Example4()
        {
            var html = "<table><tr><td>6</td></tr></table>";
            var parsedDocument = HtmlConvert.DeserializeHtml(html);
            Console.WriteLine(parsedDocument["table"]["tr"]["td"].Son);
        }

        public static void Main()
        {
            Example1();
            Example2();
            Example3();
            Example4();
            Console.ReadLine();
        }
    }
}
