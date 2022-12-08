using MarkdownToSiteGenerator.Markdown;
using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class ParentMarkdownSymbolParserTests
   {
      [TestMethod]
      public void ParseContentWithinParent()
      {
         
         string text = @"My paragraph links to [my site](https://example.com) still the paragraph";

         Paragraph p = new Paragraph(new SymbolLocation(SimpleRange.Empty(0), new SimpleRange(0, text.Length), SimpleRange.Empty(text.Length)));

         ParentMarkdownSymbolParser.ParseContentWithinParent(text, new SimpleRange(0, text.Length), p);

         Assert.AreEqual(3, p.Children.Count());
         AssertHelp.AssertPlainContent(p.Items[0], text, "My paragraph links to ");
         AssertHelp.AssertLink(p.Items[1], text, "my site", "https://example.com");
         AssertHelp.AssertPlainContent(p.Items[2], text, " still the paragraph");

      }
   }
}
