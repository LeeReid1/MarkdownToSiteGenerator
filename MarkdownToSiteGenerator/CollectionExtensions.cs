using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public static class CollectionExtensions
   {
      /// <summary>
      /// Performs the action on each item provided
      /// </summary>
      public static void ForEach<T>(this IEnumerable<T> ienum, Action<T> act)
      {
         foreach (var item in ienum)
         {
            act(item);
         }
      }


      /// <summary>
      /// Performs the act on every item in the queue. Can add or remove from to the queue while this is in progress
      /// </summary>
      public static void ForEachUntilEmpty<T>(this Queue<T> items, Action<T> act)
      {
         while(items.Count != 0)
         {
            act(items.Dequeue());
         }
      }
   }
}
