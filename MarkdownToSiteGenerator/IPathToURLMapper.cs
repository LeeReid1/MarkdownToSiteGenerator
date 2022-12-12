namespace MarkdownToSiteGenerator
{
   public interface IPathToURLMapper<TPathIn>
   {
      /// <summary>
      /// Parses an input location to a URL location
      /// </summary>
      string GetURLLocation(TPathIn source);

   }
}