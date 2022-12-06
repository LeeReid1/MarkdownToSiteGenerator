using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   internal class Metadata : HtmlSymbol
   {
      protected override string TagCode => "";

      readonly string key;
      readonly string value;
      public Metadata((string key, string value) vals)
      {
         this.key = vals.key;
         this.value = vals.value;
      }

      protected override void WriteContent(StringBuilder sb)
      { }

      public override StringBuilder Write(StringBuilder sb)
      {
         if (key.Equals("title", StringComparison.OrdinalIgnoreCase))
         {
            // special case
            sb.Append("<title>").Append(value).AppendLine("</title>");
         }
         else
         {
            sb.Append("<meta property=\"").Append(key).Append("\" content=\"").Append(value).AppendLine("\">");
         }
         return sb;
      }
   }
}
