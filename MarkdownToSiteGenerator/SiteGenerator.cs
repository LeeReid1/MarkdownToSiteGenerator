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

      public IEnumerable<TPathIn> GetInputFileLocations() => sourceFileProvider.GetFileLocations(FileTypes.All);
      public IEnumerable<TPathOut> GetOutputFileLocations() => GetInputFileLocations().Select(pathMapper.GetDestination);
      public IEnumerable<TPathOut> GetWillBeOverwritten() => GetOutputFileLocations().Where(fileWriter.FileExists);


      /// <summary>
      /// Parses all inputs that the source file provider identifies as source code
      /// </summary>
      /// <returns></returns>
      internal async IAsyncEnumerable<(TPathIn location, SymbolisedDocument doc)> ParseAllInputs()
      {
         TPathIn[] locations = sourceFileProvider.GetFileLocations(FileTypes.SourceFiles);

         foreach (var loc in locations)
         {
            yield return (loc, await converter.Parse(loc));
         }
      }
      public async Task Generate()
      {
         Configuration config = Configuration.IniToConfiguration(await sourceFileProvider.GetConfigurationFileContent());

         List<(TPathIn location, SymbolisedDocument doc)> docs = await ParseAllInputs().ToListAsync();
         List<(TPathIn location, string title)> docTitles = GetTitles(docs);

         TitleToURLLookup<TPathIn> urlLookup = CreateURLLookup(docTitles, config);

         foreach ((TPathIn location, SymbolisedDocument doc) in docs)
         {
            await converter.ConvertAndWriteHTML(doc, location, urlLookup.GetURL, docTitles, config);
         }

         await GenerateSiteMapIfConfigAllows(config, docTitles, urlLookup.GetURL);

         foreach(var imInfo in sourceFileProvider.GetFileLocations(FileTypes.Images))
         {
            using Stream s = sourceFileProvider.GetImageFileContent(imInfo);
            await fileWriter.WriteBinary(s, pathMapper.GetDestination(imInfo));
         }
         
      }


      private TitleToURLLookup<TPathIn> CreateURLLookup(List<(TPathIn location, string title)> docTitles, Configuration config)
      {
         TitleToURLLookup<TPathIn> urlLookup = new(pathMapper);
         if (config.CreateSiteMaps)
         {
            // sitemap is a reserved title
            urlLookup.AddURL("sitemap", pathMapper.GetURL_Sitemap_HTML);
         }
         urlLookup.AddRange(docTitles);
         urlLookup.AddRange(sourceFileProvider.GetImageTitles());

         return urlLookup;
      }

      private async Task GenerateSiteMapIfConfigAllows(Configuration config, IEnumerable<(TPathIn path, string title)> locations, Func<string, string> rewriteLink)
      {
         if (config.CreateSiteMaps)
         {
            if (config.DestinationDomain != null)
            {
               await WriteSiteMap_XML(config.DestinationDomain, locations.Select(a=>a.path));
            }

            await WriteSiteMap_HTML(config, locations, rewriteLink);
         }
      }

      private async Task WriteSiteMap_XML(string domain, IEnumerable<TPathIn> locations)
      {
         StringBuilder map = SiteMapGenerator.WriteXMLMap(domain, locations.Select(pathMapper.GetURLLocation));
         await fileWriter.Write(map, pathMapper.GetDestination_Sitemap_XML());
      }

      private async Task WriteSiteMap_HTML(Configuration config, IEnumerable<(TPathIn path, string title)> locations, Func<string, string> rewriteLink)
      {
         HtmlDocument doc = SiteMapGenerator.CreateHTMLMap(locations.Select(a => ( new FilePath(pathMapper.GetURLLocation(a.path)), a.title)).ToList());
         HTMLGenerator.AddOptionalsToDoc(config, null, doc, rewriteLink);
         StringBuilder map = doc.Write(new StringBuilder());
         await fileWriter.Write(map, pathMapper.GetDestination_Sitemap_HTML());
      }


      private static List<(TPathIn location, string title)> GetTitles(List<(TPathIn location, SymbolisedDocument doc)> docs)
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
