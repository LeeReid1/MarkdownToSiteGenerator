using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates markup and content denoting a hyperlink in a source document
   /// </summary>
   internal class Link : SymbolisedTextWithChildren
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="location">Tail markup should contain the </param>
      public Link(SymbolLocation location) : base(location)
      {
         if(location.ContentLocation2 == null)
         {
            throw new ArgumentNullException(nameof(location.ContentLocation2), $"A second content location must be provided in the provided {nameof(SymbolLocation)}");
         }
      }

      public ReadOnlySpan<char> GetHref(string source) => source.AsSpan(Location.ContentLocation2!.Value);
   }
}
