namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates markup denoting an image, and its alt text, in a source document
   /// </summary>
   internal class Image : SymbolisedText
   {
      public override IEnumerable<ISymbolisedText> Children => Enumerable.Empty<ISymbolisedText>();
      public Image(SymbolLocation location) : base(location)
      {
         if(location.ContentLocation2 == null)
         {
            throw new ArgumentNullException(nameof(location.ContentLocation2), $"A second content location must be provided in the provided {nameof(SymbolLocation)}");
         }
      }


      public ReadOnlySpan<char> GetAltText(string source) => source.AsSpan(Location.ContentLocation);

      public ReadOnlySpan<char> GetHref(string source) => source.AsSpan(Location.ContentLocation2!.Value);
   }
}
