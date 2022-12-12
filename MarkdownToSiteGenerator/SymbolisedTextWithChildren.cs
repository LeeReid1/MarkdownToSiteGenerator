namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// A <see cref="SymbolisedTextWithChildren"/> that can have child objects. 
   /// </summary>
   internal abstract class SymbolisedTextWithChildren : SymbolisedText
   {
      public List<ISymbolisedText> Items { get; } = new List<ISymbolisedText>();
      public override IEnumerable<ISymbolisedText> Children => Items;
      protected SymbolisedTextWithChildren(SymbolLocation location) : base(location)
      {
      }
   }
}
