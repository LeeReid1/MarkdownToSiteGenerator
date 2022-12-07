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
      

      public FileSourceProvider(FilePath dir_top)
      {
         if (!dir_top.IsDirectory)
         {
            throw new ArgumentException($"File path must explicitly be a directory (end with {Path.DirectorySeparatorChar})");
         }
         this.dir_top = dir_top;
      }

      public async Task<string> GetFileContent(FilePath path) => await File.ReadAllTextAsync(path.ToString());

      public FilePath[] GetFileLocations(bool sourceFilesOnly) => Directory.GetFiles(dir_top.ToAbsoluteString(), sourceFilesOnly ? "*" : "*.md", SearchOption.AllDirectories)
                                                                           .Select(a => new FilePath(a))
                                                                           .Where(a => !(a.Parts[^1] == Loc_config_relative)) // don't return the settings file as content
                                                                           .ToArray();

      public async Task<string?> GetConfigurationFileContent()
      {
         string path = (dir_top + Loc_config_relative).ToString();
         return File.Exists(path) ? await GetFileContent(path) : null;
      }
   }
}
