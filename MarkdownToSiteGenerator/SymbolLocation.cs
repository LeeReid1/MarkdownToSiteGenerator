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
      public SimpleRange? ContentLocation2 { get; }
      public SimpleRange MarkupLocation_Head { get; }
      public SimpleRange MarkupLocation_Tail { get; }
      /// <summary>
      /// The area this takes up in total
      /// </summary>
      public SimpleRange FullRange => new SimpleRange(MarkupLocation_Head.Start, MarkupLocation_Tail.End);

      public SymbolLocation(SimpleRange headMarkupLocation, SimpleRange contentLocation, SimpleRange contentLocation2, SimpleRange tailMarkupLocation)
      {
         // Sanity check
         if(tailMarkupLocation.Start != contentLocation2.End)
         {
            throw new ArgumentException("The tail markup should touch the end of the second content");
         }

         MarkupLocation_Head = headMarkupLocation;
         ContentLocation = contentLocation;
         ContentLocation2 = contentLocation2;
         MarkupLocation_Tail= tailMarkupLocation;
      }
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
      public bool TryExtractContent2(string source, out ReadOnlySpan<char> chars)
      {
         if (ContentLocation2 == null)
         {
            chars = default;
            return false;
         }
         else
         {
            chars = Extract(source, ContentLocation2.Value);
            return true;
         }
      }
      public ReadOnlySpan<char> ExtractMarkupHead(string source) => Extract(source, MarkupLocation_Head);
      public ReadOnlySpan<char> ExtractMarkupTail(string source) => Extract(source, MarkupLocation_Tail);
      private static ReadOnlySpan<char> Extract(string source, SimpleRange r) => source.AsSpan(r.Start, r.Length);

      public int CompareTo(SymbolLocation other) => ContentLocation.Start.CompareTo(other.ContentLocation.Start);

      int IComparable.CompareTo(object? obj) => obj is SymbolLocation sl ? ((IComparable<SymbolLocation>)this).CompareTo(sl) : 0;
   }
}
