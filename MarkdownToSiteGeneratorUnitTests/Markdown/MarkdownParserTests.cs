using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class MarkdownParserTests
   {

      [TestMethod]
      public void ParseList()
      {
         string parseMe =
@"1. list item 1
1. second item
1. third
1. final item in the list";

         MarkdownParser mp = new MarkdownParser();
         MarkdownToSiteGenerator.SymbolisedDocument doc = mp.Parse(parseMe);

         Assert.IsNotNull(doc);
         Assert.AreEqual(1, doc.Items.Count);
         Assert.IsTrue(doc.Items[0] is MarkdownToSiteGenerator.List);
         MarkdownToSiteGenerator.List l = (MarkdownToSiteGenerator.List)doc.Items[0];
         Assert.AreEqual(4, l.Items.Count);
         CheckListItemContent((ListItem)l.Items[0], "list item 1", parseMe);
         CheckListItemContent((ListItem)l.Items[1], "second item", parseMe);
         CheckListItemContent((ListItem)l.Items[2], "third", parseMe);
         CheckListItemContent((ListItem)l.Items[3], "final item in the list", parseMe);
      }


      [TestMethod]
      public void ParseParagraphs()
      {
         string parseMe =
@"This is a paragraph. It's short.

Oh, another one here! Again, not so long";

         MarkdownParser mp = new MarkdownParser();
         MarkdownToSiteGenerator.SymbolisedDocument doc = mp.Parse(parseMe);

         Assert.IsNotNull(doc);
         Assert.AreEqual(2, doc.Items.Count);
         CheckParagraphContent(doc.Items[0], "This is a paragraph. It's short.", parseMe);
         CheckParagraphContent(doc.Items[1], "Oh, another one here! Again, not so long", parseMe);
      }
      
      [TestMethod]
      public void ParseHeadings()
      {
         string parseMe =
@"# hello!
## Headline two
### three
#### fourth
##### penultimate
###### final";

         MarkdownParser mp = new MarkdownParser();
         MarkdownToSiteGenerator.SymbolisedDocument doc = mp.Parse(parseMe);

         Assert.IsNotNull(doc);
         Assert.AreEqual(6, doc.Items.Count);
         CheckHeadingItemContent(doc.Items[0], "hello!", parseMe);
         CheckHeadingItemContent(doc.Items[1], "Headline two", parseMe);
         CheckHeadingItemContent(doc.Items[2], "three", parseMe);
         CheckHeadingItemContent(doc.Items[3], "fourth", parseMe);
         CheckHeadingItemContent(doc.Items[4], "penultimate", parseMe);
         CheckHeadingItemContent(doc.Items[5], "final", parseMe);
      }


      /// <summary>
      /// Checks a list item only containing plain text
      /// </summary>
      private static void CheckParagraphContent(ISymbolisedText item, string expected, string source)
      {
         Assert.IsInstanceOfType(item, typeof(Paragraph));
         CheckPlainContent((Paragraph)item, expected, source);
      }

      private static void CheckPlainContent(SymbolisedTextWithChildren s, string expected, string source)
      {
         Assert.AreEqual(1, s.Items.Count);
         Assert.IsTrue(s.Items[0] is LiteralText);
         LiteralText lt = (LiteralText)s.Items[0];

         Assert.AreEqual(expected, lt.GetContentFragments(source).Single());
      }

      /// <summary>
      /// Checks a list item only containing plain text
      /// </summary>
      private static void CheckListItemContent(ISymbolisedText item, string expected, string source)
      {
         Assert.IsInstanceOfType(item, typeof(ListItem));
         CheckPlainContent((ListItem)item, expected, source);
      }

      /// <summary>
      /// Checks a headline only containing plain text
      /// </summary>
      private static void CheckHeadingItemContent(ISymbolisedText item, string expected, string source)
      {
         Assert.IsInstanceOfType(item, typeof(Heading));
         CheckPlainContent((Heading)item, expected, source);
      }
   }
}
