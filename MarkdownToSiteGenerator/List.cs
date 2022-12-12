using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   /// <summary>
   /// Notates markup and content denoting a list in a source document
   /// </summary>
   internal class List : SymbolisedTextWithChildren
   {
      public bool IsOrdered => Items.Count > 0 && ((ListItem)Items[0]).Ordered;
      public List(IList<ListItem> items) : base(new SymbolLocation(SimpleRange.Empty(items[0].Location.MarkupLocation_Head.Start), 
                                                                  new SimpleRange(items[0].Location.MarkupLocation_Head.Start, items[^1].Location.MarkupLocation_Tail.End), 
                                                                  SimpleRange.Empty(items[^1].Location.MarkupLocation_Tail.End)))
      {
         this.Items.AddRange(items);
      }
   }
}
