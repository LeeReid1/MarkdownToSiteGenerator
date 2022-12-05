using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.Markdown
{
    internal class MarkdownParser
   {
      IReadOnlyList<TopLevelObjectParser> topLevelParsers { get; } = new List<TopLevelObjectParser>()
      {
         new HeaderSymbolParser(1),
         new HeaderSymbolParser(2),
         new HeaderSymbolParser(3),
         new HeaderSymbolParser(4),
         new HeaderSymbolParser(5),
         new HeaderSymbolParser(6),
         new OrderedListItemParser()
      };

      public SymbolisedDocument Parse(string text)
      {

         List<SymbolisedText> symbols = new();
         foreach (var parser in topLevelParsers)
         {
            symbols.AddRange(parser.ToSymbolisedText(text));
         }

         symbols.Sort();


         // Any remaining symbols are paragraphs
         AddParagraphs(text, symbols);

         symbols = PutListItemsInLists(symbols);

         SymbolisedDocument doc = new(text);

         doc.Items.AddRange(symbols);
         return doc;
      }

      private static List<SymbolisedText> PutListItemsInLists(List<SymbolisedText> symbols)
      {
         // All list items should be inside a list
         List<SymbolisedText> symbols2 = new();

         for (int i = 0; i < symbols.Count; i++)
         {
            if (symbols[i] is ListItem)
            {
               List l = CollectListGroup(symbols, ref i);
               symbols2.Add(l);
            }
            else
            {
               symbols2.Add(symbols[i]);
            }
         }

         return symbols2;

         static List CollectListGroup(List<SymbolisedText> symbols, ref int i)
         {
            List<ListItem> group = symbols.Skip(i).TakeWhileType<SymbolisedText, ListItem>().ToList();
            i += group.Count - 1;//loop increments 1 so minus 1
            return new List(group);
         }
      }

      private static void AddParagraphs(string text, List<SymbolisedText> symbols)
      {
         ParagraphSymbolParser paragraphSymbolParser = new();

         var paragraphs = GetNonSymbolisedZones(text, symbols).SelectMany(r => paragraphSymbolParser.ToSymbolisedText(text, r.Start, r.Length));
         symbols.AddRange(paragraphs);
         symbols.Sort();
      }

      private static IEnumerable<SimpleRange> GetNonSymbolisedZones(string text, List<SymbolisedText> symbols)
      {
         List<SymbolisedTextWithChildren> paragraphs = new();
         int lastEnd = 0;
         while (lastEnd < text.Length)
         {
            var next = symbols.FirstOrDefault(a => a.Location.MarkupLocation_Head.Start == lastEnd);
            if (next != null)
            {
               lastEnd = next.Location.MarkupLocation_Tail.End;
               continue;
            }

            // We're in a space which has not been tokenised
            // Find its end
            int paragraphBlockEnd = symbols.FirstOrDefault(a => a.Location.MarkupLocation_Head.Start >= lastEnd)?.Location.MarkupLocation_Head.Start ?? text.Length;

            yield return new SimpleRange(lastEnd, paragraphBlockEnd);

            lastEnd = paragraphBlockEnd;
         }

      }
   }
}
