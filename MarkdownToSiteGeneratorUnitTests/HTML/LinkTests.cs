using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.HTML;
using System.Text;

namespace MarkdownToSiteGeneratorUnitTests.HTML
{
   [TestClass]
   public class LinkTests
   {
      [TestMethod]
      public void ToHTML()
      {
         MarkdownToSiteGenerator.HTML.Link l = new()
         {
            CSSClass = "special blue",
            HRef = "https://example.com"
         };
         l.Add(new MarkdownToSiteGenerator.HTML.LiteralText("hello!", new SimpleRange(0,6)));


         Assert.AreEqual($"<a href=\"https://example.com\" class=\"special blue\">hello!</a>", l.Write(new StringBuilder()).ToString());
      }
   }
   
   [TestClass]
   public class ImageTests
   {
      [TestMethod]
      public void ToHTML()
      {
         MarkdownToSiteGenerator.HTML.Image l = new("/images/shoes.jpg","my shoes!")
         {
            CSSClass = "special blue",
         };

         Assert.AreEqual($"<img src=\"/images/shoes.jpg\" alt=\"my shoes!\" class=\"special blue\" />", l.Write(new StringBuilder()).ToString());
      }
   }
}