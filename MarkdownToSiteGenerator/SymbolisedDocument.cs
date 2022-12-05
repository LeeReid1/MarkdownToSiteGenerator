namespace MarkdownToSiteGenerator
{
   internal class SymbolisedDocument : SymbolisedTextWithChildren
   {
      public override IEnumerable<ISymbolisedText> Children => Items;


      public SymbolisedDocument(string source):base(new SymbolLocation(new Range(0,0), new Range(0, source.Length)))
      {
      }

   }
}
