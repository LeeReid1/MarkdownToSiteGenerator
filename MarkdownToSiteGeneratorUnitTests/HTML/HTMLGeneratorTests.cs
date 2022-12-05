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

         MarkdownParser op = new();
         var doc = op.Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertParagraph(s,"Some top paragraph text."),
            s=>AssertHeading(s, 1, "Level 1"),
            s=>AssertParagraph(s,"Some more paragraph text"),
            s=>AssertHeading(s, 2, "Subheading"),
            s=>AssertParagraph(s,"Final paragraph text")
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

         MarkdownToSiteGenerator.Markdown.MarkdownParser op = new();
         var doc = op.Parse(raw);

         HtmlSymbol made = HTMLGenerator.ToHTMLSymbols(doc, raw);

         AssertDocument((HtmlDocument)made, new Action<HtmlSymbol>[]
         {
            s=>AssertParagraph(s,"Some top paragraph text."),
            s=>AssertOrderedList(s, new string[]{"first list item one", "First list second"}),
            s=>AssertParagraph(s,"Some more paragraph text"),
            s=>AssertOrderedList(s, new string[]{"My list 1", "my second item"}),
            s=>AssertParagraph(s,"Final paragraph text")
         });
      }

      private void AssertDocument(HtmlDocument doc, Action<HtmlSymbol>[] childVerifications)
      {
         Assert.IsNotNull(doc);
         Assert.AreEqual(doc.Children.Count, childVerifications.Length);
         for (int i = 0; i < childVerifications.Length; i++)
         {
            childVerifications[i].Invoke(doc.Children[i]);
         }
      }

      private void AssertParagraph(HtmlSymbol symb, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Paragraph));
         AssertPlainContent(symb, text);
      }

      private static void AssertPlainContent(HtmlSymbol symb, string text)
      {
         Assert.AreEqual(1, symb.Children.Count);
         Assert.IsInstanceOfType(symb.Children[0], typeof(MarkdownToSiteGenerator.HTML.LiteralText));
         Assert.AreEqual(text, ((MarkdownToSiteGenerator.HTML.LiteralText)symb.Children[0]).ToString());
      }

      private void AssertHeading(HtmlSymbol symb, byte level, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Heading));
         Assert.AreEqual(level, ((MarkdownToSiteGenerator.HTML.Heading)symb).Level);

         AssertPlainContent(symb, text);
      }
      private void AssertOrderedList(HtmlSymbol symb, IList<string> text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.List));

         Assert.AreEqual(symb.Children.Count, text.Count);

         for (int i = 0; i < text.Count; i++)
         {
            Assert.IsInstanceOfType(symb.Children[i], typeof(MarkdownToSiteGenerator.HTML.ListItem));
            AssertPlainContent(symb.Children[i], text[i]);
         }
      }
   }
}