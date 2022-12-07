namespace MarkdownToSiteGenerator
{
   public class GenericPath<T> : IEquatable<GenericPath<T>>, IComparable, IComparable<GenericPath<T>>
      where T:IEquatable<T>, IComparable
   {
      public IReadOnlyList<T> Parts { get; }

      public GenericPath(IEnumerable<T> parts)
      {
         Parts = parts.ToArray();
      }

      public override bool Equals(object? obj) => obj is GenericPath<T> gp && this.Equals(gp);
      public override int GetHashCode()
      {
         unchecked
         {
            int last = 94550977;
            foreach (var part in Parts)
            {
               last += (last * 29) ^ part.GetHashCode();
            }
            return last;
         }
      }
      public bool Equals(GenericPath<T>? other) => other != null && Parts.SequenceEqual(other.Parts);

      public int CompareTo(object? obj) => obj is GenericPath<T> gp ? CompareTo(gp) : 0;

      public int CompareTo(GenericPath<T>? other)
      {
         if(other == null) return 0;

         return string.Join('/', Parts.ToArray()).CompareTo(string.Join('/', Parts.ToArray()));
      }
   }
}
