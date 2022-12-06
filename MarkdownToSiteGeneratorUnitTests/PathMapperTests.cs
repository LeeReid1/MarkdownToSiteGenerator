using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class PathMapperTests
   {
      [TestMethod]
      public void ConstructorStable()
      {
         PathMapper pm = new("/fake/location/for/the/source/", "/fake/location/for/the/destination/");
      }
      
      [TestMethod]
      public void ConstructorWithBadInputs()
      {
         Assert.ThrowsException<ArgumentException>(()=> new PathMapper("/fake/location/for/the/source/a.txt", "/fake/location/for/the/destination/"));
         Assert.ThrowsException<ArgumentException>(()=> new PathMapper("/fake/location/for/the/source", "/fake/location/for/the/destination/"));
         Assert.ThrowsException<ArgumentException>(()=> new PathMapper("/fake/location/for/the/source/", "/fake/location/for/the/destination/a.txt"));
         Assert.ThrowsException<ArgumentException>(()=> new PathMapper("/fake/location/for/the/source/", "/fake/location/for/the/destination"));
      }

      [TestMethod]
      [DataRow("css")]
      [DataRow("less")]
      public void GetDestination_CSS(string suffix)
      {
         string dir_dest_top = "/fake/location/for/the/destination/".Replace('/', Path.DirectorySeparatorChar);
         string dir_dest_css = dir_dest_top + PathMapper.RelativeOutputPath_CSS;
         GetDestinationSub(suffix, dir_dest_top, dir_dest_css);

      }
      [TestMethod]
      [DataRow("js")]
      public void GetDestination_JS(string suffix)
      {
         string dir_dest_top = "/fake/location/for/the/destination/".Replace('/', Path.DirectorySeparatorChar);
         
         string dir_dest_js = dir_dest_top + PathMapper.RelativeOutputPath_JS;
         GetDestinationSub(suffix, dir_dest_top, dir_dest_js);
      }
      
      [TestMethod]
      [DataRow("jpeg")]
      [DataRow("jpg")]
      [DataRow("png")]
      [DataRow("webp")]
      [DataRow("bmp")]
      public void GetDestination_IMG(string suffix)
      {
         string dir_dest_top = "/fake/location/for/the/destination/".Replace('/', Path.DirectorySeparatorChar);
         
         string dir_dest_img = dir_dest_top + PathMapper.RelativeOutputPath_Images;
         GetDestinationSub(suffix, dir_dest_top, dir_dest_img);
      }

      private static void GetDestinationSub(string suffix, string dir_dest_top, string dir_dest_cssEtc)
      {
         string dir_src_top = "/fake/location/for/the/source/".Replace('/', Path.DirectorySeparatorChar);
         Assert.AreEqual(Path.DirectorySeparatorChar, dir_dest_cssEtc[^1]);

         PathMapper pm = new(dir_src_top, dir_dest_top);

         var dest = pm.GetDestination(Path.GetFullPath(dir_src_top) + "bob." + suffix);
         Assert.IsTrue(dest.IsFile);
         Assert.AreEqual(Path.GetFullPath(dir_dest_cssEtc) + "bob." + suffix, dest.ToAbsoluteString());
      }
      

      [TestMethod]
      [DataRow("md")]
      [DataRow("html")]
      [DataRow("htm")]
      public void GetDestination_Content(string suffix)
      {
         string dir_src_top = "/fake/location/for/the/source/".Replace('/', Path.DirectorySeparatorChar);
         string dir_dest_top = "/fake/location/for/the/destination/".Replace('/', Path.DirectorySeparatorChar);

         PathMapper pm = new(dir_src_top, dir_dest_top);
         foreach (string relativePath in new string[] { "somefile", "somefolder/somefile" })
         {
            string loc_src = dir_src_top + relativePath + "." +suffix;
            string loc_dest = Path.GetFullPath(dir_dest_top + relativePath + ".html");

            var dest = pm.GetDestination(loc_src);
            Assert.IsTrue(dest.IsFile);
            Assert.AreEqual(loc_dest, dest.ToAbsoluteString());
         }
      }

   }
}
