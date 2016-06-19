using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Obisoft.HSharp
{
    class Debug
    {
        public static void Main(string[] args)
        {
            #region Example1: Create HTML

            var Document = new Document(DocumentOptions.BasicHTML);
            Document["html"]["body"].AddChild("div");
            Document["html"]["body"]["div"].AddChild("a", new Property("href", "/#"));
            Document["html"]["body"]["div"].AddChild("table");
            Document["html"]["body"]["div"]["table"].AddChildren(
                new Tag("tr"),
                new Tag("tr", "SomeText"),
                new Tag("tr", new Tag("td")));
            var Result = Document.GenerateHTML();

            Console.WriteLine(Result);

            #endregion

            Console.WriteLine();

            #region Example2: Deserialize HTML

            var NewDocument = HtmlConvert.DeserializeHtml($@"
<html>
<head>
    <meta charset={"\"utf-8\""}></meta>
    <meta name={"\"viewport\""}></meta>
    <title>Example</title>
</head>
<body>
    <table>
        <tr>OneLine</tr>
        <tr>TwoLine</tr>
        <tr>ThreeLine</tr>
    </table>
</body>
</html>");
            Console.WriteLine(NewDocument["html"]["head"]["meta",0].Properties["charset"]);
            Console.WriteLine(NewDocument["html"]["head"]["meta",1].Properties["name"]);
            foreach (var Line in NewDocument["html"]["body"]["table"])
            {
                Console.WriteLine(Line.Children[0]);
            } 

            #endregion

            Console.ReadLine();
        }
    }

}
