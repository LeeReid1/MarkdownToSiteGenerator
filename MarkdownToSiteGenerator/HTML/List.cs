namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents a list
   /// </summary>
   internal class List : HtmlSymbol
   {
      protected override string TagCode => IsOrdered ? "ol" : "ul";
      public bool IsOrdered { get; }

      public List(bool isOrdered)
      {
         this.IsOrdered = isOrdered;
      }
   }
}