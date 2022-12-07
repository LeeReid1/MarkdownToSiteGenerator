using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class UniqueTreeNodeTests
   {
      [TestMethod]
      public void Constructor()
      {
         UniqueTreeNode<string, object> tree = new("animals");
         Assert.AreEqual("animals", tree.Value);
         Assert.AreEqual(0, tree.Children.Count);
      }

      [TestMethod]
      public void AddValueByPath_EmptyPath()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         Assert.ThrowsException<ArgumentException>(() => tree.AddValueByPath(Array.Empty<string>()));

      }
      
      [TestMethod]
      public void AddValueByPath_ChildNode()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         var leaf = tree.AddValueByPath(new string[] { "dogs" });

         Assert.AreEqual("animals", tree.Value);
         Assert.AreEqual(1, tree.Children.Count);
         Assert.AreEqual("dogs", tree.Children[0].Value);
         Assert.AreEqual(0, tree.Children[0].Children.Count);
         Assert.AreSame(leaf, tree.Children[0]);
      }

      [TestMethod]
      public void AddValueByPath_GrandChildNode()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         var leaf = tree.AddValueByPath(new string[] { "dogs", "husky" });

         Assert.AreEqual("animals", tree.Value);
         Assert.AreEqual(1, tree.Children.Count);
         Assert.AreEqual("dogs", tree.Children[0].Value);
         Assert.AreEqual(1, tree.Children[0].Children.Count);
         Assert.AreEqual("husky", tree.Children[0].Children[0].Value);
         Assert.AreEqual(0, tree.Children[0].Children[0].Children.Count);
         Assert.AreSame(leaf, tree.Children[0].Children[0]);
      }

      [TestMethod]
      public void AddValueByPath_DuplicateTopNode()
      {
         UniqueTreeNode<string, object> tree = new("animals");
         Assert.ThrowsException<ArgumentException>(() => tree.AddValueByPath(Array.Empty<string>()));
      }
      [TestMethod]
      public void AddValueByPath_DuplicateGrandChildNode()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         tree.AddValueByPath(new string[] { "dogs", "husky" });
         Assert.ThrowsException<ArgumentException>(() => tree.AddValueByPath(new string[] { "dogs", "husky" }));
      }
      
      

      [TestMethod]
      public void AsEnumerable()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         List<string>[] toAdd = new List<string>[]
         {
            new(){ "dogs", "husky" },
            new() { "dogs", "dalmation" },
            new() { "cats", "burmese" },
            new() { "mouse" }
         };

         foreach (var item in toAdd)
         {
            tree.AddValueByPath(item);
         }
         GenericPath<string>[] paths = tree.ToArray();
         Assert.AreEqual(4, paths.Length);
         foreach (var path in toAdd)
         {
            Assert.AreEqual(1, paths.Count(a => a.Parts.SequenceEqual(path.Prepend("animals"))));
         }
      }


      [TestMethod]
      public void AsEnumerable_OneNodeTree()
      {
         UniqueTreeNode<string, object> tree = new("animals");

         GenericPath<string>[] paths = tree.ToArray();
         Assert.AreEqual(1, paths.Length);
         Assert.AreEqual(1, paths[0].Parts.Count);
         Assert.AreEqual("animals", paths[0].Parts[0]);
      }
   }
}
