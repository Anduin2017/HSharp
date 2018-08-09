using Aiursoft.HSharp.Models;
using System;

namespace Aiursoft.HSharp.Example
{
    class Program
    {
        public static void Example1()
        {
            var document = new HDoc(DocumentOptions.BasicHTML);
            document["html"]["body"].AddChild("div");
            document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
            document["html"]["body"]["div"].AddChild("table");
            document["html"]["body"]["div"]["table"].AddChildren(
             new HTag("tr"),
             new HTag("tr", new HTag("td", "SomeText")));
            var result = HtmlConvert.SerializeHtml(document);

            Console.WriteLine(result);
        }
        public static void Example2()
        {
            string exampleHtml = $@"
        <html>
            <head>
                <meta charset={"\"utf-8\""}>
                <meta name={"\"viewport\""}>
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
            }
        }
        public static void Main(string[] args)
        {
            //There are two examples
            Example1();
            Example2();
            Console.ReadLine();
        }
    }
}
