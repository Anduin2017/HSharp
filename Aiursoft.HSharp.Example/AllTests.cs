using Aiursoft.HSharp.Methods;
using Aiursoft.HSharp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Aiursoft.HSharp.Example
{
    [TestClass]
    public class AllTests
    {
        [TestMethod]
        public void TestGenerateHtml()
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
            Assert.AreEqual(result, @"<html>
<head>
<meta charset=""utf-8""></meta><title>
Example </title>
</head>
<body>
<div>
<a href=""/#""></a><table>
<tr></tr><tr>
<td>
SomeText </td>
</tr>
<tr></tr></table>
</div>
</body>
</html>
");
        }

        [TestMethod]
        public void TestDeserializeHtml()
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

            Assert.AreEqual(parsedDocument["html"]["head"]["meta", 0].Properties["charset"], "utf-8");
            Assert.AreEqual(parsedDocument["html"]["head"]["meta", 1].Properties["name"], "viewport");
            var firstRow = parsedDocument["html"]["body"]["table"][0];
            var secondRow = parsedDocument["html"]["body"]["table"][1];
            Assert.AreEqual(firstRow.Son.ToString(), "OneLine");
            Assert.AreEqual(secondRow.Son.ToString(), "TwoLine");
        }

        [TestMethod]
        public void TestPerformance()
        {
            var real = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(real + Path.DirectorySeparatorChar + "test.html");
            var startTime = DateTime.UtcNow;
            var parsedDocument = HtmlConvert.DeserializeHtml(file);
            var endTime = DateTime.UtcNow;
            Assert.IsTrue(parsedDocument.AllUnder.Count > 500);
            Assert.IsTrue(endTime - startTime < TimeSpan.FromSeconds(7));
        }

        [TestMethod]
        public void TestSmallCell()
        {
            var html = "<div><p><p>6</p></p></div>";
            var parsedDocument = HtmlConvert.DeserializeHtml(html);
            Assert.AreEqual(parsedDocument["div"]["p"]["p"].Son.ToString(), "6");
        }
    }
}
