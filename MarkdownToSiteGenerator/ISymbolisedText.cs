using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public interface ISymbolisedText
   {
      public IEnumerable<ISymbolisedText> Children { get; }

      /// <summary>
      /// Returns content split by children
      /// </summary>
      public IEnumerable<string> GetContentFragments(string source);
   }

}
