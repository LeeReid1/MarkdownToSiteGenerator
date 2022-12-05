using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
    internal class HTMLGenerator
   {
      readonly string source;
      readonly SymbolisedDocument doc;
      readonly StringBuilder sb = new StringBuilder();

      public HTMLGenerator(string source, SymbolisedDocument doc)
      {
         this.source = source;
         this.doc = doc;
      }

      public StringBuilder Generate() => ToHTMLSymbols(doc, source).Write(sb);

      internal static HtmlSymbol ToHTMLSymbols(ISymbolisedText sym, string source)
      {
         HtmlSymbol htmlSymb = sym switch
         {
            MarkdownToSiteGenerator.SymbolisedDocument d => new HTML.HtmlDocument(d),
            MarkdownToSiteGenerator.Heading h => new HTML.Heading(h),
            MarkdownToSiteGenerator.Paragraph p => new HTML.Paragraph(p),
            MarkdownToSiteGenerator.List l => new HTML.List(l),
            MarkdownToSiteGenerator.ListItem li => new HTML.ListItem(li),
            MarkdownToSiteGenerator.LiteralText lt => new HTML.LiteralText(source, lt.Location.ContentLocation),
            _ => throw new NotSupportedException(sym.GetType().FullName)
         };

         // Recurse for children
         htmlSymb.Children.AddRange(sym.Children.Select(a=>ToHTMLSymbols(a, source)));

         return htmlSymb;
      }

   }
}
