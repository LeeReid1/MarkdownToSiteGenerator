using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   internal static class AssertHTML
   {
      public static void AssertMetadata(HtmlDocument doc, Action<HtmlSymbol>[] childVerifications)
      {
         Assert.IsNotNull(doc);
         Assert.AreEqual(doc.Metadata.Count, childVerifications.Length);
         for (int i = 0; i < childVerifications.Length; i++)
         {
            childVerifications[i].Invoke(doc.Metadata[i]);
         }
      }
      public static void AssertDocument(HtmlDocument doc, Action<HtmlSymbol>[] childVerifications)
      {
         Assert.IsNotNull(doc);
         AssertChildren(doc, childVerifications);
      }

      public static void AssertChildren(HtmlSymbol doc, Action<HtmlSymbol>[] childVerifications)
      {
         HtmlSymbolWithChildren withKids = (HtmlSymbolWithChildren)doc;
         Assert.AreEqual(childVerifications.Length, withKids.Children.Count);
         for (int i = 0; i < childVerifications.Length; i++)
         {
            childVerifications[i].Invoke(withKids.Children[i]);
         }
      }

      public static void AssertParagraph(HtmlSymbol symb, Action<HtmlSymbol>[] childVerifications)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Paragraph));
         AssertChildren((Paragraph)symb, childVerifications);
      }

      public static void AssertParagraph(HtmlSymbol symb, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Paragraph));
         AssertPlainContent((Paragraph)symb, text);
      }

      public static void AssertPlainContent(HtmlSymbolWithChildren symb, string text)
      {
         Assert.AreEqual(1, symb.Children.Count);
         var child = symb.Children[0];
         AssertLiteralText(child, text);
      }

      public static void AssertLiteralText(HtmlSymbol child, string text)
      {
         Assert.IsInstanceOfType(child, typeof(MarkdownToSiteGenerator.HTML.LiteralText));
         Assert.AreEqual(text, ((MarkdownToSiteGenerator.HTML.LiteralText)child).ToString());
      }

      public static void AssertHeading(HtmlSymbol symb, byte level, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Heading));
         Assert.AreEqual(level, ((MarkdownToSiteGenerator.HTML.Heading)symb).Level);

         AssertPlainContent((MarkdownToSiteGenerator.HTML.Heading)symb, text);
      }
      public static void AssertMetadata(HtmlSymbol symb, string key, string value)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Metadata));
         Assert.AreEqual(key, ((MarkdownToSiteGenerator.HTML.Metadata)symb).Key);
         Assert.AreEqual(value, ((MarkdownToSiteGenerator.HTML.Metadata)symb).Value);
      }
      public static void AssertOrderedList(HtmlSymbol symb, IList<string> text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.List));
         Assert.IsTrue(((MarkdownToSiteGenerator.HTML.List)symb).IsOrdered);
         MarkdownToSiteGenerator.HTML.List l = (MarkdownToSiteGenerator.HTML.List)symb;
         Assert.AreEqual(l.Children.Count, text.Count);

         for (int i = 0; i < text.Count; i++)
         {
            Assert.IsInstanceOfType(l.Children[i], typeof(MarkdownToSiteGenerator.HTML.ListItem));
            AssertPlainContent((MarkdownToSiteGenerator.HTML.ListItem)l.Children[i], text[i]);
         }
      }
      
      public static void AssertUnorderedList(HtmlSymbol symb)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.List));
         Assert.IsFalse(((MarkdownToSiteGenerator.HTML.List)symb).IsOrdered);
      }
      
      public static void AssertListItem(HtmlSymbol symb)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.ListItem));
      }      
      public static void AssertLink(HtmlSymbol symb, string content, string href)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Link));
         Link l = (MarkdownToSiteGenerator.HTML.Link)symb;
         Assert.AreEqual(href, l.HRef);
         Assert.AreEqual(1, l.Children.Count);
         AssertPlainContent(l, content);
      }
      
      public static void AssertImage(HtmlSymbol symb, string alt, string href)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Image));
         Image l = (MarkdownToSiteGenerator.HTML.Image)symb;
         Assert.AreEqual(href, l.HRef);
         Assert.AreEqual(alt, l.AltText);
      }
      
      public static void AssertOnlyContainsLink(HtmlSymbol sym, string content, string href)
      {
         Assert.IsInstanceOfType(sym, typeof(HtmlSymbolWithChildren));
         HtmlSymbolWithChildren symb = (HtmlSymbolWithChildren)sym;
         Assert.AreEqual(1, symb.Children.Count);
         AssertLink(symb.Children[0], content, href);
      }
   }
}
