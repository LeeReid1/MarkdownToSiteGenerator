namespace MarkdownToSiteGenerator
{
   internal class Metadata : SymbolisedTextWithChildren
   {
      public Metadata(SymbolLocation location) : base(location)
      {
      }

      internal (string key, string value) GetKeyAndValue(string source)
      {
         int splitAt = source.IndexOf(':');
         return (source[..splitAt], source[(splitAt + 1)..].Trim());
      }

   }
}
