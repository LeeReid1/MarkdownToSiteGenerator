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
      readonly Configuration config;


      public HTMLGenerator(SymbolisedDocument doc, Configuration config)
      {
         this.doc = doc;
         this.config = config;
      }

      public StringBuilder Generate(Func<string, string> linkRewriter, ICollection<(string url, string title)>? itemsInNavBar = null, IEnumerable<string>? styleURLs=null)
      {
         HTML.HtmlDocument htmlDoc = (HtmlDocument)ToHTMLSymbols(doc, doc.Source, linkRewriter);
         AddOptionalsToDoc(config, itemsInNavBar, htmlDoc, linkRewriter, styleURLs);

         return htmlDoc.Write(new StringBuilder());
      }

      /// <summary>
      /// Adds things other than content to the doc, like the navigation bar and styling
      /// </summary>
      internal static void AddOptionalsToDoc(Configuration config, ICollection<(string url, string title)>? itemsInNavBar, HtmlDocument htmlDoc, Func<string, string> linkRewriter, IEnumerable<string>? styleURLs)
      {
         if (config.IncludeBootstrap_CSS)
         {
            htmlDoc.AddToHeader("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65\" crossorigin=\"anonymous\">");
         }
         if (config.IncludeBootstrap_JS)
         {
            htmlDoc.AddToHeader("<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4\" crossorigin=\"anonymous\"></script>");
         }

         if (styleURLs != null)
         {
            foreach (string styleSheetURL in styleURLs)
            {
               htmlDoc.AddToHeader($"<link href=\"{styleSheetURL}\" rel=\"stylesheet\">");
            }
         }

         if (itemsInNavBar != null)
         {
            Navbar navbar = new(linkRewriter(config.HomePage))
            {
               SiteName = config.SiteName
            };
            itemsInNavBar.Select(cur => new Link(cur.url, cur.title)).ForEach(navbar.Add);
            htmlDoc.Insert(0, navbar);
         }
      }

      internal static HtmlSymbol ToHTMLSymbols(ISymbolisedText sym, string source) => ToHTMLSymbols(sym, source, a => a);
      /// <param name="linkRewriter">A function that swaps one link for another URL. If not wanted, provide a=>a</param>
      internal static HtmlSymbol ToHTMLSymbols(ISymbolisedText sym, string source, Func<string, string> linkRewriter)
      {
         HtmlSymbol htmlSymb = sym switch
         {
            MarkdownToSiteGenerator.SymbolisedDocument d => new HTML.HtmlDocument(),
            MarkdownToSiteGenerator.Heading h => new HTML.Heading(h),
            MarkdownToSiteGenerator.Paragraph p => new HTML.Paragraph(),
            MarkdownToSiteGenerator.Link link => new HTML.Link() {  HRef= linkRewriter(link.GetHref(source).ToString()) },
            MarkdownToSiteGenerator.Image img => new HTML.Image(linkRewriter(img.GetHref(source).ToString()), img.GetAltText(source).ToString()),
            MarkdownToSiteGenerator.List l => new HTML.List(l.IsOrdered),
            MarkdownToSiteGenerator.ListItem li => new HTML.ListItem(),
            MarkdownToSiteGenerator.LiteralText lt => new HTML.LiteralText(source, lt.Location.ContentLocation),
            MarkdownToSiteGenerator.Metadata lt => new HTML.Metadata(lt.GetKeyAndValue(source)),
            _ => throw new NotSupportedException(sym.GetType().FullName)
         };

         // Recurse for children
         if (htmlSymb is HtmlSymbolWithChildren hswc)
         {
            sym.Children.Select(a => ToHTMLSymbols(a, source, linkRewriter)).ForEach(AddChild);
         }
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
               hswc.Add(addMe);
            }
         }
      }

   }
}
