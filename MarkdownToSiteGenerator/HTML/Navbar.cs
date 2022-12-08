using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   internal class Navbar : HtmlSymbol
   {
      protected override string TagCode => "nav";
      public override string CSSClass => "navbar navbar-expand-lg navbar-light bg-light";
      public string? SiteName { get; set; }
      public string HomePage { get; set; }
      public Navbar(string homePage)
      {
         HomePage = homePage;
      }
      protected override void WriteContent(StringBuilder sb)
      {
         sb.AppendLine("<div class=\"container-fluid\">");
         if(!string.IsNullOrEmpty(SiteName))
         {
            sb.Append("<a class=\"navbar-brand me-auto\" style=\"margin-left: 20px\" href=\"").Append(HomePage).Append("\">").Append(SiteName).AppendLine("</a>");
         }

sb.AppendLine(
@"      <button
         class=""navbar-toggler bg-light""
         type=""button""
         data-bs-toggle=""collapse""
         data-bs-target=""#navbarToggler""
         aria-controls=""navbarToggler""
         aria-expanded=""false""
         aria-label=""Toggle navigation"">
         <span class=""navbar-toggler-icon""></span>
      </button>
      <div class=""collapse navbar-collapse"" id=""navbarToggler"">
         <ul class=""navbar-nav ms-auto mb-2 mb-lg-0"">");
         base.WriteContent(sb);

         sb.AppendLine(@"</ul></div></div>");
      }

      public override void Insert(int position, HtmlSymbol symbol)
      {
         symbol.CSSClass = "nav-link";
         if (symbol is Link)
         {
            ListItem li = new()
            {
               CSSClass = "nav-link"
            };
            li.Add(symbol);
            symbol = li;
         }


         if (symbol is ListItem)
         {
            base.Insert(position, symbol);
         }
         else
         {
            throw new NotSupportedException($"Must be a {nameof(ListItem)} or {nameof(Link)} type");
         }
      }
   }
}
