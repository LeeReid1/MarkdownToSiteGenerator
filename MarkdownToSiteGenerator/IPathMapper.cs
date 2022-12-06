namespace MarkdownToSiteGenerator
{
   public interface IPathMapper<TPathIn, TPathOut>
   {
      /// <summary>
      /// Parses an output location to a URL location
      /// </summary>
      string GetURLLocation(TPathOut source);


      /// <summary>
      /// Gets the final location of the provided input file
      /// </summary>
      TPathOut GetDestination(TPathIn source);

      /// <summary>
      /// Parses a string into a PathIn type
      /// </summary>
      TPathIn ParsePathIn(string source);


   }
}