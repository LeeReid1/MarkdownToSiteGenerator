using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Converts markdown to html, one document at a time
   /// </summary>
   internal class MarkdownFileConverter<TPathIn, TPathOut>
   {
      readonly ISourceFileProvider<TPathIn> sourceProvider;
      readonly IFileWriter<TPathOut> writer;
      readonly IPathMapper<TPathIn, TPathOut> pathMapper;
      readonly MarkdownParser parser = new();

      public MarkdownFileConverter(IPathMapper<TPathIn, TPathOut> pathMapper, 
                                   ISourceFileProvider<TPathIn> sourceProvider, 
                                   IFileWriter<TPathOut> writer)
      {
         this.pathMapper = pathMapper;
         this.sourceProvider = sourceProvider;
         this.writer = writer;
      }

      public async Task ConvertAndWriteHTML(SymbolisedDocument doc, TPathIn sourceLocation, Func<string, string> rewriteLink, ICollection<(TPathIn sourceLocation, string title)>? inMenu, Configuration config)
      {
         TPathOut destination = pathMapper.GetDestination(sourceLocation);
         if (writer.FileExists(destination))
         {
            throw new Exception($"File {sourceLocation} already exists at destination {destination}");
         }
         
         var menu = inMenu?.Select(a=>(pathMapper.GetURLLocation(a.sourceLocation), a.title)).ToArray();
         HTMLGenerator generator = new(doc, config);
         StringBuilder sb = generator.Generate(rewriteLink, menu);

         await writer.Write(sb, destination);
      }

      internal async Task<SymbolisedDocument> Parse(TPathIn sourceLocation)
      {
         string content = await sourceProvider.GetFileContent(sourceLocation);
         var doc = parser.Parse(content);
         return doc;
      }
   }
}
