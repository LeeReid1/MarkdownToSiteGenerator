using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;
using System.Collections.ObjectModel;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class BadLinkOrImageParser_BadBrackets_Tests
   {
      [TestInitialize]
      public void Initialize()
      {
      }

      readonly IReadOnlyDictionary<string, bool> examples = new Dictionary<string,bool>()
         {
      {"", false},
      {"[some](link.com)", false},
      {"[some](#)", false},
      {"hi there [some](link.com)", false},
      {"![this is an image not a link](#)", false},
      {"hi there ![image](#)", false},
      {"hi there [some](link.com) bye", false},
      {"(some)[link .com]", true},
      {"(some)[link.com]", true},
      {"(some)[# ]", true},
      {"hi there (some)[link.c om]", true},
      {"hi there (some)[link.com]", true},
      {"!(this is an image not a link)[#]", true},
      {"!(this is an image not a link)[ #]", true},
      {"hi there !(image)[ #]", true},
      {"hi there (some)[link .com] bye", true},
      {"hi there (some)[page link] bye", true},
         };

      [TestMethod]
      public void MatchesLine()
      {
         var h = new  BadLinkOrImageParser_BadBrackets();
         foreach (var item in examples)
         {
            Assert.AreEqual(item.Value, h.MatchesLine(item.Key));
         }
      }

      [TestMethod]
      public void GetMatches_Empty()
      {
         var h = new BadLinkOrImageParser_BadBrackets();
         Assert.AreEqual(0, h.GetMatches("").Count());
      }


      [TestMethod]
      public void GetMatches_OneOrNoneToFind()
      {
         var h = new BadLinkOrImageParser_BadBrackets();
         foreach (var item in examples)
         {
            Assert.AreEqual(item.Value, h.GetMatches(item.Key).Count() == 1);
         }
      }

      [TestMethod]
      public void GetMatches_ThreeToFind()
      {
         var h = new BadLinkOrImageParser_BadBrackets();

         string text = @"this block of text (contains)[example. com] three links that (should totally)[https://example2. net] be (found!)[ # ]";

         var round = h.GetMatches(text).ToList();
         Assert.AreEqual(3, round.Count);

         Check(text, round, 0, "contains", "example. com");
         Check(text, round, 1, "should totally", "https://example2. net");
         Check(text, round, 2, "found!", " # ");
      }
      static void Check(string text, List<SymbolLocation> round, int index, string expectedContent, string expectedHref)
      {
         SymbolLocation found = round[index];
         Assert.AreEqual(expectedContent, found.ExtractContent(text).ToString());
         Assert.IsTrue(found.TryExtractContent2(text, out var href));
         Assert.AreEqual(expectedHref, href.ToString());

         int indexOfStart = text.IndexOf("(" + expectedContent + ")[");
         Assert.AreEqual(found.MarkupLocation_Head.Start, indexOfStart);

         int indexOfEnd = text.IndexOf(expectedHref) + expectedHref.Length + 1;//+1 for bracket
         Assert.AreEqual(found.MarkupLocation_Tail.End, indexOfEnd);
      }
   }
}