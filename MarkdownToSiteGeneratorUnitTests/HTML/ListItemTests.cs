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

         MarkdownToSiteGenerator.Markdown.OrderedListItemParser op = new();
         var symbol = op.ToSymbolisedText(raw).First();

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(symbol, raw);

         Assert.IsInstanceOfType(made, typeof(MarkdownToSiteGenerator.HTML.ListItem));

         MarkdownToSiteGenerator.HTML.ListItem li = (MarkdownToSiteGenerator.HTML.ListItem)made;

         StringBuilder sb = new();

         Assert.AreEqual("<li>point 1</li>", li.Write(sb).ToString());
      }
   }
   
   
   [TestClass]
   public class MetadataTests
   {

      [TestMethod]
      public void ToHTML()
      {
         string raw = "nottitle: this is not my title!";

         MetadataParser op = new();
         var symbol = op.ToSymbolisedText(raw).First();

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(symbol, raw);

         Assert.IsInstanceOfType(made, typeof(MarkdownToSiteGenerator.HTML.Metadata));

         MarkdownToSiteGenerator.HTML.Metadata li = (MarkdownToSiteGenerator.HTML.Metadata)made;

         StringBuilder sb = new();

         Assert.AreEqual("<meta property=\"nottitle\" content=\"this is not my title!\">"+Environment.NewLine, li.Write(sb).ToString());
      }
      
      [TestMethod]
      public void Title()
      {
         // special case

         string raw = "title: this is my title!";

         MetadataParser op = new();
         var symbol = op.ToSymbolisedText(raw).First();

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(symbol, raw);

         Assert.IsInstanceOfType(made, typeof(MarkdownToSiteGenerator.HTML.Metadata));

         MarkdownToSiteGenerator.HTML.Metadata li = (MarkdownToSiteGenerator.HTML.Metadata)made;

         StringBuilder sb = new();

         Assert.AreEqual(@"<title>this is my title!</title>" + Environment.NewLine, li.Write(sb).ToString());
      }
   }

   
}