using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    class PluginLoader
    {
        public const string PLUGINSFILE = "C:\\Users\\Verena\\Desktop\\SWE1\\BIF-WS16-SWE1\\plugins.cfg";
        //public const string PLUGINSFILE = "E:\\FH\\WS2016\\SWE\\SWE1-CS\\plugins.cfg";
        private PluginManager PluginMaster;
        private FileSystemWatcher Watcher = new FileSystemWatcher();

        public PluginLoader(PluginManager manager)
        {
            PluginMaster = manager;
            EnableWatcher();
            LoadPlugins();
        }

        /// <summary>
        /// Reads plugins from file and adds them to the list Plugins.
        /// </summary>
        private void LoadPlugins()
        {
            string line;
            try
            {
                using (StreamReader reader = new StreamReader(PLUGINSFILE))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith("//"))
                        {
                            line = line.Remove(2);
                            IPlugin p = PluginMaster.GetPluginByType(Type.GetType(line));
                            PluginMaster.RemovePlugin(p);
                        }
                        else
                        {
                            PluginMaster.Add(line);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Pluginsfile was not found.");
                Console.ReadKey();
                throw;
            }
        }

        /// <summary>
        /// Checks whether pluginsfile was changed.
        /// </summary>
        private void EnableWatcher()
        {
            Watcher.Path = Path.GetDirectoryName(PLUGINSFILE);

            Watcher.NotifyFilter = NotifyFilters.LastWrite;
            Watcher.Filter = Path.GetFileName(PLUGINSFILE);
            Watcher.Changed += new FileSystemEventHandler(OnChanged);

            Watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Checks if changes could be applied.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                LoadPlugins();
                Console.WriteLine("Applied changes!");
            }
            catch (Exception ex)
            {
                if (!(ex is FileNotFoundException) && !(ex is ArgumentException))
                    throw ex;
                Console.WriteLine("Could not apply changes");
            }
        }
    }
}
