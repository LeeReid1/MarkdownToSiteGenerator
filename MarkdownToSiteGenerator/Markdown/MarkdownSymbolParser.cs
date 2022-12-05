using System.Text.RegularExpressions;

namespace MarkdownToSiteGenerator.Markdown
{
   internal abstract class MarkdownSymbolParser
   {
      protected abstract string RegexStr { get; }
      private readonly Lazy<Regex> r;
      public MarkdownSymbolParser()
      {
         r = new Lazy<Regex>(() => RegexStr.StartsWith(@"(") ? new Regex(RegexStr, RegexOptions.Multiline) : throw new MarkdownParseException($"Regex expected to begin with ( to indicate grouping into two chunks, but got: {RegexStr}"));
      }

      public bool MatchesLine(ReadOnlySpan<char> sourceSpan) => r.Value.IsMatch(sourceSpan);
      public IEnumerable<SymbolLocation> GetMatches(string source) => r.Value.Matches(source).Select(MatchToSymbolLocation);

      private static SymbolLocation MatchToSymbolLocation(Match m) => new(new Range(m.Groups[1].Index, m.Groups[1].Index + m.Groups[1].Length), new Range(m.Groups[2].Index, m.Groups[2].Index + m.Groups[2].Length));


   }

   internal abstract class TopLevelObjectParser : MarkdownSymbolParser
   {
      public IEnumerable<SymbolisedText> ToSymbolisedText(string source)
      {
         IEnumerable<SymbolLocation> matches = GetMatches(source);

         foreach (var item in matches.Select(ToSymbolisedText))
         {
            ParseInternals(item);
            yield return item;
         }
      }

      public abstract SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation sl);


      private void ParseInternals(SymbolisedTextWithChildren st)
      {
         //// Trim the new line from the end for the literal text
         //Range contentLoc = st.Location.ContentLocation;

         //if (contentLoc.End.Value != contentLoc.Start.Value)
         //{
         //   contentLoc = new Range(contentLoc.Start, contentLoc.End.Value - 1);
         //}
         st.Items.Add(new LiteralText(st.Location.ContentLocation));
      }
   }
}
