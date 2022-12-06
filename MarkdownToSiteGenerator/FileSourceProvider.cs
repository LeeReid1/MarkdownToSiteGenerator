using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class FileSourceProvider : ISourceFileProvider<FilePath>
   {

      readonly FilePath dir_top;

      public FileSourceProvider(FilePath dir_top)
      {
         if(!dir_top.IsDirectory)
         {
            throw new ArgumentException($"File path must explicitly be a directory (end with {Path.DirectorySeparatorChar})");
         }
         this.dir_top = dir_top;
      }

      public async Task<string> GetFileContent(FilePath path) => await File.ReadAllTextAsync(path.ToString());

      public FilePath[] GetFileLocations(bool sourceFilesOnly) => Directory.GetFiles(dir_top.ToAbsoluteString(), sourceFilesOnly ? "*" : "*.md", SearchOption.AllDirectories).Select(a=>new FilePath(a)).ToArray();


   }
}
