using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class FileSourceProvider : ISourceFileProvider<FilePath>
   {
      /// <summary>
      /// Location of the config file relative to the top directory
      /// </summary>
      public const string Loc_config_relative = "config.ini";
      readonly FilePath dir_top;

      static readonly IReadOnlyList<string> ImageSuffixes = new string[]
      {
         ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".webp"
      };

      public FileSourceProvider(FilePath dir_top)
      {
         if (!dir_top.IsDirectory)
         {
            throw new ArgumentException($"File path must explicitly be a directory (end with {Path.DirectorySeparatorChar})");
         }
         this.dir_top = dir_top;
      }

      public async Task<string> GetFileContent(FilePath path) => await File.ReadAllTextAsync(path.ToString());
      public Stream GetImageFileContent(FilePath imInfo) => File.OpenRead(imInfo.ToString());
      public FilePath[] GetFileLocations(FileTypes sourceFilesOnly)
      {
         List<string> extensions = new(12);
         if(sourceFilesOnly.HasFlag(FileTypes.SourceFiles))
         {
            extensions.Add(".md");
         }
         if(sourceFilesOnly.HasFlag(FileTypes.Images))
         {
            extensions.AddRange(ImageSuffixes);
         }

         return Directory.EnumerateFiles(dir_top.ToAbsoluteString(),"*", SearchOption.AllDirectories)
                  .Where(a => extensions.Contains(Path.GetExtension(a), StringComparer.InvariantCultureIgnoreCase))
                  .Select(a => new FilePath(a))
                  .ToArray();
      }
      public async Task<string?> GetConfigurationFileContent()
      {
         string path = (dir_top + Loc_config_relative).ToString();
         return File.Exists(path) ? await GetFileContent(path) : null;
      }

      public string GetImageTitle(FilePath path)
      {
         if(path.IsFile)
         {
            return path.Parts[^1];
         }
         else
         {
            throw new ArgumentException("Path does not point to an image file");
         }
      }


   }
}
