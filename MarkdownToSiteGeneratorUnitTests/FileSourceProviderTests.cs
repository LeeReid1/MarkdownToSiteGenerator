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
      FileSourceProvider? fsp;

      [TestInitialize]
      public void Initialize()
      {
         Cleanup();
         FilePath path = Directory.CreateTempSubdirectory("test_").FullName;

         // Append a / or \ if needed as this can return paths that looks like files
         dir = path.ToString() + (path.IsFile ? Path.DirectorySeparatorChar : string.Empty);
         fsp = new FileSourceProvider(dir);
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
         Assert.IsNotNull(fsp);

         string content = Guid.NewGuid().ToString();
         File.WriteAllText((dir + FileSourceProvider.Loc_config_relative).ToString(), content);

         string? foundContent = await fsp.GetConfigurationFileContent();
         Assert.IsNotNull(content, foundContent);
         Assert.AreEqual(content, foundContent);
      }

      [TestMethod]
      public async Task GetConfigurationFileContent_Empty()
      {
         Assert.IsNotNull(fsp);
         string? foundContent = await fsp.GetConfigurationFileContent();
         Assert.IsNull(foundContent);
      }

      [TestMethod]
      [DataRow(FileTypes.All)]
      [DataRow(FileTypes.SourceFiles)]
      [DataRow(FileTypes.Images)]
      public void GetFileLocations_Empty(FileTypes types)
      {
         Assert.IsNotNull(fsp);
         Assert.AreEqual(0, fsp.GetFileLocations(types).Length);
      }

      [TestMethod]
      [DataRow(FileTypes.All, 0, 9)]
      [DataRow(FileTypes.SourceFiles, 7, 2)]
      [DataRow(FileTypes.Images, 0, 7)]
      [DataRow(FileTypes.None, 0, 0)]
      public void GetFileLocations(FileTypes ft, int expectedFrom, int count)
      {
         Assert.IsNotNull(dir);
         Assert.IsNotNull(fsp);
         FilePath[] locs = new []
         {
            dir + "my-image-0.jpg",
            dir + "my-image-1.jpeg",
            dir + "my-image-2.jpeg",
            dir + "my-image-3.png",
            dir + "my-image-4.webp",
            dir + "my-image-5.bmp",
            dir + "my-image-6.gif",
            dir + "bob.md",
            dir + "bob2.md",
            dir + "config.ini", // shouldn't be found
         };

         try
         {
            foreach (var item in locs)
            {
               File.WriteAllBytes(item.ToAbsoluteString(), Array.Empty<byte>());
            }

            FilePath[] found = fsp.GetFileLocations(ft);
            CollectionAssert.AreEquivalent(locs.AsSpan(expectedFrom,count).ToArray(), found);
         }
         finally
         {
            foreach (var item in locs)
            {
               File.Delete(item.ToAbsoluteString());
            }
         }

      }


      [TestMethod]
      public void GetImageTitles()
      {
         Assert.IsNotNull(dir);
         Assert.IsNotNull(fsp);

         FilePath[] locs = new []
         {
            dir + "my-image-0.jpg",
            dir + "my-image-1.jpeg",
            dir + "my-image-2.jpeg",
            dir + "my-image-3.png",
            dir + "my-image-4.webp",
            dir + "my-image-5.bmp",
            dir + "my-image-6.gif",
            dir + "bob.md",
            dir + "bob2.md",
            dir + "config.ini", // shouldn't be found
         };

         try
         {
            foreach (var item in locs)
            {
               File.WriteAllBytes(item.ToAbsoluteString(), Array.Empty<byte>());
            }

            (FilePath location, string title)[] found = ((ISourceFileProvider<FilePath>)fsp).GetImageTitles().OrderBy(a=>a.location).ToArray();

            Assert.AreEqual(7, found.Length);
            for (int i = 0; i < found.Length; i++)
            {
               var (location, title) = found[i];
               Assert.AreEqual(locs[i], location);
               Assert.AreEqual($"my-image-{i}{Path.GetExtension(locs[i].Parts[^1])}", title);
            }

         }
         finally
         {
            foreach (var item in locs)
            {
               File.Delete(item.ToAbsoluteString());
            }
         }

      }

      [TestMethod]
      [DataRow("my-image-1.jpeg")]
      [DataRow("my-image-2.jpeg")]
      [DataRow("my-image-3.png")]
      [DataRow("my-image-4.webp")]
      [DataRow("my-image-5.bmp")]
      [DataRow("my-image-6.gif")]
      [DataRow("/some/special/path/my-image-6.gif")]
      public void GetImageTitle(string filename)
      {
         // Image titles are just the filename
         Assert.IsNotNull(dir);
         Assert.IsNotNull(fsp);

         Assert.IsFalse(string.IsNullOrEmpty(dir.ToAbsoluteString()), "sanity check");

         FilePath fp = dir + filename;

         string title = fsp.GetImageTitle(fp);
         Assert.AreEqual(filename.Split('/')[^1], title);

      }
   }
}
