using System.Diagnostics;
using Anduin.HSharp.Methods;
using Anduin.HSharp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anduin.HSharp.Tests
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
            Assert.IsTrue(result.Contains("utf-8"));
            Assert.IsTrue(result.Contains("Example"));
            Assert.IsTrue(result.Contains("SomeText"));
            Assert.IsTrue(result.Contains("</table>"));
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var parsedDocument = HtmlConvert.DeserializeHtml(file);
            stopwatch.Stop();
            Assert.IsTrue(parsedDocument.AllUnder.Count > 500);
            Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromSeconds(15));
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
