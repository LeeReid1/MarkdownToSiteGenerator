using System.Text.RegularExpressions;

namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Base class for markdown symbol parsers
   /// </summary>
   internal abstract class MarkdownSymbolParser
   {
      protected abstract string RegexStr { get; }
      private readonly Lazy<Regex> r;
      public MarkdownSymbolParser()
      {
         r = new Lazy<Regex>(() => RegexStr.StartsWith(@"(") ? new Regex(RegexStr, RegexOptions.Multiline) : throw new MarkdownParseException($"Regex expected to begin with ( to indicate grouping into two chunks, but got: {RegexStr}"));
      }

      public bool MatchesLine(ReadOnlySpan<char> sourceSpan) => r.Value.IsMatch(sourceSpan);
      public IEnumerable<SymbolLocation> GetMatches(string source) => GetMatches(source, 0, source.Length);
      public IEnumerable<SymbolLocation> GetMatches(string source, int from, int length)
      {
         Match? previous = null;
         Match m = r.Value.Match(source, from, length);
         while (m.Success && AcceptMatch(m, previous))
         {
            yield return MatchToSymbolLocation(m);

            previous = m;
            m = m.NextMatch();
         }
      }

      private static SymbolLocation MatchToSymbolLocation(Match m)
      {
         return new(ToRange(m.Groups[1]), ToRange(m.Groups[2]), ToRange(m.Groups[3]));

         static SimpleRange ToRange(Group g) => new(g.Index, g.Index + g.Length);
      }


      protected virtual bool AcceptMatch(Match match, Match? previous) => true;
   }
}
