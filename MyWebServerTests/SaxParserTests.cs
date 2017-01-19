using MyWebServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServerTests
{
    [TestFixture]
    public class SaxParserTests
    {
        [Test]
        public void saxparser_hello_world()
        {
            var ueb = new SaxParser();
            Assert.That(ueb, Is.Not.Null, "SaxParserTests.SaxParser returned null");
        }

        [Test]
        public void saxparser_should_add_to_addressdictionary()
        {
            NaviDictionary ueb = new NaviDictionary();
            SaxParser.Update("Heygsbreyt", ueb);

            Assert.That(ueb.AddressDictionary, Is.Not.Null);
            Assert.That(ueb.AddressDictionary.Count(), Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void saxparser_should_add_to_streetlistdictionary()
        {
            NaviDictionary ueb = new NaviDictionary();
            SaxParser.Update(ueb);

            Assert.That(ueb.StreetListDictionary, Is.Not.Null);
            Assert.That(ueb.StreetListDictionary.Count(), Is.GreaterThanOrEqualTo(5));
        }
    }
}