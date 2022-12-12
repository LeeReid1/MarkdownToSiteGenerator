namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Represents a list
   /// </summary>
   internal class List : HtmlSymbolWithChildren
   {
      protected override string TagCode => IsOrdered ? "ol" : "ul";
      public bool IsOrdered { get; }

      public List(bool isOrdered)
      {
         this.IsOrdered = isOrdered;
      }
   }
}