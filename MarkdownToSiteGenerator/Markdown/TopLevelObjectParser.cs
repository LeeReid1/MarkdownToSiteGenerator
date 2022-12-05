namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Parses objects that do not sit inside others
   /// </summary>
   internal abstract class TopLevelObjectParser : MarkdownSymbolParser
   {
      public IEnumerable<SymbolisedTextWithChildren> ToSymbolisedText(string source) => ToSymbolisedText(source, 0, source.Length);
      public IEnumerable<SymbolisedTextWithChildren> ToSymbolisedText(string source, int from, int length) => GetMatches(source, from, length).Select(ToSymbolisedText).ForEach(ParseInternals);

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
