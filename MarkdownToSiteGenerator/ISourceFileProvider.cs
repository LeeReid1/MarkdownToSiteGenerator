using Microsoft.Extensions.Configuration;

namespace MarkdownToSiteGenerator
{
   public interface ISourceFileProvider<TPath>
   {
      /// <summary>
      /// Returns absolute locations of source files
      /// </summary>
      /// <param name="sourceFilesOnly">Limits what is returned to files that have source code, like markdown</param>
      /// <returns></returns>
      TPath[] GetFileLocations(bool sourceFilesOnly);
      ///// <summary>
      ///// Returns content of files that have source code, like markdown
      ///// </summary>
      //IAsyncEnumerable<(string source, TPath location)> GetSourceFiles();
      //Dictionary<string, string> GetUniversalMetadata();

      Task<string> GetFileContent(TPath path);

      /// <summary>
      /// Provides contents of a INI configuration file found with the documents. If none exists, it returns null
      /// </summary>
      Task<string?> GetConfigurationFileContent();
   }
}