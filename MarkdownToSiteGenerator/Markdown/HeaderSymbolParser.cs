namespace MarkdownToSiteGenerator.Markdown
{
   internal class HeaderSymbolParser : ParentMarkdownSymbolParser
   {
      readonly byte level;
      protected override string RegexStr { get; }

      public HeaderSymbolParser(byte level)
      {
         this.level = level;
         RegexStr = @"(^[\s-[\r\n]]*#{" + level + @"}[\s-[\r\n]]+)([\S\s-[\r\n]]*)(\r\n|\n|$)";
      }

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation location)
      {
         return new Heading(location, level);
      }
   }
}
