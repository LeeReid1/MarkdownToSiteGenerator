using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.Markdown
{
   internal class MetadataParser : TopLevelObjectParser
   {
      protected override string RegexStr => @"(^(?:\r\n|\n)*)(\w+\:\s+[^(?:\n|\r)]*)(\r\n|\n|$)";

      public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation sl) => new Metadata(sl);

      protected override bool AcceptMatch(Match match, Match? previous) => previous == null ? match.Index == 0 : (previous.Index + previous.Length) == match.Index;
   }
}
