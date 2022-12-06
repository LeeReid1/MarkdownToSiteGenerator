namespace MarkdownToSiteGenerator
{
   public class GenericPath<T> : IEquatable<GenericPath<T>>
      where T:IEquatable<T>
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
   }
}
