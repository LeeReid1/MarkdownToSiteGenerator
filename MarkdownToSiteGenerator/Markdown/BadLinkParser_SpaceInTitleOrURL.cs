namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Looks for markdown links containing spaces in the url/title
   /// </summary>
   internal class BadLinkOrImageParser_SpaceInTitleOrURL : BadMarkdownChecker
   {
      protected override string Message => "Link appears to contain spaces in the URL. Use the title with underscores or a valid URL";
      protected override string RegexStr => @"(\[)([^\]]+?)(?:\])(?:\()([^\)]*?[ \t]+[^\)]*?)(\))";

   }
}
