namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Parses objects that can contain others
   /// </summary>
   internal abstract class ParentMarkdownSymbolParser : MarkdownSymbolParser
   {
      static readonly IReadOnlyList<MarkdownSymbolParser> InternalParsers = new MarkdownSymbolParser[]
      {
          new LinkParser(),
          new ImageParser()
      };

      static readonly IReadOnlyList<BadMarkdownChecker> MarkdownErrorParsers = new BadMarkdownChecker[]
      {
          new BadLinkOrImageParser_SpaceInTitleOrURL(),
          new BadLinkOrImageParser_BadBrackets(),
      };

      public override IEnumerable<SymbolisedText> ToSymbolisedText(string source, int from, int length) => GetMatches(source, from, length).Select(ToSymbolisedText).ForEachIterable(a => { if (a is SymbolisedTextWithChildren st) { ParseContentWithinParent(source, a.Location.ContentLocation, st); } });

      internal static void ParseContentWithinParent(string source, SimpleRange sourceRange, SymbolisedTextWithChildren st)
      {
         Sub(sourceRange, 0);

         void Sub(SimpleRange rangeToParse, int startFromParser)
         {
            for (int iParser = startFromParser; iParser < InternalParsers.Count; iParser++)
            {
               // Parse the source range requested
               MarkdownSymbolParser parser = InternalParsers[iParser];
               List<SymbolisedText> children = parser.ToSymbolisedText(source, rangeToParse).ToList();
               if (children.Count != 0)
               {
                  // Successful parsing
                  st.Items.AddRange(children);
                  st.Items.Sort();

                  // For what remains, split the source and parse the fragments separately
                  // (doing so avoids issues in case of badly formed MD that results in overlap)
                  // Don't re-parse with parsers that have already run, for efficiency
                  IEnumerable<SimpleRange> fragments = rangeToParse.SplitExclude(children.Select(a => a.Location.FullRange));
                  foreach (var fragment in fragments)
                  {
                     Sub(fragment, iParser + 1);
                  }

                  return;
               }
            }

            // nothing parsed. 
            // Check for bad markdown
            for (int iParser = startFromParser; iParser < MarkdownErrorParsers.Count; iParser++)
            {
               // Parse the source range requested
               BadMarkdownChecker parser = MarkdownErrorParsers[iParser];
               parser.ThrowIfBadMarkdownFound(source);
            }


            // Create a literal text item
            st.Items.Add(new LiteralText(rangeToParse));
            st.Items.Sort();
         }
      }
   }
}
