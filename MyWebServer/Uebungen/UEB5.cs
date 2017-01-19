using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using MyWebServer;
using System.IO;

namespace Uebungen
{
    public class UEB5 : IUEB5
    {
        public void HelloWorld()
        {
        }

        public IPluginManager GetPluginManager()
        {
            return new PluginManager();
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public IPlugin GetStaticFilePlugin()
        {
            return new StaticFilePlugin();
        }

        public string GetStaticFileUrl(string fileName)
        {
            string raw = "/file/";
            raw += fileName;

            return raw;
        }

        public void SetStatiFileFolder(string folder)
        {
            return;
        }
    }
}
