using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public static class CollectionExtensions
   {
      /// <summary>
      /// Performs the action on each item provided and returns the original ienumerable
      /// </summary>
      public static IEnumerable<T> ForEach<T>(this IEnumerable<T> ienum, Action<T> act)
      {
         foreach (var item in ienum)
         {
            act(item);
            yield return item;
         }
      }


      /// <summary>
      /// Performs the act on every item in the queue. Can add or remove from to the queue while this is in progress
      /// </summary>
      public static void ForEachUntilEmpty<T>(this Queue<T> items, Action<T> act)
      {
         while (items.Count != 0)
         {
            act(items.Dequeue());
         }
      }

      public static IEnumerable<int> IndexOfAll<T>(this IEnumerable<T> items, Func<T, bool> filter)
      {
         int index = 0;
         foreach (var item in items)
         {
            if(filter(item))
            {
               yield return index;
            }
            index++;
         }
      }

      public static IEnumerable<IEnumerable<T>> Split<T>(this IList<T> list, Func<T, int, bool> splitHere)
      {
         IOrderedEnumerable<int> splitAt = (IOrderedEnumerable<int>)Enumerable.Range(0, list.Count).Where(i => splitHere(list[i],i));
         return Split(list, splitAt);
      }

      public static IEnumerable<IEnumerable<T>> Split<T>(this IList<T> items, IOrderedEnumerable<int> splitAt)
      {
         int last = 0;

         foreach (var item in splitAt)
         {
            int take = item - last;
            if (take != 0)
            {
               yield return items.Skip(last).Take(take);
            }
            last = item;
         }

         // last chunk
         if (last < items.Count)
         {
            int take = items.Count - last;
            yield return items.Skip(last).Take(take);
         }
      }


      public static IEnumerable<S> TakeWhileType<T,S>(this IEnumerable<T> items) where S:T
      {
         foreach (var item in items)
         {
            if(item is S s)
            {
               yield return s;
            }
            else
            {
               break;
            }
         }
      }
   }
}
