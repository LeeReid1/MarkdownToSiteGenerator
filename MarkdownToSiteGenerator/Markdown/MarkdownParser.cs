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


         // All list items should be inside a list
         SymbolisedText last = null;
         var symbolsCopy = symbols.ToArray();
         symbols.Clear();
         foreach (var symbol in symbols)
         {
            if(symbol is ListItem && !(last is ListItem))
            {
#error to do
            }
         }

         // Any remaining symbols are paragraphs
         AddParagraphs(text, symbols);

         SymbolisedDocument doc = new(text);

         doc.Items.AddRange(symbols.Select(a => a));
         return doc;
      }

      private static void AddParagraphs(string text, List<SymbolisedText> symbols)
      {
         List<Paragraph> paragraphs = new();
         int lastEnd = 0;
         while (lastEnd < text.Length)
         {
            var next = symbols.FirstOrDefault(a => a.Location.MarkupLocation.Start.Value == lastEnd);
            if (next != null)
            {
               lastEnd = next.Location.MarkupLocation.End.Value;
               continue;
            }

            var end = symbols.FirstOrDefault(a => a.Location.MarkupLocation.Start.Value >= lastEnd)?.Location.MarkupLocation.Start.Value ?? text.Length;

            paragraphs.Add(new Paragraph(new SymbolLocation(new Range(lastEnd, lastEnd), new Range(lastEnd, end))));

            lastEnd = end;
         }

         symbols.AddRange(paragraphs);
         symbols.Sort();
      }
   }
}
