using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace Uebungen
{
    public class UEB6 : IUEB6
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

        public string GetNaviUrl()
        {
            return "/navi";
        }

        public IPlugin GetNavigationPlugin()
        {
            return new NaviPlugin();
        }

        public IPlugin GetTemperaturePlugin()
        {
            return new TemperaturePlugin();
        }

        public string GetTemperatureRestUrl(DateTime from, DateTime until)
        {
            string url = "/temperature/rest/";
            url += from.ToString();
            url += "/";
            url += until.ToString();

            return url;
        }

        public string GetTemperatureUrl(DateTime from, DateTime until)
        {
            string url = "/temperature/span/";
            url += from.Year + "-" + from.Month + "-" + from.Day;
            url += "/";
            url += until.Year + "-" + until.Month + "-" + until.Day;
            url += "/";

            return url;
        }

        public IPlugin GetToLowerPlugin()
        {
            return new LowerPlugin();
        }

        public string GetToLowerUrl()
        {
            return "/tolower";
        }
    }
}
