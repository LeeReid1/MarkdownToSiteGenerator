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
      public void IniToConfiguration_Empty()
      {
         string content = $"";
         Configuration config = Configuration.IniToConfiguration(content);
         Assert.IsNotNull(config);

         // Check stability and sensible defaults
         Assert.IsNull(config.SiteName);
         Assert.IsTrue(config.CreateSiteMaps);
         Assert.IsTrue(config.IncludeBootstrap_CSS);
         Assert.IsTrue(config.IncludeBootstrap_JS);
         Assert.IsNull(config.DestinationDomain);
         Assert.AreEqual("/", config.HomePage);
      }
      
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
      
      [TestMethod]
      [DataRow(true)]
      [DataRow(false)]
      public void IniToConfiguration_SiteMaps(bool val)
      {
         string content = $"{Configuration.Key_CreateSiteMaps}={val}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(val, config.CreateSiteMaps);
      }
      
      
      [TestMethod]
      [DataRow("legendary site")]
      [DataRow("")]
      public void IniToConfiguration_SiteName(string val)
      {
         string content = $"{Configuration.Key_SiteName}={val}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(val, config.SiteName);
      }

      
      [TestMethod]
      [DataRow("home page")]
      public void IniToConfiguration_HomePage(string val)
      {
         string content = $"{Configuration.Key_HomePage}={val}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(val, config.HomePage);
      }

      

      [TestMethod]
      [DataRow("https://example.com", "https://example.com/")]
      [DataRow("https://example.com/", "https://example.com/")]
      public void IniToConfiguration_DestinationDomain(string setting, string expected)
      {
         string content = $"{Configuration.Key_DestinationDomain}={setting}";
         Configuration config = Configuration.IniToConfiguration(content);
         
         Assert.AreEqual(expected, config.DestinationDomain);
      }
   }
}
