using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class FileWriter : IFileWriter<FilePath>
   {
      public void Delete(FilePath path)
      {
         string abs = path.ToAbsoluteString();
         if (File.Exists(abs))
         {
            File.Delete(abs);
         }
      }
      public bool FileExists(FilePath path) => File.Exists(path.ToString());
      public async Task Write(StringBuilder content, FilePath destination)
      {
         string dest = Write_Prepare(destination);
         using StreamWriter stream = new(dest, false, Encoding.UTF8, 128000);
         await stream.WriteAsync(content);
      }
      public async Task Write(string content, FilePath destination)
      {
         string dest = Write_Prepare(destination);
         using StreamWriter stream = new(dest, false, Encoding.UTF8, 128000);
         await stream.WriteAsync(content);
      }

      private static string Write_Prepare(FilePath destination)
      {
         string dest = destination.ToAbsoluteString();
         Directory.CreateDirectory(Path.GetDirectoryName(dest) ?? throw new NullReferenceException(nameof(destination)));
         return dest;
      }

      public async Task WriteBinary(Stream s, FilePath destination)
      {
         string dest = Write_Prepare(destination);

         using FileStream fs = System.IO.File.OpenWrite(dest);
         await s.CopyToAsync(fs);
         await s.DisposeAsync();
      }
   }
}
