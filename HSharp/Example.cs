using Obisoft.HSharp.Models;
using System;

namespace Obisoft.HSharp
{
    class Example
    {
        public static void Example1()
        {
            var Document = new HDoc(DocumentOptions.BasicHTML);
            Document["html"]["body"].AddChild("div");
            Document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
            Document["html"]["body"]["div"].AddChild("table");
            Document["html"]["body"]["div"]["table"].AddChildren(
             new HTag("tr"),
             new HTag("tr", "SomeText"),
             new HTag("tr", new HTag("td")));
            var Result = Document.GenerateHTML();
            Console.WriteLine(Result);
        }
        public static void Example2()
        {
            var NewDocument = HtmlConvert.DeserializeHtml($@"
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
</html>");
            Console.WriteLine(NewDocument["html"]["head"]["meta", 0].Properties["charset"]);
            Console.WriteLine(NewDocument["html"]["head"]["meta", 1].Properties["name"]);
            foreach (var Line in NewDocument["html"]["body"]["table"])
            {
                Console.WriteLine(Line.Son);
            }
        }
        public static void Example3()
        {
            var WebSiteDocument = new HDoc(new Uri("https://www.obisoft.com.cn"));
            Console.WriteLine(WebSiteDocument["html"]["head"]["title"].Children[1]);
            Console.WriteLine(WebSiteDocument.FindTagById("service")["div"]["div"]["div"]["div"]["h3"]["b"].Son);
            Console.WriteLine(WebSiteDocument.FindTagByTagName("nav").GenerateHTML());
        }
        public static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
            Console.ReadLine();
        }
    }
}
