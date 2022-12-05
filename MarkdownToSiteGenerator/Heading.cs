using MarkdownToSiteGenerator.Markdown;

namespace MarkdownToSiteGenerator
{
   internal class Heading : SymbolisedTextWithChildren
   {
      public byte Level { get; }

      public Heading(SymbolLocation location, byte level) : base(location)
      {
         Level = level;
      }
   }
}
