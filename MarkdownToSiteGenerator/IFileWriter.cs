using System.Text;

namespace MarkdownToSiteGenerator
{
   public interface IFileWriter<TPathOut>
   {
      bool FileExists(TPathOut path);
      void Delete(TPathOut path);
      Task Write(StringBuilder content, TPathOut destination);
      Task Write(string content, TPathOut destination);
      Task WriteBinary(Stream s, TPathOut pathOut);
   }
}