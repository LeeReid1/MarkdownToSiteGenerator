using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using System.Text;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   [TestClass]
   public class HTMLGeneratorTests
   {

      [TestMethod]
      public void MarkdownDocToHtmlDoc_ParagraphsAndHeadings()
      {
         string raw =
@"Some top paragraph text.

# Level 1

Some more paragraph text

## Subheading

Final paragraph text

";

         var doc = new MarkdownParser().Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertHelp.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHelp.AssertParagraph(s,"Some top paragraph text."),
            s=>AssertHelp.AssertHeading(s, 1, "Level 1"),
            s=>AssertHelp.AssertParagraph(s,"Some more paragraph text"),
            s=>AssertHelp.AssertHeading(s, 2, "Subheading"),
            s=>AssertHelp.AssertParagraph(s,"Final paragraph text")
         });
      }


      [TestMethod]
      public void MarkdownDocToHtmlDoc_ParagraphsAndOrderedLists()
      {
         string raw =
@"Some top paragraph text.

1. first list item one
2. First list second

Some more paragraph text

1. My list 1
2. my second item

Final paragraph text";

         var doc = new MarkdownParser().Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertHelp.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHelp.AssertParagraph(s,"Some top paragraph text."),
            s=>AssertHelp.AssertOrderedList(s, new string[]{"first list item one", "First list second"}),
            s=>AssertHelp.AssertParagraph(s,"Some more paragraph text"),
            s=>AssertHelp.AssertOrderedList(s, new string[]{"My list 1", "my second item"}),
            s=>AssertHelp.AssertParagraph(s,"Final paragraph text")
         });
      }

      [TestMethod]
      public void MarkdownDocToHtmlDoc_MetadataAndHeading()
      {
         string raw =
@"title: Home
keywords: homepage c# markdown

# Top Section
";

         var doc = new MarkdownParser().Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertHelp.AssertMetadata((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHelp.AssertMetadata(s,"title","Home"),
            s=>AssertHelp.AssertMetadata(s,"keywords","homepage c# markdown"),
         });
         AssertHelp.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHelp.AssertHeading(s, 1, "Top Section")
         });
      }

   }
}