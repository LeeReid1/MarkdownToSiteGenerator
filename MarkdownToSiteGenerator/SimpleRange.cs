using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Describes a range of integers
   /// </summary>
   public readonly struct SimpleRange
   {
      public int Start { get; }
      public int End { get; }
      public int Length => End - Start;
      public SimpleRange(int start, int end)
      {
         if(end< start)
         {
            throw new ArgumentException($"{nameof(end)} must be smaller than {start}");
         }
         Start = start;
         End = end;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static SimpleRange Empty(int start) => new SimpleRange(start, start);
   }
}
