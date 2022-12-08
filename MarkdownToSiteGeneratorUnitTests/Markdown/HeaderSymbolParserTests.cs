using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class HeaderSymbolParserTests
   {
      [TestInitialize]
      public void Initialize()
      {
      }

      KeyValuePair<string, bool>[] GetLineMatches(int noHashes)
      {
         string hashes = string.Concat(Enumerable.Repeat("#", noHashes));
         string toFewHashes = string.Concat(Enumerable.Repeat("#", noHashes - 1));
         return new KeyValuePair<string, bool>[]
         {
            new KeyValuePair<string,bool>($"{hashes} ", true),
            new KeyValuePair<string,bool>($"{hashes} Hi", true),
            new KeyValuePair<string,bool>($"{hashes}Hi", false),
            new KeyValuePair<string,bool>($"{hashes}9Hi", false),
            new KeyValuePair<string,bool>($"{hashes} 9Hi", true),
            new KeyValuePair<string,bool>($"{hashes} $123", true),
            new KeyValuePair<string,bool>($"{hashes}# ", false),
            new KeyValuePair<string,bool>($"{hashes}## ", false),
            new KeyValuePair<string,bool>($"{hashes}## hi", false),
            new KeyValuePair<string,bool>($"{toFewHashes}", false),
            new KeyValuePair<string,bool>($"{toFewHashes} Hi", false),
            new KeyValuePair<string,bool>($"{toFewHashes}Hi", false),
            new KeyValuePair<string,bool>($" ", false),
            new KeyValuePair<string,bool>($"cat", false),
            new KeyValuePair<string,bool>($"cat {hashes}", false),
            new KeyValuePair<string,bool>($"cat {hashes} hi", false),
            new KeyValuePair<string,bool>($"cat {hashes}# hi", false),
            new KeyValuePair<string,bool>($" cat", false),
            new KeyValuePair<string,bool>($" 3at", false)
         };
      }


      [TestMethod]
      [DataRow((byte)1)]
      [DataRow((byte)2)]
      [DataRow((byte)3)]
      public void IsMatch(byte level)
      {
         var h = new HeaderSymbolParser(level);
         foreach (var item in GetLineMatches(level))
         {
            Assert.AreEqual(item.Value, h.MatchesLine(item.Key));
         }
      }

      [TestMethod]
      [DataRow((byte)1)]
      [DataRow((byte)2)]
      public void GetMatches_Empty(byte level)
      {
         var h = new HeaderSymbolParser(level);
         Assert.AreEqual(0, h.GetMatches("").Count());
      }


      [TestMethod]
      [DataRow((byte)1)]
      [DataRow((byte)2)]
      public void GetMatches_NoneToFind(byte level)
      {
         var h = new HeaderSymbolParser(level);

         string text =
@"### Level 3

This is my new site
* Test one
* Test two

".ReplaceLineEndings();

         Assert.AreEqual(0, h.GetMatches(text).Count());
      }

      [TestMethod]
      [DataRow((byte)1, "Level 1")]
      [DataRow((byte)2, "Level Two")]
      public void GetMatches_OneToFind(byte level, string contentExpected)
      {
         var h = new HeaderSymbolParser(level);

         string text =
@"# Level 1
## Level Two
### Level 3

This is my new site
* Test one
* Test two

".ReplaceLineEndings();

         var round = h.GetMatches(text).ToList();
         Assert.AreEqual(1, round.Count);
         Check(text, round, 0, string.Concat(Enumerable.Repeat('#', level)) + " ", contentExpected, Environment.NewLine);
      }


      [TestMethod]
      [DataRow((byte)1, "Level 1")]
      [DataRow((byte)2, "Level Two:")]
      public void GetMatches_OneToFind_AmongstParagraphs(byte level, string contentExpected)
      {
         var h = new HeaderSymbolParser(level);

         string text =
@"Some top paragraph text.

# Level 1

Some more paragraph text

## Level Two:

Final paragraph text

".ReplaceLineEndings();
         
         var round = h.GetMatches(text).ToList();
         Assert.AreEqual(1, round.Count);
         Check(text, round, 0, string.Concat(Enumerable.Repeat('#', level)) + " ", contentExpected, Environment.NewLine);
      }


      [TestMethod]
      public void GetMatches_ThreeToFind()
      {
         var h = new HeaderSymbolParser(2);

         string text =
@"# Level 1
## Level Two!
### Level 3

## Subheading?

# No

This is my new site
* ## Not subheading Test one
* Test two

## Final one".ReplaceLineEndings();

         var round = h.GetMatches(text).ToList();
         Assert.AreEqual(3, round.Count);

         Check(text, round, 0, "## ", "Level Two!", Environment.NewLine);
         Check(text, round, 1, "## ", "Subheading?", Environment.NewLine);
         Check(text, round, 2, "## ", "Final one", string.Empty);
      }
      static void Check(string text, List<SymbolLocation> round, int index, string expectedMarkup, string expectedContent, string expectedTail)
      {
         SymbolLocation found = round[index];
         Assert.AreEqual(expectedContent, found.ExtractContent(text).ToString());
         Assert.AreEqual(expectedMarkup, found.ExtractMarkupHead(text).ToString());
         Assert.AreEqual(expectedTail, found.ExtractMarkupTail(text).ToString());

         Assert.AreEqual(found.MarkupLocation_Head.End, found.ContentLocation.Start);
         Assert.AreEqual(found.ContentLocation.End, found.MarkupLocation_Tail.Start);
      }
   }
}