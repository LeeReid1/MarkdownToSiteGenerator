namespace MarkdownToSiteGenerator
{
   internal class LiteralText : SymbolisedText
   {
      public override IEnumerable<ISymbolisedText> Children => Enumerable.Empty<ISymbolisedText>();

      public LiteralText(SimpleRange contentLocation) : base(new SymbolLocation(SimpleRange.Empty(contentLocation.Start), contentLocation, SimpleRange.Empty(contentLocation.End)))
      {
      }

      public override IEnumerable<string> GetContentFragments(string source)
      {
         yield return Location.ExtractContent(source).ToString();
      }
   }
}
