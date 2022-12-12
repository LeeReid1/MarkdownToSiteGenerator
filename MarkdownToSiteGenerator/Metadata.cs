namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates markup for a document's metadata
   /// </summary>
   internal class Metadata : SymbolisedTextWithChildren
   {
      public Metadata(SymbolLocation location) : base(location)
      {
      }

      internal (string key, string value) GetKeyAndValue(string source)
      {
         int splitAt = source.IndexOf(':', Location.ContentLocation.Start);
         return (source[Location.ContentLocation.Start..splitAt], source[(splitAt + 1)..Location.ContentLocation.End].Trim());
      }

   }
}
