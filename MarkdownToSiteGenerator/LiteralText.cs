namespace MarkdownToSiteGenerator
{
   internal class LiteralText : SymbolisedText
   {
      public override IEnumerable<ISymbolisedText> Children => Enumerable.Empty<ISymbolisedText>();

      public LiteralText(SimpleRange contentLocation) : base(new SymbolLocation(SimpleRange.Empty(contentLocation.Start), contentLocation, SimpleRange.Empty(contentLocation.End)))
      {
      }


      public ReadOnlySpan<char> ExtractContent(string source) => Location.ExtractContent(source);
   }
}
