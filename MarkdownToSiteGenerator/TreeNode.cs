using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Represents a tree, or path thereof, with no full-path duplicates
   /// </summary>
   /// <typeparam name="T">The value held at each leaf or branch point</typeparam>
   public class UniqueTreeNode<T> : IEnumerable<GenericPath<T>> where T : IEquatable<T>, IComparable<T>, IComparable
   {
      public List<UniqueTreeNode<T>> Children { get; } = new List<UniqueTreeNode<T>>();
      public T Value { get; }

      public UniqueTreeNode(T value) { Value = value; }

      public UniqueTreeNode<T> AddValueByPath(IEnumerable<T> path)
      {
         UniqueTreeNode<T> final = this;

         var enumerator = path.GetEnumerator();

         return Sub(this, enumerator);

         static UniqueTreeNode<T> Sub(UniqueTreeNode<T> node, IEnumerator<T> enumerator)
         {
            if (!enumerator.MoveNext())
            {
               throw new ArgumentException("An item with the same key has already been added.");
            }

            foreach (var child in node.Children)
            {
               if (child.Value.Equals(enumerator.Current))
               {
                  return Sub(child, enumerator);
               }
            }

            // Does not match any child
            // Add a new leaf to the tree
            return node.AddChildPath(enumerator);
         }
      }

      private UniqueTreeNode<T> AddChildPath(IEnumerator<T> path)
      {
         UniqueTreeNode<T> next = new(path.Current);
         if (path.MoveNext())
         {
            next.AddChildPath(path);
         }
         Children.Add(next);
         return next;

      }

      public IEnumerable<GenericPath<T>> AsEnumerable()
      {
         List<T> path = new();
         return Sub(this, path);

         static IEnumerable<GenericPath<T>> Sub(UniqueTreeNode<T> node, List<T> prepend)
         {
            prepend = new List<T>(prepend)
            {
               node.Value
            };
            if (node.Children.Count == 0)
            {
               return Enumerable.Repeat(new GenericPath<T>(prepend), 1);
            }
            else
            {
               return node.Children.SelectMany(a => Sub(a, prepend));
            }
         }
      }
      public IEnumerator<GenericPath<T>> GetEnumerator() => AsEnumerable().GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => AsEnumerable().GetEnumerator();

      public override string ToString()
      {
         return $"Value {Value} Items: {Children.Count}";
      }
   }
}
