using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal readonly struct SymbolLocation : IComparable<SymbolLocation>, IComparable
   {
      public Range ContentLocation { get; }
      public Range MarkupLocation { get; }

      public SymbolLocation(Range markupLocation, Range contentLocation)
      {
         MarkupLocation = markupLocation;
         ContentLocation = contentLocation;
      }

      public ReadOnlySpan<char> ExtractContent(string source) => Extract(source, ContentLocation);
      public ReadOnlySpan<char> ExtractMarkup(string source) => Extract(source, MarkupLocation);
      private static ReadOnlySpan<char> Extract(string source, Range r) => source.AsSpan(r.Start.Value, r.End.Value - r.Start.Value);

      public int CompareTo(SymbolLocation other) => ContentLocation.Start.Value.CompareTo(other.ContentLocation.Start.Value);

      int IComparable.CompareTo(object? obj) => obj is SymbolLocation sl ? ((IComparable<SymbolLocation>)this).CompareTo(sl) : -1;
   }
}
