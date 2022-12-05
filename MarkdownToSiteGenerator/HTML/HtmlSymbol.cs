using System.Text;

namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents a symbol in html
   /// </summary>
   public abstract class HtmlSymbol
   {
      protected abstract string TagCode { get; }
      protected virtual string TagMetaData => string.Empty;

      public List<HtmlSymbol> Children { get; } = new List<HtmlSymbol>();


      public virtual StringBuilder Write(StringBuilder sb)
      {

         sb.Append('<').Append(TagCode);
         if (TagMetaData.Length != 0)
         {
            sb.Append(TagMetaData);
         }
         sb.Append('>');

         WriteContent(sb);

         sb.Append("</").Append(TagCode).Append('>');

         return sb;
      }

      protected virtual void WriteContent(StringBuilder sb)
      {
         foreach (var item in Children)
         {
            item.Write(sb);
         }
      }
   }
}
