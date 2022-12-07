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
   internal class PathMapper : IPathMapper<FilePath, FilePath>
   {
      public static readonly string RelativeOutputPath_Images = "images" + Path.DirectorySeparatorChar;
      public static readonly string RelativeOutputPath_CSS = "css" + Path.DirectorySeparatorChar;
      public static readonly string RelativeOutputPath_JS = "js" + Path.DirectorySeparatorChar;

      /// <summary>
      /// Destination directory
      /// </summary>
      public FilePath Dir_DestinationTop { get; }

      /// <summary>
      /// Source directory
      /// </summary>
      public FilePath Dir_SourceTop { get; }

      /// <summary>
      /// Image directory
      /// </summary>
      public FilePath Dir_Images => Dir_DestinationTop + RelativeOutputPath_Images;
      /// <summary>
      /// Javascript directory
      /// </summary>
      public FilePath Dir_JS => Dir_DestinationTop + RelativeOutputPath_JS;
      /// <summary>
      /// Stylesheets directory
      /// </summary>
      public FilePath Dir_CSS => Dir_DestinationTop + RelativeOutputPath_CSS;

      public PathMapper(FilePath sourceTop, FilePath destinationTop)
      {
         if (!sourceTop.IsDirectory)
         {
            throw new ArgumentException($"{nameof(sourceTop)} must explicitly be a directory (end with {Path.DirectorySeparatorChar})");
         }
         if (!destinationTop.IsDirectory)
         {
            throw new ArgumentException($"{nameof(destinationTop)} must explicitly be a directory (end with {Path.DirectorySeparatorChar})");
         }
         this.Dir_SourceTop = sourceTop.ToAbsolute();
         this.Dir_DestinationTop = destinationTop.ToAbsolute();
      }

      public FilePath GetDestination(FilePath source)
      {
         string relativePath = source.ToRelative(Dir_SourceTop).ToString().Replace(" ","-");
         string filenameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
         string extension = Path.GetExtension(relativePath);
         string filenameWithExtension = filenameWithoutExtension + extension;
         return extension.ToLowerInvariant() switch
         {
            ".md" or ".html" or ".htm"                       => Dir_DestinationTop + (relativePath[..^extension.Length] + ".html"), // maintain folder structure
            ".jpg" or ".jpeg" or ".png" or ".bmp" or ".webp" => Dir_Images + filenameWithExtension, // move all to same folder
            ".css" or ".less"                                => Dir_CSS + filenameWithExtension, // move all to same folder
            ".js"                                            => Dir_JS + filenameWithExtension, // move all to same folder
            _ => throw new NotSupportedException(extension)
         };

      }


      public string GetURLLocation(FilePath source) => GetDestination(source).ToURLFormat_RelativeToRoot(Dir_DestinationTop);

      public FilePath ParsePathIn(string source) => new(Path.GetFullPath(source));
   }
}
