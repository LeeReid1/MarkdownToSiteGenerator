namespace MarkdownToSiteGenerator.HTML
{

   /// <summary>
   /// a list item in an unodered list
   /// </summary>
   internal class Heading : HtmlSymbol
   {
      readonly MarkdownToSiteGenerator.Heading symbol;
      protected override string TagCode => $"h{symbol.Level}";

      public Heading(MarkdownToSiteGenerator.Heading symbol)
      {
         this.symbol = symbol;
      }

   }
}
