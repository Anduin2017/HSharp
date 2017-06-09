using Aiursoft.HSharp.Models;
using System;
using System.Threading.Tasks;

namespace Aiursoft.HSharp
{
    class Example
    {
        public static string ExampleHtml = $@"
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
        public static void Example1()
        {
            var Document = new HDoc(DocumentOptions.BasicHTML);
            Document["html"]["body"].AddChild("div");
            Document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
            Document["html"]["body"]["div"].AddChild("table");
            Document["html"]["body"]["div"]["table"].AddChildren(
             new HTag("tr"),
             new HTag("tr", new HTag("td", "SomeText")));
            var Result = HtmlConvert.SerializeHtml(Document);

            Console.WriteLine(Result);
        }
        public static void Example2()
        {
            var Document = HtmlConvert.DeserializeHtml(ExampleHtml);
            Console.WriteLine(Document["html"]["head"]["meta", 0].Properties["charset"]);
            Console.WriteLine(Document["html"]["head"]["meta", 1].Properties["name"]);
            foreach (var Line in Document["html"]["body"]["table"])
            {
                Console.WriteLine(Line.Son);
            }
        }
        public static void Main(string[] args)
        {
		//This is two examples
            Example1();
            Example2();
        }
    }
}
