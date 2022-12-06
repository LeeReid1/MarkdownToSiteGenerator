using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   internal class HtmlDocument : HtmlSymbol
   {
      protected override string TagCode => "html";
      protected override string TagMetaData => "lang=\"en-nz\"";
      readonly List<string> headerExtras = new();

      public override StringBuilder Write(StringBuilder sb)
      {
         sb.AppendLine("<!DOCTYPE html>");
         return base.Write(sb);
      }
      protected override void WriteContent(StringBuilder sb)
      {
         sb.AppendLine("<head>");
         foreach (var item in headerExtras)
         {
            sb.AppendLine(item);
         }
         sb.AppendLine("</head>");

         sb.AppendLine("<body>");

         WriteMain();
         
         sb.Append("</body>");


//         void WriteHeader()
//         {
//            if (menuLinks.Count == 0)
//            {
//               return;
//            }

//            sb.Append(
//@"<nav class=""navbar navbar-expand-lg navbar-light bg-light"">
//    <div class=""container-fluid"">
//      <a class=""navbar-brand me-auto"" style=""margin-left: 20px"" href=""#"">Example Name</a>
//        <button
//          class=""navbar-toggler bg-light""
//          type=""button""
//          data-bs-toggle=""collapse""
//          data-bs-target=""#navbarToggler""
//          aria-controls=""navbarToggler""
//          aria-expanded=""false""
//          aria-label=""Toggle navigation"">
//          <span class=""navbar-toggler-icon""></span>
//        </button>
//        <div class=""collapse navbar-collapse"" id=""navbarToggler"">
//          <ul class=""navbar-nav ms-auto mb-2 mb-lg-0"">");

//            menuLinks.Write(sb);

//            foreach (var item in menuLinks)
//            {
//               sb.Append("<li class=\"nav-item\"><a class=\"nav-link\" href=\"").Append().Append("\">").Append().Append("</a></li>");
//            }

//            sb.AppendLine(@"</ul>
//         </div>
//   </div>
//</nav>");
//         }

         void WriteMain()
         {
            //sb.AppendLine("<div class=\"col-lg-8 mx-auto p-4 py-md-5\">").AppendLine("<main>");
            sb.AppendLine("<main id=\"content\" class=\"bd-content order-1 py-5\"><div class=\"container-xxl bd-gutter\">");
            base.WriteContent(sb);
            //sb.AppendLine("</main>").AppendLine("</div>");
            sb.AppendLine("</div></main>");
         }
      }

      

      internal void AddToHeader(string line)
      {
         headerExtras.Add(line);
      }
   }
}
