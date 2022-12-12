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
      SourceFiles,
      Images,
      All = SourceFiles| Images,
   }
}
