namespace MarkdownToSiteGenerator
{
   public interface IPathMapper<TPathIn, TPathOut> : IPathToURLMapper<TPathIn>
   {
      string GetURL_Sitemap_HTML => "/sitemap.html";
      string GetURL_Sitemap_XML => "/sitemap.xml";

      /// <summary>
      /// Gets the final location of the provided input file
      /// </summary>
      TPathOut GetDestination(TPathIn source);
      /// <summary>
      /// Gets the final location of the XML sitemap
      /// </summary>
      TPathOut GetDestination_Sitemap_XML();

      /// <summary>
      /// Gets the final location of the HTML sitemap
      /// </summary>
      TPathOut GetDestination_Sitemap_HTML();

      /// <summary>
      /// Parses a string into a PathIn type
      /// </summary>
      TPathIn ParsePathIn(string source);


   }
}