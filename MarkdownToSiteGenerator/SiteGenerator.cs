using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public class SiteGenerator<TPathIn, TPathOut> where TPathIn:IComparable
   {
      readonly ISourceFileProvider<TPathIn> sourceFileProvider;
      readonly IPathMapper<TPathIn, TPathOut> pathMapper;
      readonly IFileWriter<TPathOut> fileWriter;
      readonly MarkdownFileConverter<TPathIn, TPathOut> converter;
      public SiteGenerator(ISourceFileProvider<TPathIn> sourceFileProvider, IPathMapper<TPathIn, TPathOut> pathMapper, IFileWriter<TPathOut> fileWriter)
      {
         this.sourceFileProvider = sourceFileProvider;
         this.pathMapper = pathMapper;
         this.fileWriter = fileWriter;
         converter = new(pathMapper, sourceFileProvider, fileWriter);
      }

      public IEnumerable<TPathIn> GetInputFileLocations() => sourceFileProvider.GetFileLocations(false);
      public IEnumerable<TPathOut> GetOutputFileLocations() => GetInputFileLocations().Select(pathMapper.GetDestination);
      public IEnumerable<TPathOut> GetWillBeOverwritten() => GetOutputFileLocations().Where(fileWriter.FileExists);


      /// <summary>
      /// Parses all inputs that the source file provider identifies as source code
      /// </summary>
      /// <returns></returns>
      internal async IAsyncEnumerable<(TPathIn location, SymbolisedDocument doc)> ParseAllInputs()
      {
         TPathIn[] locations = sourceFileProvider.GetFileLocations(true);

         foreach (var loc in locations)
         {
            yield return (loc, await converter.Parse(loc));
         }
      }
      public async Task Generate()
      {
         Configuration config =  Configuration.IniToConfiguration(await sourceFileProvider.GetConfigurationFileContent());

         List<(TPathIn location, SymbolisedDocument doc)> docs = await ParseAllInputs().ToListAsync();
         List<(TPathIn location, string title)> withTitle = GetMenuItemPaths(docs);

         foreach ((TPathIn location, SymbolisedDocument doc) in docs)
         {
            await converter.ConvertAndWriteHTML(doc, location, withTitle, config);
         }
      }

      private static List<(TPathIn location, string title)> GetMenuItemPaths(List<(TPathIn location, SymbolisedDocument doc)> docs)
      {
         return docs.Select(a => (a.location, a.doc, title: a.doc.TryGetTitle()))
                                    .Where(a => a.title != null)
                                    .Select(a => (a.location, title:a.title!))
                                    .OrderBy(a => a.location)
                                    .ToList();
      }

      internal void DeleteDestinationFiles() => GetOutputFileLocations().ForEach(fileWriter.Delete);
   }
}
