namespace MarkdownToSiteGenerator
{
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
