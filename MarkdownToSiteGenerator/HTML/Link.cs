namespace MarkdownToSiteGenerator.HTML
{
   internal class Link : HtmlSymbol
   {
      protected override string TagCode => "a";
      public string HRef { get; set; } = "#";
      protected override string TagMetaData => $"href=\"{HRef}\"";
   }
}
