using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class GenericPathTests
   {
      [TestMethod]
      public void Equatable()
      {
         GenericPath<int> p1 = new(new int[] { 3, 7, 11 });
         GenericPath<int> p2 = new(new int[] { 3, 7, 11 });

         Assert.IsTrue(p1.Equals(p2));
         Assert.IsTrue(p1.Equals((object)p2));
      }
      
      [TestMethod]
      public void NotEquatable()
      {
         GenericPath<int> p1 = new(new int[] { 3, 7, 11 });
         GenericPath<int> p2 = new(new int[] { 3, 71, 11 });

         Assert.IsFalse(p1.Equals(p2));
         Assert.IsFalse(p1.Equals((object)p2));
         Assert.IsFalse(p1.Equals(3));
      }

      [TestMethod]
      public void GetHashCodeMatches()
      {
         GenericPath<int> p1 = new(new int[] { 3, 7, 11 });
         GenericPath<int> p2 = new(new int[] { 3, 7, 11 });

         Assert.IsTrue(p1.GetHashCode().Equals(p2.GetHashCode()));
      }
      [TestMethod]
      public void GetHashCodeMisMatches()
      {
         Random r = new(98);

         int identicalCount = 0;
         for (int i = 0; i < 1000000; i++)
         {
            GenericPath<long> p1 = new(new long[] { r.NextInt64(), r.NextInt64(), r.NextInt64() });
            GenericPath<long> p2 = new(new long[] { r.NextInt64(), r.NextInt64(), r.NextInt64() });

            if(p1.Parts.SequenceEqual(p2.Parts))
            {
               identicalCount++;
               continue;
            }

            Assert.IsFalse(p1.GetHashCode().Equals(p2.GetHashCode()));
         }

         Assert.IsTrue(identicalCount < 2, "Numerous identical collisions? Design flaw in test");
      }
      
   }
}
