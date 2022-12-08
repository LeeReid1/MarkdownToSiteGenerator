using Microsoft.Extensions.Configuration;
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
      public const string Key_CreateSiteMaps = "create_sitemaps";
      public const string Key_DestinationDomain = "destination_domain";
      public const string Key_SiteName = "site_name";
      public const string Key_HomePage = "home_page";

      public bool IncludeBootstrap_JS { get; set; }
      public bool IncludeBootstrap_CSS { get; set; }
      public bool CreateSiteMaps { get; set; }
      public string? SiteName { get; set; }
      public string HomePage { get; set; } = "/";

      string? _destinationDomain;
      /// <summary>
      /// Guaranteed null or ending in /
      /// </summary>
      public string? DestinationDomain
      {
         get => _destinationDomain; 
         set
         {
            if(value == null)
            {
               _destinationDomain = null;
            }
            else if(value.EndsWith("/"))
            {
               _destinationDomain = value;
            }
            else
            {
               _destinationDomain = value + "/";
            }
         }
      }

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
            IncludeBootstrap_CSS = ParseBool(Key_IncludeBootstrap_CSS, true),
            CreateSiteMaps = ParseBool(Key_CreateSiteMaps, true),
            DestinationDomain = cr?[Key_DestinationDomain],
            SiteName = cr?[Key_SiteName],
            HomePage = cr?[Key_HomePage] ?? "/"
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
