namespace MarkdownToSiteGenerator
{
   internal abstract class SymbolisedTextWithChildren : SymbolisedText
   {
      public List<ISymbolisedText> Items { get; } = new List<ISymbolisedText>();
      public override IEnumerable<ISymbolisedText> Children => Items;
      protected SymbolisedTextWithChildren(SymbolLocation location) : base(location)
      {
      }

      
      public override IEnumerable<string> GetContentFragments(string source) => Children.SelectMany(a=>a.GetContentFragments(source));

   }
}
