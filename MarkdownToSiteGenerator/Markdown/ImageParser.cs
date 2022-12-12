namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Parses markdown image tags
   /// </summary>
   internal class ImageParser : MarkdownSymbolParser
   {
      protected override string RegexStr => @"(\!\[)(.+?)(?:\])(?:\()(\S+?)(\))";

      public override SymbolisedText ToSymbolisedText(SymbolLocation sl) => new Image(sl);
   }
}
