using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class FileWriter : IFileWriter<FilePath>
   {
      public void Delete(FilePath path) => File.Delete(path.ToAbsoluteString());

      public bool FileExists(FilePath path) => File.Exists(path.ToString());
      public void Write(StringBuilder content, FilePath destination)
      {
         string dest = destination.ToAbsoluteString();
         Directory.CreateDirectory(Path.GetDirectoryName(dest) ?? throw new NullReferenceException(nameof(destination)));

         using StreamWriter stream = new(dest, false, Encoding.UTF8, 128000);
         stream.WriteAsync(content);
      }
   }
}
