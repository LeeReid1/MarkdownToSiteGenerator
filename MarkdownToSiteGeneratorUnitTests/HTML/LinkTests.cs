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
}