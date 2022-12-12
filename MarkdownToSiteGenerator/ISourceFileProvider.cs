using Microsoft.Extensions.Configuration;

namespace MarkdownToSiteGenerator
{
   public interface ISourceFileProvider<TPath>
   {
      /// <summary>
      /// Returns absolute locations of input files
      /// </summary>
      /// <param name="types">Limits what is returned to files that have source code, like markdown</param>
      /// <returns></returns>
      TPath[] GetFileLocations(FileTypes types);

      /// <summary>
      /// Returns the name of an image, typically its filename, as it can be referred to in markdown
      /// </summary>
      string GetImageTitle(TPath path);

      Task<string> GetFileContent(TPath path);
      /// <summary>
      /// Returns a stream for an image binary, rewound to the start of the stream
      /// </summary>
      Stream GetImageFileContent(TPath imInfo);

      /// <summary>
      /// Provides contents of a INI configuration file found with the documents. If none exists, it returns null
      /// </summary>
      Task<string?> GetConfigurationFileContent();

      /// <summary>
      /// Returns images and their titles (as they can be referred to in source files)
      /// </summary>
      IEnumerable<(TPath location, string title)> GetImageTitles() 
         => GetFileLocations(FileTypes.Images).Select(a => (a, GetImageTitle(a)));
   }
}