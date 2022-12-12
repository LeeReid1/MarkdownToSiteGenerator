using MarkdownToSiteGenerator.Markdown;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates markup+content denoting a heading in a source document
   /// </summary>
   internal class Heading : SymbolisedTextWithChildren
   {
      public byte Level { get; }

      public Heading(SymbolLocation location, byte level) : base(location)
      {
         Level = level;
      }
   }
}
