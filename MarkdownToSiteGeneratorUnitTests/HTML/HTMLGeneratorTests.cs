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
      public void MarkdownDocToHtmlDoc_SwapOutURLs()
      {
         string raw = @"Look at this [photograph](my_fav_page), every time it makes [me laugh](https://example.com)";

         var doc = new MarkdownParser().Parse(raw);

         Func<string, string> swapper = a => a == "my_fav_page" ? "/best-page.html" : a;

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw, swapper);

         AssertHTML.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertParagraph(s, new Action<HtmlSymbol>[]
            {
               pt => AssertHTML.AssertLiteralText(pt, "Look at this "),
               l => AssertHTML.AssertLink(l, "photograph", "/best-page.html"),
               pt => AssertHTML.AssertLiteralText(pt, ", every time it makes "),
               l => AssertHTML.AssertLink(l, "me laugh", "https://example.com")
            } )
         });



      }


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

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw, a=>a);

         AssertHTML.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertParagraph(s,"Some top paragraph text."),
            s=>AssertHTML.AssertHeading(s, 1, "Level 1"),
            s=>AssertHTML.AssertParagraph(s,"Some more paragraph text"),
            s=>AssertHTML.AssertHeading(s, 2, "Subheading"),
            s=>AssertHTML.AssertParagraph(s,"Final paragraph text")
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

         AssertHTML.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertParagraph(s,"Some top paragraph text."),
            s=>AssertHTML.AssertOrderedList(s, new string[]{"first list item one", "First list second"}),
            s=>AssertHTML.AssertParagraph(s,"Some more paragraph text"),
            s=>AssertHTML.AssertOrderedList(s, new string[]{"My list 1", "my second item"}),
            s=>AssertHTML.AssertParagraph(s,"Final paragraph text")
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

         AssertHTML.AssertMetadata((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertMetadata(s,"title","Home"),
            s=>AssertHTML.AssertMetadata(s,"keywords","homepage c# markdown"),
         });
         AssertHTML.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertHeading(s, 1, "Top Section")
         });
      }

      [TestMethod]
      public void MarkdownDocToHtmlDoc_ParagraphWithLink()
      {
         string raw =@"Look at this [photograph](https://example.com/photograph)";

         var doc = new MarkdownParser().Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertHTML.AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertHTML.AssertParagraph(s, new Action<HtmlSymbol>[]
            {
               pt => AssertHTML.AssertLiteralText(pt, "Look at this "),
               l => AssertHTML.AssertLink(l, "photograph", "https://example.com/photograph")
            } )
         });
      }

   }
}