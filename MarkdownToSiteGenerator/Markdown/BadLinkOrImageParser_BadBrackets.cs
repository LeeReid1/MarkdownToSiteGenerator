namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Looks for markdown links containing spaces in the url/title
   /// </summary>
   internal class BadLinkOrImageParser_BadBrackets : BadMarkdownChecker
   {
      protected override string Message => "Link appears to have round and square brackets reversed.";
      protected override string RegexStr => @"(\()([^\]]+?)(?:\))(?:\[)([^\)]*?[ \t]?[^\)]*?)(\])";

   }
}
