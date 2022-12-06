using MarkdownToSiteGenerator;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class SiteGeneratorTests
   {
      [TestMethod]
      public void GetWillBeOverwritten()
      {
         Moq.Mock<ISourceFileProvider<FilePath>> mockSrc = new(Moq.MockBehavior.Strict);
         mockSrc.Setup(a => a.GetFileLocations(false)).Returns(new FilePath[] { "/test/test.md", "/test/test.jpg", "/test/test2.md" });

         Moq.Mock<IPathMapper<FilePath, FilePath>> mockMap = new(Moq.MockBehavior.Strict);
         Func<FilePath, FilePath> valueFunction = input => new FilePath(input.ToString().Replace("test", "test-output"));
         mockMap.Setup(a => a.GetDestination(It.IsAny<FilePath>())).Returns(valueFunction);
         

         Moq.Mock<IFileWriter<FilePath>> mockWriter = new(Moq.MockBehavior.Strict);
         mockWriter.Setup(a => a.FileExists(It.IsAny<FilePath>())).Returns(WillOverwrite);

         SiteGenerator<FilePath,FilePath> generator = new(mockSrc.Object, mockMap.Object, mockWriter.Object);

         CollectionAssert.AreEqual(new FilePath[] { "/test-output/test-output2.md" }, generator.GetWillBeOverwritten().ToArray());

         static bool WillOverwrite(FilePath input)
         {
            // Say that only the second md file will clash
            return input.Parts[^1] == "test-output2.md";
         }
      }


   }
}
