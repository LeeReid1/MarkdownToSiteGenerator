using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public static class StringExtensionMethods
   {
      /// <summary>
      /// Returns indices of all instances of one string within another, without overlaps in anything found
      /// </summary>
      /// <exception cref="ArgumentException"></exception>
      /// <remarks>From https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp</remarks>
      public static IEnumerable<int> IndexOfAll(this string searchIn, string searchFor)
      {
         if (string.IsNullOrEmpty(searchFor))
            throw new ArgumentException($"{nameof(searchFor)} may not be null or empty", nameof(searchFor));
         for (int index = 0; ; index += searchFor.Length)
         {
            index = searchIn.IndexOf(searchFor, index);
            if (index == -1)
               break;
            yield return index;
         }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ReadOnlySpan<char> AsSpan(this string str, SimpleRange r) => str.AsSpan(r.Start,r.Length);
   }
}
