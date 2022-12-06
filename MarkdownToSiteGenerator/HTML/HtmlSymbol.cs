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


      readonly List<HtmlSymbol> children = new();
      public IReadOnlyList<HtmlSymbol> Children => children;


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

      public void Add(HtmlSymbol symbol) => Insert(children.Count, symbol);
      public virtual void Insert(int position, HtmlSymbol symbol) => children.Insert(position, symbol);
   }
}
