using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   [TestClass]
   public class SiteMapGeneratorTests
   {
      [TestMethod]
      public void WriteXMLMap_Empty()
      {
         string result = SiteMapGenerator.WriteXMLMap("https://example.com/", Array.Empty<string>()).ToString();

         string expected = "<urlset></urlset>";
         Assert.AreEqual(expected, result);
      }
      
      [TestMethod]
      [DataRow("https://example.com/")]
      [DataRow("https://example.com")]
      public void WriteXMLMap(string domain)
      {
         string result = SiteMapGenerator.WriteXMLMap(domain, new string[] { "cat.html", "/cat2.svg", "other/dog.txt" }).ToString();

         string date = DateTime.UtcNow.ToString("yyyy-MM-dd");

         string expected = "<urlset>"+
                           $"<url><loc>https://example.com/cat.html</loc><lastmod>{date}</lastmod><priority>1</priority><changefreq>monthly</changefreq></url>" +                           
                           $"<url><loc>https://example.com/cat2.svg</loc><lastmod>{date}</lastmod><priority>1</priority><changefreq>monthly</changefreq></url>" +                           
                           $"<url><loc>https://example.com/other/dog.txt</loc><lastmod>{date}</lastmod><priority>1</priority><changefreq>monthly</changefreq></url>" +                           
                           "</urlset>";
         Assert.AreEqual(expected, result);
      }

      [TestMethod]
      public void WriteHTMLMap()
      {
         HtmlDocument doc = SiteMapGenerator.CreateHTMLMap(new [] {( new FilePath("/cat.html"), "catty"), ("/cat2.svg", "catty two"), ("/other/dog.txt", "other dog"), ("/other/dog2.txt", "last") });

         // Result should be:
         // * cat.html
         // * cat2.svg
         // * other (no link)
         //    * dog
         //    * dog2


         AssertHelp.AssertDocument(doc, new Action<HtmlSymbol>[]
         {   
            result => { AssertHelp.AssertUnorderedList(result);
                        AssertHelp.AssertChildren(result, new Action<HtmlSymbol>[]
                        {
                           a=>{AssertHelp.AssertListItem(a); AssertHelp.AssertOnlyContainsLink(a,"catty", "/cat.html"); },
                           a=>{AssertHelp.AssertListItem(a); AssertHelp.AssertOnlyContainsLink(a,"catty two", "/cat2.svg"); },
                           other=>{ AssertHelp.AssertListItem(other);
                                    AssertHelp.AssertChildren(other, new Action<HtmlSymbol>[]
                                    {
                                       child =>
                                          {  AssertHelp.AssertUnorderedList(child);
                                           AssertHelp.AssertChildren(child, new Action<HtmlSymbol>[]
                                           {
                                              dog =>  {AssertHelp.AssertListItem(dog); AssertHelp.AssertOnlyContainsLink(dog,"other dog", "/other/dog.txt"); },
                                              dog2 => {AssertHelp.AssertListItem(dog2); AssertHelp.AssertOnlyContainsLink(dog2,"last", "/other/dog2.txt"); }
                                           });
                                          }

                                    });
                                 }
                        });
            } 
         });

      }
   }
}
