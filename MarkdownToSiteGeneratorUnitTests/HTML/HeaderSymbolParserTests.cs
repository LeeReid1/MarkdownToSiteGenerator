using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using System.Text;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   [TestClass]
   public class ListItemTests
   {

      [TestMethod]
      public void ToHTML()
      {
         string raw = "1. point 1";

         MarkdownToSiteGenerator.Markdown.OrderedListItemParser op = new OrderedListItemParser();
         var symbol = op.ToSymbolisedText(raw).First();

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(symbol, raw);

         Assert.IsInstanceOfType(made, typeof(MarkdownToSiteGenerator.HTML.ListItem));

         MarkdownToSiteGenerator.HTML.ListItem li = (MarkdownToSiteGenerator.HTML.ListItem)made;

         StringBuilder sb = new();

         Assert.AreEqual("<li>point 1</li>", li.Write(sb).ToString());
      }
   }
}