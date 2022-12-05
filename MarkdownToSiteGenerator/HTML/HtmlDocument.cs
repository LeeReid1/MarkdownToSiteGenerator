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

      readonly SymbolisedDocument d;
      public HtmlDocument(SymbolisedDocument d)
      {
         this.d = d;
      }

      public override StringBuilder Write(StringBuilder sb)
      {
         sb.AppendLine("<!DOCTYPE html>");
         return base.Write(sb);
      }
      protected override void WriteContent(StringBuilder sb)
      {
         sb.AppendLine("<head></head>");
         sb.AppendLine("<body>");
         base.WriteContent(sb);
         sb.Append("</body>");
      }
   }
}
