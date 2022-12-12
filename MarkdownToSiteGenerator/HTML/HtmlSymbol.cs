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
      public virtual string CSSClass { get; set; } = string.Empty;



      public virtual StringBuilder Write(StringBuilder sb)
      {

         sb.Append('<').Append(TagCode);
         if (TagMetaData.Length != 0)
         {
            sb.Append(' ').Append(TagMetaData);
         }
         if (CSSClass.Length != 0)
         {
            sb.Append(" class=\"").Append(CSSClass).Append('"');
         }


         CompleteWriteAndCloseTag(sb);
         return sb;
      }

      protected virtual void CompleteWriteAndCloseTag(StringBuilder sb)
      {
         sb.Append(" />");
      }
   }


}
