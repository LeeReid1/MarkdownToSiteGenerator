namespace MarkdownToSiteGenerator.Markdown
{
   internal class ParagraphSymbolParser : TopLevelObjectParser
   {
      protected override string RegexStr => @"(^)(\S+?[\s\S]*?)((?:\r\n|\n|\z){2}|\z)";

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation location) => new Paragraph(location);
   }
}
