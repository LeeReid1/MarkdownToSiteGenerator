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


         AssertHTML.AssertDocument(doc, new Action<HtmlSymbol>[]
         {   
            result => { AssertHTML.AssertUnorderedList(result);
                        AssertHTML.AssertChildren(result, new Action<HtmlSymbol>[]
                        {
                           a=>{AssertHTML.AssertListItem(a); AssertHTML.AssertOnlyContainsLink(a,"catty", "/cat.html"); },
                           a=>{AssertHTML.AssertListItem(a); AssertHTML.AssertOnlyContainsLink(a,"catty two", "/cat2.svg"); },
                           other=>{ AssertHTML.AssertListItem(other);
                                    AssertHTML.AssertChildren(other, new Action<HtmlSymbol>[]
                                    {
                                          text => AssertHTML.AssertLiteralText(text, "other"),
                                       childList =>
                                          {  AssertHTML.AssertUnorderedList(childList);
                                           AssertHTML.AssertChildren(childList, new Action<HtmlSymbol>[]
                                           {
                                              dog =>  {AssertHTML.AssertListItem(dog); AssertHTML.AssertOnlyContainsLink(dog,"other dog", "/other/dog.txt"); },
                                              dog2 => {AssertHTML.AssertListItem(dog2); AssertHTML.AssertOnlyContainsLink(dog2,"last", "/other/dog2.txt"); }
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
