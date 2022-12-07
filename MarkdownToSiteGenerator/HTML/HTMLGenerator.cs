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
      readonly SymbolisedDocument doc;

      public bool IncludeBootstrap { get; set; } = true;
      public HTMLGenerator(SymbolisedDocument doc)
      {
         this.doc = doc;
      }

      public StringBuilder Generate(ICollection<(string url, string title)>? itemsInNavBar = null)
      {
         HTML.HtmlDocument htmlDoc = (HtmlDocument)ToHTMLSymbols(doc, doc.Source);
         if (IncludeBootstrap)
         {
            htmlDoc.AddToHeader("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65\" crossorigin=\"anonymous\">");
            htmlDoc.AddToHeader("<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4\" crossorigin=\"anonymous\"></script>");
         }

         if (itemsInNavBar != null)
         {
            Navbar navbar = new();
            itemsInNavBar.Select(cur => new Link(cur.url, cur.title)).ForEach(navbar.Add);
            htmlDoc.Insert(0, navbar);
         }

         return htmlDoc.Write(new StringBuilder());
      }

      internal static HtmlSymbol ToHTMLSymbols(ISymbolisedText sym, string source)
      {
         HtmlSymbol htmlSymb = sym switch
         {
            MarkdownToSiteGenerator.SymbolisedDocument d => new HTML.HtmlDocument(),
            MarkdownToSiteGenerator.Heading h => new HTML.Heading(h),
            MarkdownToSiteGenerator.Paragraph p => new HTML.Paragraph(p),
            MarkdownToSiteGenerator.List l => new HTML.List(l),
            MarkdownToSiteGenerator.ListItem li => new HTML.ListItem(),
            MarkdownToSiteGenerator.LiteralText lt => new HTML.LiteralText(source, lt.Location.ContentLocation),
            MarkdownToSiteGenerator.Metadata lt => new HTML.Metadata(lt.GetKeyAndValue(source)),
            _ => throw new NotSupportedException(sym.GetType().FullName)
         };

         // Recurse for children
         sym.Children.Select(a => ToHTMLSymbols(a, source)).ForEach(AddChild);

         return htmlSymb;

         void AddChild(HtmlSymbol addMe)
         {
            if(addMe is HTML.Metadata m)
            {
               if(htmlSymb is HtmlDocument doc)
               {
                  doc.AddToHeader(m);
               }
               else
               {
                  throw new Exception("Metadata can only be at the document level");
               }
            }
            else
            {
               htmlSymb.Add(addMe);
            }
         }
      }

   }
}
