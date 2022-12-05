using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal readonly struct SymbolLocation : IComparable<SymbolLocation>, IComparable
   {
      public SimpleRange ContentLocation { get; }
      public SimpleRange MarkupLocation_Head { get; }
      public SimpleRange MarkupLocation_Tail { get; }

      public SymbolLocation(SimpleRange headMarkupLocation, SimpleRange contentLocation, SimpleRange tailMarkupLocation)
      {
         // Sanity check
         if(tailMarkupLocation.Start != contentLocation.End)
         {
            throw new ArgumentException("The tail markup should touch the end of the content");
         }

         MarkupLocation_Head = headMarkupLocation;
         ContentLocation = contentLocation;
         MarkupLocation_Tail= tailMarkupLocation;

         
      }

      public ReadOnlySpan<char> ExtractContent(string source) => Extract(source, ContentLocation);
      public ReadOnlySpan<char> ExtractMarkupHead(string source) => Extract(source, MarkupLocation_Head);
      public ReadOnlySpan<char> ExtractMarkupTail(string source) => Extract(source, MarkupLocation_Tail);
      private static ReadOnlySpan<char> Extract(string source, SimpleRange r) => source.AsSpan(r.Start, r.Length);

      public int CompareTo(SymbolLocation other) => ContentLocation.Start.CompareTo(other.ContentLocation.Start);

      int IComparable.CompareTo(object? obj) => obj is SymbolLocation sl ? ((IComparable<SymbolLocation>)this).CompareTo(sl) : 0;
   }
}
