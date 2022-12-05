namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents a list
   /// </summary>
   internal class List : HtmlSymbol
   {
      protected override string TagCode => isOrdered ? "ol" : "ul";
      readonly bool isOrdered;

      public List(MarkdownToSiteGenerator.List symbol)
      {
         isOrdered = symbol.IsOrdered;
      }
   }
}