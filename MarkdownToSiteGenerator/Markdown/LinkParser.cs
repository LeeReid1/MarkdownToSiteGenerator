namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Parses markdown links
   /// </summary>
   internal class LinkParser : ParentMarkdownSymbolParser
   {
      protected override string RegexStr => @"((?<!\!)\[)(.+?)(?:\])(?:\()(\S+?)(\))";

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation sl)
      {
         return new Link(sl);
      }
   }
}
