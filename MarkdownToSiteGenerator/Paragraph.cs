using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class Paragraph : SymbolisedTextWithChildren
   {
      public Paragraph(SymbolLocation sl) : base(sl) { }
   }
}
