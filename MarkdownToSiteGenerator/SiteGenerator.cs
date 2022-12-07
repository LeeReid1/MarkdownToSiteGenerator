﻿using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public class SiteGenerator<TPathIn, TPathOut> 
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
         List<(TPathIn location, SymbolisedDocument doc)> docs = await ParseAllInputs().ToListAsync();
         List<(TPathIn location, string title)> withTitle = GetMenuItemPaths(docs);

         foreach ((TPathIn location, SymbolisedDocument doc) in docs)
         {
            await converter.ConvertAndWriteHTML(doc, location, withTitle);
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

   /// <summary>
   /// A site generator that reads from and writes to the file system
   /// </summary>
   public class SiteGenerator_FolderToFolder : SiteGenerator<FilePath, FilePath>
   {
      public SiteGenerator_FolderToFolder(string dir_from, string dir_to) : base(new FileSourceProvider(dir_from), new PathMapper(dir_from, dir_to), new FileWriter())
      {
         FilePath fpFrom = dir_from;
         FilePath fpTo = dir_to;

         if(fpFrom.Equals(fpTo))
         {
            throw new ArgumentException("Read and write locations cannot be the same");
         }
      }
   }
}
