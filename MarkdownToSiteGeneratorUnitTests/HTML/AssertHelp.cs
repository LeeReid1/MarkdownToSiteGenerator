﻿using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   internal static class AssertHelp
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
         Assert.AreEqual(childVerifications.Length, doc.Children.Count);
         for (int i = 0; i < childVerifications.Length; i++)
         {
            childVerifications[i].Invoke(doc.Children[i]);
         }
      }

      public static void AssertParagraph(HtmlSymbol symb, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Paragraph));
         AssertPlainContent(symb, text);
      }

      public static void AssertPlainContent(HtmlSymbol symb, string text)
      {
         Assert.AreEqual(1, symb.Children.Count);
         Assert.IsInstanceOfType(symb.Children[0], typeof(MarkdownToSiteGenerator.HTML.LiteralText));
         Assert.AreEqual(text, ((MarkdownToSiteGenerator.HTML.LiteralText)symb.Children[0]).ToString());
      }

      public static void AssertHeading(HtmlSymbol symb, byte level, string text)
      {
         Assert.IsInstanceOfType(symb, typeof(MarkdownToSiteGenerator.HTML.Heading));
         Assert.AreEqual(level, ((MarkdownToSiteGenerator.HTML.Heading)symb).Level);

         AssertPlainContent(symb, text);
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
         Assert.AreEqual(symb.Children.Count, text.Count);

         for (int i = 0; i < text.Count; i++)
         {
            Assert.IsInstanceOfType(symb.Children[i], typeof(MarkdownToSiteGenerator.HTML.ListItem));
            AssertPlainContent(symb.Children[i], text[i]);
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
      
      public static void AssertOnlyContainsLink(HtmlSymbol symb, string content, string href)
      {
         Assert.AreEqual(1, symb.Children.Count);
         AssertLink(symb.Children[0], content, href);
      }
   }
}
