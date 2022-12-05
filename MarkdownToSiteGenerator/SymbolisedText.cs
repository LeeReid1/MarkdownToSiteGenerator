namespace MarkdownToSiteGenerator
{
   internal abstract class SymbolisedText : ISymbolisedText
   {
      public SymbolLocation Location { get; }
      public abstract IEnumerable<ISymbolisedText> Children { get; }

      protected SymbolisedText(SymbolLocation location)
      {
         this.Location = location;
      }

      public abstract IEnumerable<string> GetContentFragments(string source);
   }

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
