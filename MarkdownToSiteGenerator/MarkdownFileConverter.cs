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

      public async Task ConvertAndWriteHTML(TPathIn sourceLocation, TPathIn[] inMenu)
      {
         TPathOut destination = pathMapper.GetDestination(sourceLocation);

         string content = await sourceProvider.GetFileContent(sourceLocation);

         ConvertAndWriteHTML(content, destination, inMenu);

      }

      public void ConvertAndWriteHTML(string content, TPathOut destination, TPathIn[] inMenu)
      {
         if (writer.FileExists(destination))
         {
            throw new Exception("File already exists at destination");
         }

         var doc = parser.Parse(content);
         string[]? menu = inMenu?.Select(pathMapper.GetURLLocation).ToArray();
         HTMLGenerator generator = new(content, doc);
         StringBuilder sb = generator.Generate(menu);

         writer.Write(sb, destination);
      }
   }
}
