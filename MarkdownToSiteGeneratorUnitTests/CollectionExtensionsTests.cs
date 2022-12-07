using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class CollectionExtensionsTests
   {
      [TestMethod]
      public void FirstOrNull_Empty()
      {
         Assert.IsNull(Enumerable.Empty<int>().FirstOrNull(a=>a==9));
      }
      
      [TestMethod]
      public void FirstOrNull_None()
      {
         Assert.IsNull(new int[] { 1, 2, 11 }.FirstOrNull(a=>a==9));
      }
      

      [TestMethod]
      public void FirstOrNull_One()
      {
         Assert.AreEqual(9, new int[] { 1, 2, 9, 11 }.FirstOrNull(a=>a%3 == 0));
      }
      [TestMethod]
      public void FirstOrNull_Multiple()
      {
         Assert.AreEqual(9, new int[] { 1, 2, 9, 12, 15 }.FirstOrNull(a => a % 3 == 0));
      }
      
      [TestMethod]
      public void IndexOfAll_Empty()
      {
         Assert.AreEqual(0, Enumerable.Empty<object>().IndexOfAll(a => true).Count());
         Assert.AreEqual(0, Enumerable.Empty<object>().IndexOfAll(a => false).Count());
      }
      
      [TestMethod]
      public void IndexOfAll_None()
      {
         string[] items = { "a", "bobcat", "c", "gorilla", "chimpanzee" };
         Assert.AreEqual(0, items.IndexOfAll(a => a.Contains("y")).Count());
      }
      [TestMethod]
      public void IndexOfAll_Some()
      {
         string[] items = { "a", "bobcat", "c", "gorilla", "chimpanzee" };
         CollectionAssert.AreEqual(new int[] { 1, 2, 4 }, items.IndexOfAll(a => a.Contains("c")).ToArray());
      }

      [TestMethod]
      public void TakeWhileType_Empty()
      {
         string[] items = Enumerable.Empty<object>().TakeWhileType<object, string>().ToArray();
         Assert.AreEqual(0, items.Length);
      }

      [TestMethod]
      public void TakeWhileType_None()
      {
         List<object> list = new()
         {
            1, 2, 3, "cat", "bean", 11, "frank"
         };

         string[] items = list.TakeWhileType<object, string>().ToArray();
         Assert.AreEqual(0, items.Length);
      }

      [TestMethod]
      public void TakeWhileType()
      {
         List<object> list = new()
         {
            1,2,3,"cat","bean",11, "frank"
         };

         string[] items = list.Skip(3).TakeWhileType<object, string>().ToArray();
         Assert.AreEqual(2, items.Length);
         Assert.AreSame(list[3], items[0]);
         Assert.AreSame(list[4], items[1]);
      }


      [TestMethod]
      public void TakeWhileType_End()
      {
         List<object> list = new()
         {
            1,2,3,"cat","bean",11, "frank"
         };

         string[] items = list.Skip(6).TakeWhileType<object, string>().ToArray();
         Assert.AreEqual(1, items.Length);
         Assert.AreSame(list[6], items[0]);
      }

   }
}
