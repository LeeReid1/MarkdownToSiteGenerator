namespace MarkdownToSiteGenerator
{
   internal class SymbolisedDocument : SymbolisedTextWithChildren
   {
      public override IEnumerable<ISymbolisedText> Children => Items;


      public SymbolisedDocument(string source):base(new SymbolLocation(SimpleRange.Empty(0), new SimpleRange(0, source.Length), SimpleRange.Empty(source.Length)))
      {
      }

   }
}
