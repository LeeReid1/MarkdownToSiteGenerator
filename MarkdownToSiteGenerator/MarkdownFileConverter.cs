using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
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

      public async Task ConvertAndWriteHTML(SymbolisedDocument doc, TPathIn sourceLocation, IReadOnlyDictionary<string, TPathIn> allPagesByName, ICollection<(TPathIn sourceLocation, string title)>? inMenu, Configuration config)
      {
         TPathOut destination = pathMapper.GetDestination(sourceLocation);
         if (writer.FileExists(destination))
         {
            throw new Exception("File already exists at destination");
         }
         
         var menu = inMenu?.Select(a=>(pathMapper.GetURLLocation(a.sourceLocation), a.title)).ToArray();
         HTMLGenerator generator = new(doc, config);
         StringBuilder sb = generator.Generate(title => RewriteLink(title, allPagesByName), menu);

         await writer.Write(sb, destination);
      }

      private string RewriteLink(string written, IReadOnlyDictionary<string, TPathIn> allPagesByName)
      {
         if(
            allPagesByName.TryGetValue(written, out var page) || 
            allPagesByName.TryGetValue(written.ToLowerInvariant(), out page) || 
            allPagesByName.TryGetValue(written.Replace('_', ' '), out page) || // _ is recognised as a space to allow valid markdown for titles with spaces
            allPagesByName.TryGetValue(written.ToLowerInvariant().Replace('_', ' '), out page))
         {
            return pathMapper.GetURLLocation(page);
         }
         if(written.StartsWith("/") || written.StartsWith("//") || written.StartsWith("http://") || written.StartsWith("https://") || written.StartsWith("www."))
         {
            return written;
         }
         else
         {
            throw new Exception($"Could not find page {written} to link to. Write link as the page title, or as a fully qualified URL (https://)");
         }
      }

      internal async Task<SymbolisedDocument> Parse(TPathIn sourceLocation)
      {
         string content = await sourceProvider.GetFileContent(sourceLocation);
         var doc = parser.Parse(content);
         return doc;
      }
   }
}
