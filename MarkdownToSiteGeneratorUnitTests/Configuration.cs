using MarkdownToSiteGenerator;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class ConfigurationTests
   {
      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void IniToConfiguration_JS(bool js)
      {
         string content = $"{Configuration.Key_IncludeBootstrap_JS}={js}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(js, config.IncludeBootstrap_JS);
      }
      
      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void IniToConfiguration_CSS(bool val)
      {
         string content = $"{Configuration.Key_IncludeBootstrap_CSS}={val}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(val, config.IncludeBootstrap_CSS);
      }
   }
}
