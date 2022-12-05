namespace MarkdownToSiteGenerator.HTML
{

   /// <summary>
   /// Represents a list item in an ordered or unordered list
   /// </summary>
   internal class ListItem : HtmlSymbol
   {
      protected override string TagCode => "li";


      public ListItem(MarkdownToSiteGenerator.ListItem symbol)
      {
      }
   }
}