using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator.HTML
{
   /// <summary>
   /// Creates site maps
   /// </summary>
   internal abstract class SiteMapGenerator
   {
      public static StringBuilder WriteXMLMap(string domain, IEnumerable<string> relativeUrls)
      {
         StringBuilder w = new();
         if (string.IsNullOrWhiteSpace(domain))
         {
            throw new ArgumentException($"{domain} is required to generate an valid XML map");
         }
         if (!domain.EndsWith("/"))
         {
            domain += "/";
         }

         w.Append("<urlset>");
         foreach (var page in relativeUrls)
         {
            w.Append("<url>");
            WriteTag("loc", domain, page);
            WriteTag("lastmod", null, DateTime.UtcNow.ToString("yyyy-MM-dd"));
            WriteTag("priority", null, "1");
            WriteTag("changefreq", null, "monthly");
            w.Append("</url>");
         }
         w.Append("</urlset>");

         void WriteTag(string tag, string? preContent, string content)
         {
            w.Append('<');
            w.Append(tag);
            w.Append('>');
            if (preContent != null)
            {
               w.Append(preContent);
            }
            w.Append(content.TrimStart('/'));
            w.Append("</");
            w.Append(tag);
            w.Append('>');
         }

         return w;
      }

      public static HtmlDocument CreateHTMLMap(ICollection<(FilePath path, string title)> relativeUrls)
      {
         if(relativeUrls.Any(a=>a.path.Parts.Count == 0 || a.path.Parts[0].Length != 0))
         {
            throw new ArgumentException("Paths should be relative to the site root and so begin with a path separator");
         }


         UniqueTreeNode<string, string> tree = new UniqueTreeNode<string, string>("");
         foreach (var item in relativeUrls)
         {
            var leaf = tree.AddValueByPath(item.path.Parts.Skip(1));//skip the empty entry as we've already made that node
            leaf.Tag = item.title;
         }

         HtmlSymbol content = TreeToHTML(tree);

         HtmlDocument doc = new();
         doc.Add(content);
         doc.AddToHeader(new HTML.Metadata((HTML.Metadata.Key_Title, "Site Map")));
         
         return doc;
      }


      private static HtmlSymbol TreeToHTML(UniqueTreeNode<string, string> topItem)
      {
         List<string> path = new List<string>();

         return NodeToListItem(topItem).Children[0];//.Children[0] strips out the top list item layer, leaving the list

         HtmlSymbol NodeToListItem(UniqueTreeNode<string, string> tree)
         {
            path.Add(tree.Value);

            HtmlSymbol symb;
            if (tree.Children.Count == 0)
            {
               ListItem li = new();
               Link link = new(string.Join('/', path), tree.Tag ?? string.Join('/', path));
               li.Add(link);
               symb = li;
            }
            else
            {
               ListItem li = new();
               if (path.Count != 1)
               {
                  li.Add(new LiteralText(path[^1]));//folder name
               }
               List l = new(false);
               foreach (var child in tree.Children)
               {
                  l.Add(NodeToListItem(child));
               }
               li.Add(l);
               symb = li;
            }

            path.RemoveAt(path.Count - 1);
            return symb;
         } }

   }
}
