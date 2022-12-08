using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class SimpleRangeTests
   {
      [TestMethod]
      public void SplitExclude_None()
      {
         SimpleRange r= new(11,17);
         var actual = r.SplitExclude(Enumerable.Empty<SimpleRange>()).ToArray();

         Assert.AreEqual(1, actual.Length);
         Assert.AreEqual(r, actual[0]);
      }

      [TestMethod]
      public void SplitExclude_Full()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         { r  };

         SplitExcludeCheck(r, splitWith, Array.Empty<SimpleRange>());
      }
      [TestMethod]
      public void SplitExclude_AtStart()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(11,27),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(27, 100),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }
      
      [TestMethod]
      public void SplitExclude_BeforeStart()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(-13,27),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(27, 100),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }
      [TestMethod]
      public void SplitExclude_AtEnd()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(31,100),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(11, 31),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }
      
      [TestMethod]
      public void SplitExclude_PastEnd()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(31,190),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(11, 31),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }      
      [TestMethod]
      public void SplitExclude_StartPastEnd()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(131,190),
         };
         SimpleRange[] expected = new[]
         {
            r
         };
         SplitExcludeCheck(r, splitWith, expected);
      }

      [TestMethod]
      public void SplitExclude_Multiple_NoOverlaps()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(13,37),
            new SimpleRange(37,39),
            new SimpleRange(41,97),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(11, 13),
            new SimpleRange(39, 41),
            new SimpleRange(97, 100),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }
      [TestMethod]
      public void SplitExclude_Multiple_Overlaps()
      {
         SimpleRange r = new(11, 100);
         SimpleRange[] splitWith = new[]
         {
            new SimpleRange(13,37),
            new SimpleRange(14,40),
            new SimpleRange(36,39),
            new SimpleRange(43,97),
         };
         SimpleRange[] expected = new[]
         {
            new SimpleRange(11, 13),
            new SimpleRange(40, 43),
            new SimpleRange(97, 100),
         };
         SplitExcludeCheck(r, splitWith, expected);
      }

      private static void SplitExcludeCheck(SimpleRange r, SimpleRange[] splitWith, SimpleRange[] expected)
      {
         var actual = r.SplitExclude(splitWith).ToArray();
         CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Equals_True()
      {
         SimpleRange r = new(11, 17);
         SimpleRange r2 = new(11, 17);

         Assert.IsTrue(r.Equals(r2));
         Assert.AreEqual(r, r2);//sanity check for other unit tests
      }
   }
}
