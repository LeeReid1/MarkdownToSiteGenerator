using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Denotes a section of text that represents something, such as a heading or a paragraph
   /// </summary>
   public interface ISymbolisedText
   {
      public IEnumerable<ISymbolisedText> Children { get; }

   }

}
