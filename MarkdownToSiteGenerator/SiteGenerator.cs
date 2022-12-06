using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public class SiteGenerator<TPathIn, TPathOut> 
      where TPathIn : IPath
      where TPathOut : IPath
   {
      readonly ISourceFileProvider<TPathIn> sourceFileProvider;
      readonly IPathMapper<TPathIn, TPathOut> pathMapper;
      readonly IFileWriter<TPathOut> fileWriter;

      public SiteGenerator(ISourceFileProvider<TPathIn> sourceFileProvider, IPathMapper<TPathIn, TPathOut> pathMapper, IFileWriter<TPathOut> fileWriter)
      {
         this.sourceFileProvider = sourceFileProvider;
         this.pathMapper = pathMapper;
         this.fileWriter = fileWriter;
      }

      public IEnumerable<TPathIn> GeInputFileLocations() => sourceFileProvider.GetFileLocations(false);
      public IEnumerable<TPathOut> GetOutputFileLocations() => GeInputFileLocations().Select(pathMapper.GetDestination);
      public IEnumerable<TPathOut> GetWillBeOverwritten() => GetOutputFileLocations().Where(fileWriter.FileExists);

      

      public async Task Generate()
      {
         //TPathOut[] fileLocations = sourceFileProvider.GetAllFileLocations();
         //TPathOut[] fileDestinations = GetOutputFileLocations().ToArray();


         //Dictionary<string, string> metadata = sourceFileProvider.GetUniversalMetadata();

         
         MarkdownFileConverter<TPathIn,TPathOut> converter = new(pathMapper, sourceFileProvider, fileWriter);
         TPathIn[] fileLocationsIn = sourceFileProvider.GetFileLocations(true);
         foreach (TPathIn location in fileLocationsIn)
         {
            await converter.ConvertAndWriteHTML(location, fileLocationsIn);
         }
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
