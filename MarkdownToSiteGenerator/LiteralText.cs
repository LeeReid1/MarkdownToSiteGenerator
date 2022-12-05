namespace MarkdownToSiteGenerator
{
   internal class LiteralText : SymbolisedText
   {
      public override IEnumerable<ISymbolisedText> Children => Enumerable.Empty<ISymbolisedText>();

      public LiteralText(Range contentLocation) : base(new SymbolLocation(new Range(contentLocation.Start, contentLocation.Start), contentLocation))
      {
      }

      public override IEnumerable<string> GetContentFragments(string source)
      {
         yield return Location.ExtractContent(source).ToString();
      }
   }
}
