using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   internal class AssertHelp
   {

      internal static void AssertParagraph(string source, SymbolisedText symb, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.Paragraph));
         AssertContainsPlainContent(source, (MarkdownToSiteGenerator.Paragraph)symb, text);
      }

      internal static void AssertContainsPlainContent(string source, MarkdownToSiteGenerator.SymbolisedTextWithChildren symb, string text)
      {
         Assert.AreEqual(1, symb.Items.Count);
         var item = symb.Items[0];
         AssertPlainContent(item, source, text);
      }

      internal static void AssertPlainContent(ISymbolisedText item, string source, string text)
      {
         Assert.IsInstanceOfType(item, typeof(MarkdownToSiteGenerator.LiteralText));
         Assert.AreEqual(text, ((MarkdownToSiteGenerator.LiteralText)item).ExtractContent(source).ToString());
      }
      internal static void AssertLink(ISymbolisedText item, string source, string text, string href)
      {
         Assert.IsInstanceOfType(item, typeof(MarkdownToSiteGenerator.Link));
         Assert.AreEqual(href, ((MarkdownToSiteGenerator.Link)item).GetHref(source).ToString());
         AssertContainsPlainContent(source, (SymbolisedTextWithChildren)item, text);
      }
   }
}
