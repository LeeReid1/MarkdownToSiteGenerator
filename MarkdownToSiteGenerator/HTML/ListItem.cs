using System.Text;

namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents a list item in an ordered or unordered list
   /// </summary>
   internal class ListItem : HtmlSymbol
   {
      protected override string TagCode => "li";


      public ListItem(Markdown.ListItem symbol)
      {
      }
      protected override void WriteContent(StringBuilder sb)
      {

         foreach (var item in Children)
         {
            item.Write(sb);
         }
         base.WriteContent(sb);
      }
   }
}