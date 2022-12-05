namespace MarkdownToSiteGenerator.HTML
{
   internal class Paragraph : HtmlSymbol
   {
      readonly MarkdownToSiteGenerator.Paragraph symbol;

      public Paragraph(MarkdownToSiteGenerator.Paragraph symbol)
      {
         this.symbol = symbol;
      }

      protected override string TagCode => "p";
   }
}
