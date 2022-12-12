using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   internal class TitleToURLLookup<TPathIn>
   {
      readonly Dictionary<string, string> urlByTitle = new(StringComparer.InvariantCultureIgnoreCase);
      readonly IPathToURLMapper<TPathIn> pathMapper;

      public TitleToURLLookup(IPathToURLMapper<TPathIn> pathMapper)
      {
         this.pathMapper = pathMapper;
      }


      public void AddRange(IEnumerable<(TPathIn sourceLocation, string title)> items) => items.ForEach(a => Add(a.title, a.sourceLocation));
      public void Add(string title, TPathIn sourceLocation) => AddURL(title, pathMapper.GetURLLocation(sourceLocation));

      public void AddURL(string title, string url)
      {
         if (!urlByTitle.TryAdd(NormaliseTitle(title), url))
         {
            throw new Exception("Page titles are not unique. Titles are not case-sensitive, and underscores and spaces are considered equivalent. Do not set a title to the same name as an image filename.");
         }
      }

      public string GetURL(string titleOrLink)
      {
         if (titleOrLink.StartsWith("http://") || 
            titleOrLink.StartsWith("https://") || 
            titleOrLink.StartsWith("www."))
         {
            return titleOrLink;
         }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
         if (urlByTitle.TryGetValue(NormaliseTitle(titleOrLink), out string url))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
         {
            return url;
         }
         throw new Exception($"Title {titleOrLink} points to a page or resource that was not found. Write link as the page title, or as a fully qualified URL (https://)");
      }

      private static string NormaliseTitle(string s) => s.Replace(' ', '_').ToLowerInvariant(); // _ is recognised as a space to allow valid markdown for titles with spaces

   }
}
