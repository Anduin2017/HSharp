using System;

namespace Obisoft.HSharp
{
    public class Example
    {
        public static void Main(string[] args)
        {
            #region Example1: Create HTML

            var Document = new HDoc(DocumentOptions.BasicHTML);
            Document["html"]["body"].AddChild("div");
            Document["html"]["body"]["div"].AddChild("a", new HProp("href", "/#"));
            //Document["html"]["body"]["div"].AddChild("table");
            //Document["html"]["body"]["div"]["table"].AddChildren(
            //    new HTag("tr"),
            //    new HTag("tr", "SomeText"),
            //    new HTag("tr", new HTag("td")));
            var Result = Document.GenerateHTML();

            Console.WriteLine(Result);

            #endregion

            Console.WriteLine();

            #region Example2: Deserialize HTML

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
            Console.WriteLine(NewDocument["html"]["head"]["meta",0].Properties["charset"]);
            Console.WriteLine(NewDocument["html"]["head"]["meta",1].Properties["name"]);
            foreach (var Line in NewDocument["html"]["body"]["table"])
            {
                Console.WriteLine(Line.Son);
            }
            #endregion

            Console.WriteLine();

            #region Example3: Deserialize WebSite
            var WebSiteDocument = new HDoc(new Uri("https://www.obisoft.com.cn"));
            Console.WriteLine(WebSiteDocument["html"]["head"]["title"].Children[1]);
            #endregion

            Console.ReadLine();
        }
    }

}
