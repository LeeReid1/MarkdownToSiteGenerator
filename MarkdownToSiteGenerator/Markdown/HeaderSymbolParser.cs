using System.Reflection.Emit;

namespace MarkdownToSiteGenerator.Markdown
{
   internal class HeaderSymbolParser : TopLevelObjectParser
   {
      readonly byte level;
      protected override string RegexStr { get; }

      public HeaderSymbolParser(byte level)
      {
         this.level = level;
         RegexStr = @"(^[\s-[\r\n]]*#{" + level + @"}[\s-[\r\n]]+)([^(?:\n|\r)]*)(\r\n|\n|$)";
      }

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation location)
      {
         return new Heading(location, level);
      }
   }
}
