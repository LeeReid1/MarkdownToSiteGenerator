namespace MarkdownToSiteGenerator.HTML
{

   /// <summary>
   /// Represents a list item in an ordered or unordered list
   /// </summary>
   internal class ListItem : HtmlSymbolWithChildren
   {
      protected override string TagCode => "li";
   }
}