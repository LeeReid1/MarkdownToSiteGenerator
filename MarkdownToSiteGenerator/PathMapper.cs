using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Maps from source to publish directories
   /// </summary>
   internal class PathMapper
   {
      /// <summary>
      /// Destination directory
      /// </summary>
      public string Dir_DestinationTop { get; }

      /// <summary>
      /// Source directory
      /// </summary>
      public string Dir_SourceTop { get; }

      /// <summary>
      /// Image directory
      /// </summary>
      public string Dir_Images => Path.Join(Dir_DestinationTop, "images");
      /// <summary>
      /// Javascript directory
      /// </summary>
      public string Dir_JS => Path.Join(Dir_DestinationTop, "js");
      /// <summary>
      /// Stylesheets directory
      /// </summary>
      public string Dir_CSS => Path.Join(Dir_DestinationTop, "css");

      public PathMapper(string sourceTop, string destinationTop)
      {
         this.Dir_SourceTop = Path.GetFullPath(sourceTop);
         this.Dir_DestinationTop = Path.GetFullPath(destinationTop);
      }

      public string GetDestination(string source)
      {
         string relativePath = Path.GetFullPath(source).Substring(Dir_SourceTop.Length).Replace(" ","-");
         string filenameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
         string extension = Path.GetExtension(relativePath);
         return extension.ToLowerInvariant() switch
         {
            ".md" or ".txt" => Path.Join(Dir_DestinationTop, filenameWithoutExtension + ".html"),
            ".jpeg" or ".png" or ".bmp" or ".webp" => Path.Join(Dir_Images, filenameWithoutExtension + extension),
            ".css" or ".less" => Path.Join(Dir_CSS, Path.GetFileName(relativePath)),
            _ => throw new NotSupportedException(extension)
         };
      }

      public static bool SuffixIsMarkdown(string path)
      {
         string suffix = Path.GetExtension(path);
         return suffix == ".md" || suffix == ".txt";
      }
   }
}
