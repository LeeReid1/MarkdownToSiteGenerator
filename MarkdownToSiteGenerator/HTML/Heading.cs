namespace MarkdownToSiteGenerator.HTML
{

   /// <summary>
   /// a list item in an unodered list
   /// </summary>
   internal class Heading : HtmlSymbolWithChildren
   {
      readonly MarkdownToSiteGenerator.Heading symbol;
      protected override string TagCode => $"h{symbol.Level}";
      public byte Level => symbol.Level;

      public Heading(MarkdownToSiteGenerator.Heading symbol)
      {
         this.symbol = symbol;
      }

   }
}
