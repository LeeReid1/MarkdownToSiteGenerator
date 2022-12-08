using MarkdownToSiteGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests
{
   [TestClass]
   public class SymbolisedDocumentTests
   {

      [TestMethod]
      public void ConstructorStability()
      {
         string source =
@"title: cats

# My Cat page

Ok, not much to say, really. But these animals are certainly popular.
".ReplaceLineEndings();
         SymbolisedDocument sd = new(source);
      }


      [TestMethod]
      public void TryGetH1Text_Empty()
      {
         SymbolisedDocument sd = new("");
         Assert.AreEqual(null, sd.TryGetH1Text());
      }

      [TestMethod]
      public void TryGetTitle_Empty()
      {
         SymbolisedDocument sd = new("");
         Assert.AreEqual(null, sd.TryGetTitle());
      }

      [TestMethod]
      public void TryGetMetadata_Empty()
      {
         SymbolisedDocument sd = new("");
         Assert.AreEqual(null, sd.TryGetMetadata("notThere"));
      }

      [TestMethod]
      public void TryGetH1Text()
      {
         string source =
@"title: cats

# My Cat page!

# Another title

Ok, not much to say, really. But these animals are certainly popular.
".ReplaceLineEndings();
         SymbolisedDocument sd = new(source);

         // manually parse the text
         int start = source.IndexOf('#') +2;
         sd.Items.Add(CreateH1("My Cat page!", start));


         start = source.IndexOf('#', start);
         sd.Items.Add(CreateH1("Another title", start));
         
         // Test
         Assert.AreEqual("My Cat page!", sd.TryGetH1Text());
      }

      [TestMethod]
      [DataRow(false)]
      [DataRow(true)]
      public void TryGetTitle_FromMetadata(bool includeH1)
      {
         string source =
@"title: cats

# My Cat page

# Another title

Ok, not much to say, really. But these animals are certainly popular.
".ReplaceLineEndings();
         SymbolisedDocument sd = new(source);

         // manually parse the text
         if (includeH1)
         {
            int start = source.IndexOf('#') + 2;
            sd.Items.Add(CreateH1("My Cat page", start));


            start = source.IndexOf('#', start);
            sd.Items.Add(CreateH1("Another title", start));
         }

         var sl = new SymbolLocation(new SimpleRange(0, 0), new SimpleRange(0, 11), new SimpleRange(11, 11 + Environment.NewLine.Length));
         sd.Items.Add(new Metadata(sl));


         // Test
         Assert.AreEqual("cats", sd.TryGetTitle());
      }

      [TestMethod]
      public void TryGetTitle_FromH1()
      {
         string source =
@"# My Cat page

# Another title

Ok, not much to say, really. But these animals are certainly popular.
".ReplaceLineEndings();
         SymbolisedDocument sd = new(source);

         // manually parse the text
         int start = source.IndexOf('#') + 2;
         sd.Items.Add(CreateH1("My Cat page", start));


         start = source.IndexOf('#', start);
         sd.Items.Add(CreateH1("Another title", start));

         
         // Test
         Assert.AreEqual("My Cat page", sd.TryGetTitle());
      }

      [TestMethod]
      public void TryGetMetadata()
      {
         string source =
$@"title: cats
spell: jingle berries

# My Cat page

Ok, not much to say, really. But these animals are certainly popular.

# Another title

".ReplaceLineEndings();
         SymbolisedDocument sd = new(source);

         // manually parse the text
         int start = source.IndexOf('#') + 2;
         sd.Items.Add(CreateH1("My Cat page", start));


         start = source.IndexOf('#', start);
         sd.Items.Add(CreateH1("Another title", start));

         CreateMetadata(sd, "title: cats", 0);
         CreateMetadata(sd, "spell: jingle berries", "title: cats".Length + Environment.NewLine.Length);

         // Test
         Assert.AreEqual("cats", sd.TryGetMetadata("title"));
         Assert.AreEqual("jingle berries", sd.TryGetMetadata("spell"));
      }

      static Heading CreateH1(string text, int from)
      {
         int to = from + text.Length;
         var sl = new SymbolLocation(new SimpleRange(from, from), new SimpleRange(from, to), new SimpleRange(to, to + Environment.NewLine.Length));
         Heading h1 = new(sl, 1);
         LiteralText lt = new(new SimpleRange(from, to));
         h1.Items.Add(lt);
         return h1;
      }

      static void CreateMetadata(SymbolisedDocument sd, string phrase, int offset)
      {
         var sl = new SymbolLocation(new SimpleRange(offset, offset), new SimpleRange(offset, offset+ phrase.Length), new SimpleRange(offset + phrase.Length, offset + phrase.Length + Environment.NewLine.Length));
         sd.Items.Add(new Metadata(sl));
      }
   }
}
