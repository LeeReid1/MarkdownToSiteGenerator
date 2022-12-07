using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   internal class Metadata : HtmlSymbol
   {
      //key that indicates this holds a title
      internal const string Key_Title = "title";
      protected override string TagCode => "";
      public bool IsTitle => Key.Equals(Key_Title, StringComparison.OrdinalIgnoreCase);
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
         if (IsTitle)
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
