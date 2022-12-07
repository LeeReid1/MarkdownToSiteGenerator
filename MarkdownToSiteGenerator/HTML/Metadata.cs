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

      public string Key { get; }
      public string Value { get; }
      public Metadata((string key, string value) vals)
      {
         this.Key = vals.key;
         this.Value = vals.value;
      }

      protected override void WriteContent(StringBuilder sb)
      { }

      public override StringBuilder Write(StringBuilder sb)
      {
         if (Key.Equals("title", StringComparison.OrdinalIgnoreCase))
         {
            // special case
            sb.Append("<title>").Append(Value).AppendLine("</title>");
         }
         else
         {
            sb.Append("<meta property=\"").Append(Key).Append("\" content=\"").Append(Value).AppendLine("\">");
         }
         return sb;
      }
   }
}
