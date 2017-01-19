using MyWebServer;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MyWebServerTests
{
    [TestFixture]
    public class NaviDictionaryTests
    {
        [Test]
        public void navidictionary_hello_world()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null, "NaviDictionaryTests.NaviDictionary returned null");
        }

        [Test]
        public void addressdictionary_should_create_empty()
        {
            var ueb = new NaviDictionary();

            Assert.That(ueb, Is.Not.Null);
            Assert.That(ueb.AddressDictionary, Is.Not.Null);
            Assert.That(ueb.AddressDictionary.Count(), Is.EqualTo(0));
        }

        [Test]
        public void streetlistdictionary_should_create_empty()
        {
            var ueb = new NaviDictionary();

            Assert.That(ueb, Is.Not.Null);
            Assert.That(ueb.StreetListDictionary, Is.Not.Null);
            Assert.That(ueb.StreetListDictionary.Count(), Is.EqualTo(0));
        }

        [Test]
        public void addressdictionary_should_add_entry()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null);

            ueb.AddressDictionary.Add("foo", "bar");
            Assert.That(ueb.AddressDictionary, Is.Not.Null);
            Assert.That(ueb.AddressDictionary.Count(), Is.EqualTo(1));
            Assert.That(ueb.AddressDictionary["foo"], Is.EqualTo("bar"));
        }

        [Test]
        public void addressdictionary_should_return_string()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null);

            ueb.AddressDictionary.Add("foo", "bar");
            Assert.That(ueb.AddressDictionary, Is.Not.Null);

            var result = ueb.GetCities();
            Assert.That(result.Equals("\n=>\nfoo"));
        }

        [Test]
        public void streetlistdictionary_should_add_new_entry()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null);

            ueb.AddItem("foo", "bar");
            Assert.That(ueb.StreetListDictionary, Is.Not.Null);

            Assert.That(ueb.StreetListDictionary.ContainsKey("foo"), Is.True);
            List<string> list = ueb.StreetListDictionary["foo"];
            Assert.That(list.Contains("bar"), Is.True);
        }

        [Test]
        public void streetlistdictionary_should_add_to_existing_entry()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null);

            ueb.AddItem("foo", "bar");
            ueb.AddItem("foo", "car");
            Assert.That(ueb.StreetListDictionary, Is.Not.Null);

            Assert.That(ueb.StreetListDictionary.Count(), Is.EqualTo(1));
            Assert.That(ueb.StreetListDictionary.ContainsKey("foo"), Is.True);

            List<string> list = ueb.StreetListDictionary["foo"];
            Assert.That(list.Contains("bar"), Is.True);
            Assert.That(list.Contains("car"), Is.True);
        }

        [Test]
        public void streetlistdictionary_should_not_add_duplicate()
        {
            var ueb = new NaviDictionary();
            Assert.That(ueb, Is.Not.Null);

            ueb.AddItem("foo", "bar");
            ueb.AddItem("foo", "bar");
            Assert.That(ueb.StreetListDictionary, Is.Not.Null);

            Assert.That(ueb.StreetListDictionary.Count(), Is.EqualTo(1));
            Assert.That(ueb.StreetListDictionary.ContainsKey("foo"), Is.True);

            List<string> list = ueb.StreetListDictionary["foo"];
            Assert.That(list.Contains("bar"), Is.True);
            Assert.That(list.Count(), Is.EqualTo(1));
        }
    }
}