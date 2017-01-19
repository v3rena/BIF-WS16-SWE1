using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class PluginManager : IPluginManager
    {
        private readonly PluginLoader Loader;

        public PluginManager()
        {
            Loader = new PluginLoader(this);
        }

        /// <summary>
        /// Returns a list of all plugins. Never returns null.
        /// </summary>
        public IEnumerable<IPlugin> Plugins => PluginsList;
        private List<IPlugin> PluginsList { get; } = new List<IPlugin>();

        /// <summary>
        /// Adds a new plugin by type name. If the plugin was already added, nothing will happen.
        /// Throws an exeption, when the type cannot be resolved or the type does not implement IPlugin.
        /// </summary>
        /// <param name="plugin"></param>
        public void Add(string plugin)
        {
            Type pluginType;
            pluginType = !plugin.Contains(".") ?
                Type.GetType("MyWebServer." + plugin) :
                pluginType = Type.GetType(plugin);

            if (pluginType == null)
                throw new ArgumentException("String does not match any Class name!");
            var pluginToAdd = Activator.CreateInstance(pluginType);
            if (!(pluginToAdd is IPlugin))
            {
                throw new ArgumentException("String does not match any IPlugin name!");
            }
            Add((IPlugin)pluginToAdd);
        }

        /// <summary>
        /// Adds a new plugin. If the plugin was already added, nothing will happen.
        /// </summary>
        /// <param name="plugin"></param>
        public void Add(IPlugin plugin)
        {
            PluginsList.Add(plugin);
        }

        /// <summary>
        /// Clears all plugins
        /// </summary>
        public void Clear()
        {
            PluginsList.Clear();
        }

        /// <summary>
        /// Returns the plugin the client requested.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>The requested Plugin. If no plugin is specified, TestPlugin is returned</returns>
        public IPlugin GetRequestedPlugin(IRequest req)
        {
            IPlugin plugin = new TestPlugin();
            float max = 0;
            foreach (var p in Plugins)
            {
                float canHandle = p.CanHandle(req);
                if (canHandle > max)
                {
                    max = canHandle;
                    plugin = p;
                }
            }
            return plugin;
        }

        /// <summary>
        /// Removes a specific plugin from Plugins.
        /// </summary>
        /// <param name="plugin"></param>
        public void RemovePlugin(IPlugin plugin)
        {
            if (PluginsList.Remove(plugin))
            {
                Console.WriteLine("Disabled " + plugin.GetType());
            }
        }

        public IPlugin GetPluginByType(Type type)
        {
            foreach (var p in Plugins)
            {
                if (p.GetType().Equals(type))
                {
                    return p;
                }
            }
            return null;
        }
    }

}
