namespace MarkdownToSiteGenerator
{
   internal abstract class SymbolisedText : ISymbolisedText, IComparable
   {
      public SymbolLocation Location { get; }
      public abstract IEnumerable<ISymbolisedText> Children { get; }
      protected SymbolisedText(SymbolLocation location)
      {
         this.Location = location;
      }

      public abstract IEnumerable<string> GetContentFragments(string source);

      int IComparable.CompareTo(object? obj) => obj is SymbolisedText ist ? Location.CompareTo(ist.Location) : 0;
   }
}
