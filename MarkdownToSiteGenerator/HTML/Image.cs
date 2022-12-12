namespace MarkdownToSiteGenerator.HTML
{
   internal class Image : HtmlSymbol
   {
      protected override string TagCode => "img";
      public string HRef { get; }
      public string AltText { get; }
      protected override string TagMetaData => $"src=\"{HRef}\" alt=\"{AltText}\"";


      public Image(string href, string altText)
      {
         HRef= href;
         AltText = altText;
         this.CSSClass = "img-fluid";
      }
   }
}
