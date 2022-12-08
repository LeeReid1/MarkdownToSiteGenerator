namespace MarkdownToSiteGenerator.Markdown
{
   internal class LinkParser : ParentMarkdownSymbolParser
   {
      protected override string RegexStr { get; }

      public LinkParser()
      {
         RegexStr = @"(\[)(.+?)(?:\])(?:\()(\S+?)(\))";
      }

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation sl)
      {
         return new Link(sl);
      }
   }
}
