using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
   [TestClass]
   public class MetadataParserTests
   {
      readonly MetadataParser parser = new();

      [TestMethod]
      public void GetMatches_NoneToFind()
      {
         string text =
@"### Level 3

Bad: wrong place in file

This is my new site
* Test one
* Test two

".ReplaceLineEndings();

         Assert.AreEqual(0, parser.GetMatches(text).Count());
      }

      [TestMethod]
      [DataRow("")]
      [DataRow("\n")]
      [DataRow("\r\n")]
      [DataRow("\r\n\r\n")]
      public void GetMatches_OneToFind(string atStart)
      {
         string text = atStart + @"Title: Some Test Thing

# Level 1
## Level Two
### Level 3

This is my new site
* Test one
* Test two

".ReplaceLineEndings();

         var round = parser.GetMatches(text).ToList();
         Assert.AreEqual(1, round.Count);
         Check(text, round, 0, "Title: Some Test Thing", Environment.NewLine);
      }
      
      [TestMethod]
      [DataRow("")]
      [DataRow("\n")]
      [DataRow("\r\n")]
      [DataRow("\r\n\r\n")]
      public void GetMatches_TwoToFind(string atStart)
      {
         string text =  atStart + @"Title: Some Test Thing
Date: 12/08/2005

# Level 1
## Level Two
### Level 3

Nope: This shouldn't be found

This is my new site
* Test one
* Test two

".ReplaceLineEndings();

         var round = parser.GetMatches(text).ToList();
         Assert.AreEqual(2, round.Count);
         Check(text, round, 0, "Title: Some Test Thing", Environment.NewLine);
         Check(text, round, 1, "Date: 12/08/2005", Environment.NewLine);
      }   
      

      [TestMethod]
      [DataRow("")]
      [DataRow("\n")]
      [DataRow("\r\n")]
      [DataRow("\r\n\r\n")]
      public void GetMatches_EndOfFile(string atStart)
      {
         string text =
atStart + @"Title: Some Test Thing
Date: 12/08/2005".ReplaceLineEndings();

         var round = parser.GetMatches(text).ToList();
         Assert.AreEqual(2, round.Count);
         Check(text, round, 0, "Title: Some Test Thing", Environment.NewLine);
         Check(text, round, 1, "Date: 12/08/2005", string.Empty);
      }
      
      static void Check(string text, List<SymbolLocation> round, int index, string expectedContent, string expectedTail)
      {
         SymbolLocation found = round[index];
         Assert.AreEqual(expectedContent, found.ExtractContent(text).ToString());
         Assert.AreEqual(string.Empty, found.ExtractMarkupHead(text).Trim().ToString());
         Assert.AreEqual(expectedTail, found.ExtractMarkupTail(text).ToString());

         Assert.AreEqual(found.MarkupLocation_Head.End, found.ContentLocation.Start);
         Assert.AreEqual(found.ContentLocation.End, found.MarkupLocation_Tail.Start);
      }
   }
}