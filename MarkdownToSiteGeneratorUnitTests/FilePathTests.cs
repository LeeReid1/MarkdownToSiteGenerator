using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class FilePathTests
   {
      [TestMethod]
      public void ImplicitOperator()
      {
         FilePath fp = $"{Path.DirectorySeparatorChar}abc{Path.AltDirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt";

         CollectionAssert.AreEqual(new string[] { "", "abc", "def", "filename.txt" }, fp.Parts.ToArray());
      }

      [TestMethod]
      public void ToStringTest()
      {
         string str = $"{Path.DirectorySeparatorChar}abc{Path.DirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt";
         FilePath fp = new(str);
         Assert.AreEqual(str, fp.ToString());
      }


      [TestMethod]
      public void ToFilenameStringTest_Implicit()
      {
         FilePath fp = "filename.txt";
         Assert.AreEqual("filename.txt", fp.ToString());
         Assert.AreEqual(1, fp.Parts.Count);
      }
      [TestMethod]
      public void ToFilenameStringTest_Explicit()
      {
         string str = "filename.txt";
         FilePath fp = new(str);
         Assert.AreEqual(str, fp.ToString());
         Assert.AreEqual(1, fp.Parts.Count);
      }

      [TestMethod]
      public void EqualsTest()
      {
         FilePath fp = new($"{Path.DirectorySeparatorChar}abc{Path.AltDirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt");
         FilePath fp2 = new($"{Path.DirectorySeparatorChar}abc{Path.AltDirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt");

         Assert.AreEqual(fp2, fp);
      }


      [TestMethod]
      public void ToRelative_SameRoot()
      {
         string root = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}abc{Path.DirectorySeparatorChar}";
         string other = root + $"def{Path.DirectorySeparatorChar}filename.txt";
         FilePath fp = new(root);
         FilePath fp2 = new(other);

         Assert.AreEqual($"def{Path.DirectorySeparatorChar}filename.txt", fp2.ToRelative(fp).ToString());

      }


      [TestMethod]
      public void ToRelative_SameRootButIsFile()
      {
         string root = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}abc";
         string other = root + $"{Path.DirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt";
        

         FilePath fp = new(root);
         FilePath fp2 = new(other);

         Assert.ThrowsException<ArgumentException>(()=>fp2.ToRelative(fp));
      }

      [TestMethod]
      public void ToRelative_RefIsFile()
      {
         string root = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}abc.bob";
         string other = root + $"{Path.DirectorySeparatorChar}def{Path.DirectorySeparatorChar}filename.txt";
        

         FilePath fp = new(root);
         FilePath fp2 = new(other);

         Assert.ThrowsException<ArgumentException>(()=>fp2.ToRelative(fp));
      }

      [TestMethod]
      public void ToRelative_RefIsFile_MismatchedRoot()
      {
         string root = $"/unlikely/path/goes/here/jam.txt";
         string other = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}myfile.txt";

         FilePath fp = new(root);
         FilePath fp2 = new(other);

         Assert.ThrowsException<ArgumentException>(()=>fp2.ToRelative(fp));
      }

      [TestMethod]
      public void PlusOperator_TwoFiles()
      {
         FilePath fp = $"/unlikely/path/goes/here/myfile";
         Assert.ThrowsException<ArgumentException>(()=> fp + "jam.txt");
      }
      [TestMethod]
      public void PlusOperator_Implicit()
      {
         FilePath fp = $"/unlikely/path/goes/here/";
         Assert.IsInstanceOfType(fp + "jam.txt", typeof(FilePath));
         FilePath fp2 = fp + "jam.txt";

         Assert.AreEqual($"{Path.DirectorySeparatorChar}unlikely{Path.DirectorySeparatorChar}path{Path.DirectorySeparatorChar}goes{Path.DirectorySeparatorChar}here{Path.DirectorySeparatorChar}jam.txt", fp2.ToString());
      }
      
      [TestMethod]
      public void PlusOperator_Explicit()
      {
         FilePath fp = $"/unlikely/path/goes/here/";
         Assert.IsInstanceOfType(fp + new FilePath("jam.txt"), typeof(FilePath));
         FilePath fp2 = fp + new FilePath("jam.txt");

         Assert.AreEqual($"{Path.DirectorySeparatorChar}unlikely{Path.DirectorySeparatorChar}path{Path.DirectorySeparatorChar}goes{Path.DirectorySeparatorChar}here{Path.DirectorySeparatorChar}jam.txt", fp2.ToString());
      }
   }
}
