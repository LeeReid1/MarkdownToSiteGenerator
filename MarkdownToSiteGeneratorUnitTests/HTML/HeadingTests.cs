using MarkdownToSiteGenerator.HTML;
using System.Text;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   [TestClass]
   public class HeadingTests
   {

      [TestMethod]
      public void ToHTML()
      {
         string raw = "## Catch phrase";

         MarkdownToSiteGenerator.Markdown.HeaderSymbolParser op = new(2);
         var symbol = op.ToSymbolisedText(raw).First();

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(symbol, raw);

         Assert.IsInstanceOfType(made, typeof(MarkdownToSiteGenerator.HTML.Heading));

         MarkdownToSiteGenerator.HTML.Heading li = (MarkdownToSiteGenerator.HTML.Heading)made;

         StringBuilder sb = new();

         Assert.AreEqual("<h2>Catch phrase</h2>", li.Write(sb).ToString());
      }
   }
}