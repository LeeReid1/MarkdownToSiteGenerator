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
   public readonly struct SimpleRange : IEquatable<SimpleRange>
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
      public static SimpleRange Empty(int start) => new(start, start);

      /// <summary>
      /// Splits into sub-ranges at and excluding the ranges provided
      /// </summary>
      public IEnumerable<SimpleRange> SplitExclude(IEnumerable<SimpleRange> splitAt)
      {
         int thisStart = Start;//req by compiler
         int thisEnd = End; 
         List<SimpleRange> ranges = splitAt.OrderBy(x => x.Start)
                                           .ThenBy(a => a.End)
                                           .SkipWhile(a => a.End < thisStart)
                                           .TakeWhile(a => a.Start <= thisEnd)
                                           .ToList();

         List<SimpleRange> gaps = new();

         int pos = thisStart;
         int excludeTill = pos;
         for (int i = 0; i < ranges.Count; i++)
         {
            var cur = ranges[i];
            if(pos < cur.Start)
            {
               // We are in a gap
               yield return new SimpleRange(pos, cur.Start);
            }
            // Move to the end of that range
            excludeTill = Math.Max(excludeTill, cur.End);
            pos = excludeTill;
         }

         if(pos < thisEnd)
         {
            yield return new SimpleRange(pos, thisEnd);
         }
      }

      public bool Equals(SimpleRange other)=> this.Start == other.Start && this.End == other.End;

      public override bool Equals(object? obj) => obj is SimpleRange sr && Equals(sr);

      public override int GetHashCode()
      {
         unchecked
         {
            return (94550977 + Start * 29) ^ End;
         }
      }

      public static bool operator ==(SimpleRange left, SimpleRange right) => left.Equals(right);

      public static bool operator !=(SimpleRange left, SimpleRange right) => !(left == right);
   }
}
