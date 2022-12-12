using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using MarkdownToSiteGeneratorUnitTests.HTML;
using System.Runtime.CompilerServices;

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
      public void GetMatches_WithLink()
      {
         var h = new  ParagraphSymbolParser();

         string text =@"My paragraph links to [my site](https://example.com) still the paragraph";

         SymbolisedText[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(1, symbols.Length);
         Assert.IsInstanceOfType(symbols[0], typeof(MarkdownToSiteGenerator.Paragraph));
         MarkdownToSiteGenerator.Paragraph symb = (MarkdownToSiteGenerator.Paragraph)symbols[0];
         Assert.AreEqual(3, symb.Children.Count());
         AssertHelp.AssertPlainContent(symb.Items[0], text, "My paragraph links to ");
         AssertHelp.AssertLink(symb.Items[1], text, "my site", "https://example.com");
         AssertHelp.AssertPlainContent(symb.Items[2], text, " still the paragraph");
      }
            
      [TestMethod]
      public void GetMultipleMatches_WithLink()
      {
         var h = new  ParagraphSymbolParser();

         string text =
@"You can use the menu bar to see pages in this website, or jump straight to the page about cat [history](some_page).

You can also take a look at the [site map](/sitemap.html)!".ReplaceLineEndings();

         SymbolisedText[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(2, symbols.Length);

         {
            Assert.IsInstanceOfType(symbols[0], typeof(MarkdownToSiteGenerator.Paragraph));
            MarkdownToSiteGenerator.Paragraph symb = (MarkdownToSiteGenerator.Paragraph)symbols[0];
            Assert.AreEqual(3, symb.Children.Count());
            AssertHelp.AssertPlainContent(symb.Items[0], text, "You can use the menu bar to see pages in this website, or jump straight to the page about cat ");
            AssertHelp.AssertLink(symb.Items[1], text, "history", "some_page");
            AssertHelp.AssertPlainContent(symb.Items[2], text, ".");
         }

         {
            Assert.IsInstanceOfType(symbols[1], typeof(MarkdownToSiteGenerator.Paragraph));
            MarkdownToSiteGenerator.Paragraph symb = (MarkdownToSiteGenerator.Paragraph)symbols[1];
            Assert.AreEqual(3, symb.Children.Count());
            AssertHelp.AssertPlainContent(symb.Items[0], text, "You can also take a look at the ");
            AssertHelp.AssertLink(symb.Items[1], text, "site map", "/sitemap.html");
            AssertHelp.AssertPlainContent(symb.Items[2], text, "!");
         }
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
".ReplaceLineEndings();

         SymbolisedText[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(2, symbols.Length);
         AssertHelp.AssertParagraph(text, symbols[0], "My paragraph");
         AssertHelp.AssertParagraph(text, symbols[1], $"My other paragraph{Environment.NewLine}still the second paragraph");
      }
      
      [TestMethod]
      public void GetMatches_ThreeToFind()
      {
         var h = new  ParagraphSymbolParser();

         string text =
@"My paragraph

My other paragraph
still the second paragraph

final paragraph".ReplaceLineEndings();

         SymbolisedText[] symbols = h.ToSymbolisedText(text).ToArray();
         Assert.AreEqual(3, symbols.Length);
         AssertHelp.AssertParagraph(text, symbols[0], "My paragraph");
         AssertHelp.AssertParagraph(text, symbols[1], $"My other paragraph{Environment.NewLine}still the second paragraph");
         AssertHelp.AssertParagraph(text, symbols[2], $"final paragraph");
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

final paragraph".ReplaceLineEndings();



         int startFrom = text.IndexOf("third paragraph");
         SymbolisedText[] symbols = h.ToSymbolisedText(text, startFrom, text.Length - startFrom).ToArray();
         Assert.AreEqual(2, symbols.Length);
         AssertHelp.AssertParagraph(text, symbols[0], "third paragraph");
         AssertHelp.AssertParagraph(text, symbols[1], $"final paragraph");
      }

   }
}