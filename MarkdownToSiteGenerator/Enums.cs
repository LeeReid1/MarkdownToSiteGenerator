using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   [Flags]
   public enum FileTypes
   {
      None = 0,
      SourceFiles=0x1,
      Images = 0x2,
      Style = 0x4,
      All = SourceFiles| Images | Style,
   }
}
