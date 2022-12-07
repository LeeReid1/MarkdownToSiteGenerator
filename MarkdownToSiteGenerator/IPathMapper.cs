namespace MarkdownToSiteGenerator
{
   public interface IPathMapper<TPathIn, TPathOut>
   {
      /// <summary>
      /// Parses an output location to a URL location
      /// </summary>
      string GetURLLocation(TPathIn source);


      /// <summary>
      /// Gets the final location of the provided input file
      /// </summary>
      TPathOut GetDestination(TPathIn source);
      /// <summary>
      /// Gets the final locatio of the XML sitemap
      /// </summary>
      TPathOut GetDestination_Sitemap_XML();

      /// <summary>
      /// Gets the final locatio of the HTML sitemap
      /// </summary>
      TPathOut GetDestination_Sitemap_HTML();

      /// <summary>
      /// Parses a string into a PathIn type
      /// </summary>
      TPathIn ParsePathIn(string source);


   }
}