using System.Text;

namespace MarkdownToSiteGenerator.HTML
{
   public abstract class HtmlSymbolWithChildren : HtmlSymbol
   {
      readonly List<HtmlSymbol> children = new();
      public IReadOnlyList<HtmlSymbol> Children => children;

      protected override void CompleteWriteAndCloseTag(StringBuilder sb)
      {
         sb.Append('>');

         WriteContent(sb);

         sb.Append("</").Append(TagCode).Append('>');

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
