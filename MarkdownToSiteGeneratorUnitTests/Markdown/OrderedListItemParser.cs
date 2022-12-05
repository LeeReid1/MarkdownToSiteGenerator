using MarkdownToSiteGenerator;
using MarkdownToSiteGenerator.Markdown;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToSiteGeneratorUnitTests.Markdown
{
    [TestClass]
    public class OrderedListItemParserTests
    {
        OrderedListItemParser? parser;

        [TestInitialize]
        public void Initialise()
        {
            parser = new OrderedListItemParser();
        }

        [TestMethod]
        public void IsMatch()
        {
            Assert.IsNotNull(parser);

            string[] beforeOK = new string[] { "", " ", "  ", "\t", "\t", "\n" };
            string[] beforeInvalid = new string[] { "a", "a ", "a  ", "1 ", "1 \t", "a\t" };

            string[] numbersOK = new string[] { "1.", "7.", "44." };
            string[] numbersInvalid = new string[] { "", "a.", "." };

            string[] aftersOK = new string[] { " ", " hi", " hi", " .", " 123", " I went to the station" };
            string[] aftersInvalid = new string[] { "", "1", "cat", "1 hi", ".", ". 123" };

            Sub(beforeOK, numbersOK, aftersOK, true);
            Sub(beforeInvalid, numbersOK, aftersOK, false);
            Sub(beforeOK, numbersInvalid, aftersOK, false);
            Sub(beforeInvalid, numbersInvalid, aftersOK, false);
            Sub(beforeInvalid, numbersOK, aftersInvalid, false);
            Sub(beforeOK, numbersInvalid, aftersInvalid, false);
            Sub(beforeInvalid, numbersInvalid, aftersInvalid, false);


            void Sub(string[] before, string[] numbers, string[] after, bool expected)
            {
                foreach (var prefix in before)
                {
                    foreach (var num in numbers)
                    {
                        string firstHalf = prefix + num;
                        foreach (var suffix in after)
                        {
                            string content = firstHalf + suffix;
                            Assert.AreEqual(expected, parser.MatchesLine(content), content);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void BasicOneLineParse()
        {
            Assert.IsNotNull(parser);
            string text = "1. this message";
            var parsed = parser.ToSymbolisedText(text).ToArray();

            Assert.AreEqual(1, parsed.Length);
            Assert.AreEqual(1, parsed[0].Children.Count());
            var child = parsed[0].Children.First();
            Assert.IsInstanceOfType(child, typeof(LiteralText));
            var fragments = ((LiteralText)child).GetContentFragments(text).ToArray();
            Assert.AreEqual(1, fragments.Length);
            Assert.AreEqual("this message", string.Concat(fragments));

        }


        [TestMethod]
        public void BasicThreeLineParse()
        {
            Assert.IsNotNull(parser);
            string text =
   @"1. this message
2. is not very long
3. last one";
            var parsed = parser.ToSymbolisedText(text).ToArray();

            Assert.AreEqual(3, parsed.Length);


            Check(text, parsed[0], "this message");
            Check(text, parsed[1], "is not very long");
            Check(text, parsed[2], "last one");



            static void Check(string text, SymbolisedText cur, string expected)
            {
                Assert.AreEqual(1, cur.Children.Count());
                var child = cur.Children.First();
                Assert.IsInstanceOfType(child, typeof(LiteralText));
                var fragments = ((LiteralText)child).GetContentFragments(text).ToArray();
                Assert.AreEqual(1, fragments.Length);
                Assert.AreEqual(expected, string.Concat(fragments));
            }
        }
    }
}
