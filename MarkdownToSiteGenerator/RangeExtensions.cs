using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public static class RangeExtensions
   {
      public static Range EmptyRange(Index index) => new(index, index);

      public static void AssertForwardDirAndStartBeforeEnd(this Range r)
      {
         if(r.Start.IsFromEnd || r.End.IsFromEnd)
         {
            throw new Exception("Range either is not or to the start index");
         }
         if (r.Start.Value > r.End.Value)
         {
            throw new Exception("Range ends before beginning");
         }
      }
   }
}
