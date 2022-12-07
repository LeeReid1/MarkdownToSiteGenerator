using System.Text;

namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents literal text
   /// </summary>
   internal class LiteralText : HtmlSymbol
   {
      protected override string TagCode => "";

      readonly string contentSource;
      readonly SimpleRange range;

      public LiteralText(string text) : this(text, new SimpleRange(0, text.Length)) { }
      public LiteralText(string contentSource, SimpleRange range)
      {
         this.contentSource = contentSource;
         this.range = range;
      }
      protected override void WriteContent(StringBuilder sb)
      { }
      public override StringBuilder Write(StringBuilder sb)
      {
         // Copy from the string into the destination without heap allocation
         return sb.Append(contentSource.AsSpan(range.Start, range.Length));
      }

      public override string ToString() => contentSource.Substring(range.Start, range.Length);
   }
}