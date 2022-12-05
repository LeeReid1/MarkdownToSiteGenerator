using MarkdownToSiteGenerator.HTML;
using MarkdownToSiteGenerator.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class MarkdownFileConverter
   {
      readonly PathMapper pathMapper;
      readonly  MarkdownParser parser = new();
      
      public MarkdownFileConverter(PathMapper pathMapper)
      {
         this.pathMapper = pathMapper;
      }

      public void ConvertAndWriteHTML(string sourceLocation)
      {
         string destination = pathMapper.GetDestination(sourceLocation);
         if(File.Exists(destination))
         {
            throw new Exception("File already exists at destination");
         }
         string content = File.ReadAllText(sourceLocation);

         var doc = parser.Parse(content);

         HTMLGenerator generator = new HTMLGenerator(content, doc);

         Directory.CreateDirectory(Path.GetDirectoryName(destination) ?? throw new NullReferenceException(nameof(destination)));


         using StreamWriter stream= new(destination,false,Encoding.UTF8,128000);
         stream.WriteAsync(generator.Generate());
      }
   }
}
