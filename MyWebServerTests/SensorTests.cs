using NUnit.Framework;
using MyWebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace MyWebServer.Tests
{
    [TestFixture]
    public class SensorTests
    {
        [Test]
        public void sensor_hello_world()
        {
            var ueb = new Sensor();
            Assert.That(ueb, Is.Not.Null, "SensorTests.Sensor returned null");
        }

        [Test]
        public void sensor_should_connect()
        {
            var ueb = new Sensor();
            Assert.That(ueb, Is.Not.Null, "SensorTests.Sensor returned null");

            ueb.connect();
            Assert.That(ueb.connection, Is.Not.Null, "SensorTests.Connection returned null");
        }
    }
}