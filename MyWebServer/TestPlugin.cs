using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class TestPlugin : IPlugin
    {
        public TestPlugin()
        {
        }

        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the testplugin is willing to handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            float valid = 0;

            if(req.IsValid)
            {
                if (req.Url.Segments[0] == "test")
                {
                    valid = 0.4f;
                }
                else if (req.Url.Parameter.ContainsKey("test_plugin") && req.Url.Parameter["test_plugin"] == "true")
                {
                    valid = 0.4f;
                }
                else if(req.Url.Segments[0] == "")
                {
                    valid = 0.4f;
                }        
            }
            else
            {
                valid = 0;
            }

            return valid;
        }

        /// <summary>
        /// Called by the server when the testplugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            Response resp = new Response();
            resp.AddHeader("Content-Type", "text/html");

            if (req.IsValid == true && this.CanHandle(req) > 0)
            {
                resp.StatusCode = 200;

                string temp = "<html><head><link rel =\"icon\" href=\"data:; base64,iVBORw0KGgo =\"></head><body>Testplugin success</body></html>";

                resp.SetContent(temp);
                resp.ContentType = "text/html";
            }
            else
            {
                resp.StatusCode = 404;

                string temp = "<html><body>Testplugin fail</body></html>";
                resp.SetContent(temp);
            }
            resp.AddHeader("Content-Length", resp.ContentLength.ToString());
            resp.AddHeader("Connection", "keep alive");

            return resp;
        }
    }
}
