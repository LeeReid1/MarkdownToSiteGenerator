using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   internal class HtmlDocument : HtmlSymbol
   {
      protected override string TagCode => "html";
      protected override string TagMetaData => "lang=\"en-nz\"";
      readonly List<string> headerExtras = new();
      readonly List<Metadata> metaData = new();
      public IReadOnlyList<Metadata> Metadata => metaData;

      public override StringBuilder Write(StringBuilder sb)
      {
         sb.AppendLine("<!DOCTYPE html>");
         return base.Write(sb);
      }
      protected override void WriteContent(StringBuilder sb)
      {
         SanitiseMetadata();

         sb.AppendLine("<head>");
         metaData.ForEach(item => item.Write(sb));
         headerExtras.ForEach(item => sb.AppendLine(item));
         sb.AppendLine("</head>");

         sb.AppendLine("<body>");

         WriteMain();
         
         sb.Append("</body>");

         void WriteMain()
         {
            sb.AppendLine("<main id=\"content\" class=\"bd-content order-1 py-5\"><div class=\"container-xxl bd-gutter\">");
            base.WriteContent(sb);
            sb.AppendLine("</div></main>");
         }
      }

      private void SanitiseMetadata()
      {
         // Remove duplicate titles and ensure it is the first item so it is written first
         var titles = metaData.Where(a => a.IsTitle).ToList();
         if(titles.Count != 0)
         {
            metaData.RemoveAll(titles.Contains);
            metaData.Insert(0, titles[0]);
         }
      }

      internal void AddToHeader(string line) => headerExtras.Add(line);

      internal void AddToHeader(Metadata m) => metaData.Add(m);
   }
}
