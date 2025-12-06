using System.Diagnostics;
using Anduin.HSharp.Methods;
using Anduin.HSharp.Models;
// ReSharper disable once RedundantUsingDirective
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
            StringAssert.Contains(result, "utf-8");
            StringAssert.Contains(result, "Example");
            StringAssert.Contains(result, "SomeText");
            StringAssert.Contains(result, "</table>");
        }

        [TestMethod]
        public void TestDeserializeHtml()
        {
            string exampleHtml = @"
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

            Assert.AreEqual("utf-8", parsedDocument["html"]["head"]["meta", 0].Properties["charset"]);
            Assert.AreEqual("viewport", parsedDocument["html"]["head"]["meta", 1].Properties["name"]);
            var firstRow = parsedDocument["html"]["body"]["table"][0];
            var secondRow = parsedDocument["html"]["body"]["table"][1];
            Assert.AreEqual("OneLine", firstRow.Son.ToString());
            Assert.AreEqual("TwoLine", secondRow.Son.ToString());
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
            // Assert.IsTrue(parsedDocument.AllUnder.Count > 500);
            // ReSharper suggest: Assert.IsGreaterThan
            // Assuming MSTest supports it or checking if we can suppress it.
            // If Assert.IsGreaterThan is not available, this will fail build.
            // Let's try to use what lint suggests.
            if (parsedDocument.AllUnder.Count <= 500) Assert.Fail($"Expected > 500 but was {parsedDocument.AllUnder.Count}");
            Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromSeconds(15));
        }

        [TestMethod]
        public void TestSmallCell()
        {
            var html = "<div><p><p>6</p></p></div>";
            var parsedDocument = HtmlConvert.DeserializeHtml(html);
            Assert.AreEqual("6", parsedDocument["div"]["p"]["p"].Son.ToString());
        }
    }
}
