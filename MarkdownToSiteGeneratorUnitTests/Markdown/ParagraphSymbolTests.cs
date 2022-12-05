using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class ParagraphSymbolTests
   {
      [TestInitialize]
      public void Initialize()
      {
      }

      
      [TestMethod]
      public void GetMatches_PriorAndPostEmptyLines()
      {
         var h = new  ParagraphSymbolParser();

         string text =
@"
My paragraph

My other paragraph
still the second paragraph
";
         var matches = h.GetMatches(text).ToArray();



         SymbolisedTextWithChildren[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(2, symbols.Length);
         AssertParagraph(text, symbols[0], "My paragraph");
         AssertParagraph(text, symbols[1], $"My other paragraph{Environment.NewLine}still the second paragraph");
      }
      
      [TestMethod]
      public void GetMatches_ThreeToFind()
      {
         var h = new  ParagraphSymbolParser();

         string text =
@"My paragraph

My other paragraph
still the second paragraph

final paragraph";

         SymbolisedTextWithChildren[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(3, symbols.Length);
         AssertParagraph(text, symbols[0], "My paragraph");
         AssertParagraph(text, symbols[1], $"My other paragraph{Environment.NewLine}still the second paragraph");
         AssertParagraph(text, symbols[2], $"final paragraph");
      }
      

      [TestMethod]
      public void GetMatches_RestrictedRange()
      {
         var h = new  ParagraphSymbolParser();

         string text =
@"My paragraph

My other paragraph
still the second paragraph

third paragraph

final paragraph";



         int startFrom = text.IndexOf("third paragraph");
         SymbolisedTextWithChildren[] symbols = h.ToSymbolisedText(text, startFrom, text.Length - startFrom).ToArray();
         Assert.AreEqual(2, symbols.Length);
         AssertParagraph(text, symbols[0], "third paragraph");
         AssertParagraph(text, symbols[1], $"final paragraph");
      }

      private void AssertParagraph(string source, SymbolisedTextWithChildren symb, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.Paragraph));
         AssertPlainContent(source, symb, text);
      }

      private static void AssertPlainContent(string source, MarkdownToSiteGenerator.SymbolisedTextWithChildren symb, string text)
      {
         Assert.AreEqual(1, symb.Items.Count);
         Assert.IsInstanceOfType(symb.Items[0], typeof(MarkdownToSiteGenerator.LiteralText));
         Assert.AreEqual(text, ((MarkdownToSiteGenerator.LiteralText)symb.Items[0]).GetContentFragments(source).Single());
      }
   }
}