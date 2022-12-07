using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class FileSourceProviderTests
   {
      FilePath? dir;
      [TestInitialize]
      public void Initialize()
      {
         Cleanup();
         FilePath path = Directory.CreateTempSubdirectory("test_").FullName;

         // Append a / or \ if needed as this can return paths that looks like files
         dir = path.ToString() + (path.IsFile ? Path.DirectorySeparatorChar : string.Empty);

      }

      [TestCleanup]
      public void Cleanup()
      {
         if (dir != null)
         {
            Directory.Delete(dir.ToAbsoluteString(), true);
         }
         dir = null;
      }



      [TestMethod]
      public async Task GetConfigurationFileContent()
      {
         Assert.IsNotNull(dir);

         string content = Guid.NewGuid().ToString();
         File.WriteAllText((dir + FileSourceProvider.Loc_config_relative).ToString(), content);

         FileSourceProvider fsp = new(dir);
         string? foundContent = await fsp.GetConfigurationFileContent();
         Assert.IsNotNull(content, foundContent);
         Assert.AreEqual(content, foundContent);
      }

      [TestMethod]
      public async Task GetConfigurationFileContent_Empty()
      {
         Assert.IsNotNull(dir);

         FileSourceProvider fsp = new(dir);
         string? foundContent = await fsp.GetConfigurationFileContent();
         Assert.IsNull(foundContent);
      }
   }
}
