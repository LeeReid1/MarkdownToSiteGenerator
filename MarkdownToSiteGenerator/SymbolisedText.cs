namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Base class. Denotes a section of text that represents something, such as a heading or a paragraph
   /// </summary>
   internal abstract class SymbolisedText : ISymbolisedText, IComparable
   {
      public SymbolLocation Location { get; }
      public abstract IEnumerable<ISymbolisedText> Children { get; }
      protected SymbolisedText(SymbolLocation location)
      {
         this.Location = location;
      }


      int IComparable.CompareTo(object? obj) => obj is SymbolisedText ist ? Location.CompareTo(ist.Location) : 0;
   }
}
