namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// A source document whose text content have been parsed into symbols such as paragraph, heading, image
   /// </summary>
   internal class SymbolisedDocument : SymbolisedTextWithChildren
   {
      public override IEnumerable<ISymbolisedText> Children => Items;
      public string Source { get; }
      public IEnumerable<(string key, string value)> Metadata => Children.OfType<Metadata>().Select(a => a.GetKeyAndValue(Source));

      public SymbolisedDocument(string source):base(new SymbolLocation(SimpleRange.Empty(0), new SimpleRange(0, source.Length), SimpleRange.Empty(source.Length)))
      {
         Source = source;
      }

      /// <summary>
      /// Returns the title in the metadata. If not found, returns the first encountered H1 text. If not found, returns null
      /// </summary>
      /// <returns></returns>
      public string? TryGetTitle() => TryGetMetadata("title") ?? TryGetH1Text();
      
      public string? TryGetMetadata(string key)=> Metadata.FirstOrNull(a=>a.key.Equals(key, StringComparison.OrdinalIgnoreCase))?.value;
      /// <summary>
      /// Returns the first encountered H1 text, if any
      /// </summary>
      /// <returns></returns>
      public string? TryGetH1Text()
      {
         IEnumerable<string>? substrings = Children.OfType<Heading>()
                                                   .FirstOrDefault(a => a.Level == 1)?
                                                   .Children.OfType<LiteralText>()
                                                   .Select(a => a.Location.ExtractContent(Source).ToString());
         return substrings == null ? null : string.Concat(substrings);
      }
   }
}
