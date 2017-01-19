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
    public class AddressTests
    {
        [Test]
        public void address_hello_world()
        {
            var ueb = new Address();
            Assert.That(ueb, Is.Not.Null, "AddressTests.Address returned null");
        }

        [Test]
        public void address_should_not_be_complete_if_empty()
        {
            Address ueb = new Address();
            Assert.That(ueb, Is.Not.Null);

            var result = ueb.isComplete();
            Assert.That(result, Is.EqualTo(false));
        }
    }
}