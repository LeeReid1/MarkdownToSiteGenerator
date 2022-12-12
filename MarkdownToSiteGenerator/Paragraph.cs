using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates content for a paragraph in a source document
   /// </summary>
   internal class Paragraph : SymbolisedTextWithChildren
   {
      public Paragraph(SymbolLocation sl) : base(sl) { }
   }
}
