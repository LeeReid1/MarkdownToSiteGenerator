using MarkdownToSiteGenerator;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class TitleToURLLookupTests
   {
      [TestMethod]
      public void ConstructorStability()
      {
         Mock<IPathToURLMapper<int>> m = new Mock<IPathToURLMapper<int>>(MockBehavior.Strict);

         TitleToURLLookup<int> lookup = new TitleToURLLookup<int>(m.Object);
      }

      [TestMethod]
      public void Add()
      {
         Mock<IPathToURLMapper<int>> m = new Mock<IPathToURLMapper<int>>(MockBehavior.Strict);
         m.Setup(a => a.GetURLLocation(It.IsAny<int>())).Returns<int>(GetURL);

         TitleToURLLookup<int> lookup = new(m.Object);

         lookup.Add("cat", 1);
         lookup.Add("dog", 2);
         lookup.Add("puffin", 3);

         Assert.AreEqual(GetURL(3), lookup.GetURL("puffin"));
         Assert.AreEqual(GetURL(1), lookup.GetURL("cat"));
         Assert.AreEqual(GetURL(2), lookup.GetURL("dog"));

         static string GetURL(int id) => $"https://example.com/{id}";
      }

      [TestMethod]
      public void Add_DuplicateTitle()
      {
         Mock<IPathToURLMapper<int>> m = new Mock<IPathToURLMapper<int>>(MockBehavior.Strict);
         m.Setup(a => a.GetURLLocation(It.IsAny<int>())).Returns<int>(GetURL);

         TitleToURLLookup<int> lookup = new(m.Object);

         lookup.Add("cat", 13);
         Assert.ThrowsException<Exception>(()=>lookup.Add("cat", 13)); // same key and title
         Assert.ThrowsException<Exception>(()=>lookup.Add("cat", 91)); // same title only

         // Does it still function after an exception?
         Assert.AreEqual(GetURL(13), lookup.GetURL("cat"));


         static string GetURL(int id) => $"https://example.com/{id}";
      }

      [TestMethod]
      public void Add_DuplicateLocation()
      {
         Mock<IPathToURLMapper<int>> m = new Mock<IPathToURLMapper<int>>(MockBehavior.Strict);
         m.Setup(a => a.GetURLLocation(It.IsAny<int>())).Returns<int>(GetURL);

         TitleToURLLookup<int> lookup = new(m.Object);

         //more than one title per resource is legal, though probably shouldn't be used
         lookup.Add("cat", 13);
         lookup.Add("dog", 13);

         Assert.AreEqual(GetURL(13), lookup.GetURL("cat"));
         Assert.AreEqual(GetURL(13), lookup.GetURL("dog"));


         static string GetURL(int id) => $"https://example.com/{id}";
      }


      [TestMethod]
      [DataRow("http://wikipedia.org")]
      [DataRow("https://wikipedia.org")]
      [DataRow("www.wikipedia.org")]
      public void GetURL_Explicit(string url)
      {
         Mock<IPathToURLMapper<int>> m = new Mock<IPathToURLMapper<int>>(MockBehavior.Strict);
         m.Setup(a => a.GetURLLocation(It.IsAny<int>())).Returns<int>(GetURL);

         TitleToURLLookup<int> lookup = new(m.Object);

         lookup.Add("cat", 13);

         Assert.AreEqual(url, lookup.GetURL(url));

         static string GetURL(int id) => $"https://example.com/{id}";
      }

   }
}
