using NUnit.Framework;
using MyWebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using BIF.SWE1.Interfaces;


namespace MyWebServerTests
{
    [TestFixture]
    public class PluginManagerTests
    {
        [Test]
        public void pluginmanager_should_load_plugins()
        {
            var obj = new PluginManager();
            Assert.That(obj, Is.Not.Null, "PluginManagerTests.GetPluginManager returned null");
            Assert.That(obj.Plugins, Is.Not.Null);
            Assert.That(obj.Plugins.Count(), Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public void pluginmanager_should_add_plugin_by_string()
        {
            var obj = new PluginManager();
            Assert.That(obj, Is.Not.Null, "PluginManager returned null");
            Assert.That(obj.Plugins, Is.Not.Null);
            int count = obj.Plugins.Count();

            obj.Add("TestPlugin");
            Assert.That(obj.Plugins.Count(), Is.EqualTo(count + 1));
            bool found = false;
            foreach (var plugin in obj.Plugins)
            {
                if (plugin is TestPlugin) found = true;
            }
            Assert.That(found, Is.True, "New plugin was not found.");
        }

        [Test]
        public void pluginmanager_should_remove_plugin()
        {
            PluginManager ueb = new PluginManager();
            int count = ueb.Plugins.Count();
            var plugin = ueb.Plugins.First();
            Type plugintype = plugin.GetType();

            ueb.RemovePlugin(plugin);
            Assert.That(ueb.Plugins.Count(), Is.EqualTo(count - 1));
            bool found = false;
            foreach (var p in ueb.Plugins)
            {
                if (p.GetType().Equals(plugintype)) found = true;
            }
            Assert.That(found, Is.False, "Plugin not removed.");
        }

        [Test]
        public void pluginmanager_should_remove_all_plugins()
        {
            PluginManager ueb = new PluginManager();
            int count = ueb.Plugins.Count();

            ueb.Clear();
            Assert.That(ueb.Plugins.Count(), Is.EqualTo(0));
            bool found = false;
            foreach (var plugin in ueb.Plugins)
            {
                if (plugin is IPlugin) found = true;
            }
            Assert.That(found, Is.False, "Not all Plugins removed.");
        }

        [Test]
        public void pluginmanager_should_get_plugin_by_type()
        {
            var ueb = new PluginManager();
            Assert.That(ueb, Is.Not.Null, "PluginManagerTests.GetPluginManager returned null");
            Assert.That(ueb.Plugins, Is.Not.Null);

            IPlugin test = new TestPlugin();
            IPlugin obj = ueb.GetPluginByType(Type.GetType("MyWebServer.TestPlugin,MyWebServer"));
            var equals = (obj.GetType() == test.GetType());
            Assert.That(equals, Is.True);
        }
    }
}
