using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public class FilePath : GenericPath<string>, IPath
   {
      public bool IsDirectory => Parts[^1] == string.Empty;
      public bool IsFile => !IsDirectory;

      public FilePath(IEnumerable<string> parts) : base(parts) { }
      public FilePath(string path):base(path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
      {
      }

      public static implicit operator FilePath(string path)=> new FilePath(path);
      public static implicit operator FilePath(char path)=> new FilePath(path.ToString());
      public static FilePath operator +(FilePath left, FilePath right) => left.IsDirectory ? new FilePath(left.Parts.Take(left.Parts.Count-1).Concat(right.Parts)) :  throw new ArgumentException("The left file path is not a directory");

      /// <summary>
      /// Converts this into a legal path string
      /// </summary>
      public override string ToString() => string.Join(Path.DirectorySeparatorChar, Parts);

      public FilePath ToAbsolute() => new(ToAbsoluteString());
      public string ToAbsoluteString() => Path.GetFullPath(this.ToString());

      public FilePath ToRelative(FilePath relativeTo)
      {
         if(!relativeTo.IsDirectory)
         {
            throw new ArgumentException("relative to is a file");
         }
         var relativeToParts = relativeTo.ToAbsolute().Parts.ToArray()[..^1];
         var asAbs = this.ToAbsolute();

         if(!relativeToParts.SequenceEqual(asAbs.Parts.Take(relativeToParts.Length)))
         {
            throw new ArgumentException("Arguments do not share a common path");
         }

         return new FilePath(asAbs.Parts.Skip(relativeToParts.Length));
      }


   }
}
