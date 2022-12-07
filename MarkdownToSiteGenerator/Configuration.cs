using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGenerator
{
   public class Configuration
   {
      public const string Key_IncludeBootstrap_JS = "include_bootstrap_js";
      public const string Key_IncludeBootstrap_CSS = "include_bootstrap_css";

      public bool IncludeBootstrap_JS { get; set; }
      public bool IncludeBootstrap_CSS { get; set; }

      public static Configuration IniToConfiguration(string? iniContent)
      {
         IConfigurationRoot? cr;
         if (iniContent == null)
         {
            cr = null;
         }
         else
         {
            using MemoryStream sr = new(Encoding.UTF8.GetBytes(iniContent));
            cr = new ConfigurationBuilder().AddIniStream(sr).Build();
         }

         return new Configuration()
         {
            IncludeBootstrap_JS = ParseBool(Key_IncludeBootstrap_JS, true),
            IncludeBootstrap_CSS = ParseBool(Key_IncludeBootstrap_CSS, true)
         };


         bool ParseBool(string key, bool defaultValue) => cr?[key]?.ToLowerInvariant() switch
         {
            null => defaultValue,
            "true" or "1" or "on" or "yes" => true,
            "false" or "0" or "off" or "no" => false,
            _ => throw new Exception($"Badly formatted configuration for setting {key}. Expecting true, false, 1, or 0")
         };
      }
   }
}
