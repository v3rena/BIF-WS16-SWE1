using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    class StaticFilePlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the StaticFilePlugin is willing to handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            float valid = 0;

            if(req.IsValid)
            {
                if (req.Url.Segments[0] == "file")
                {
                    string path = getFilePath(req.Url.Segments[1]);

                    if (File.Exists(path))
                    {
                        valid = 0.5f;
                    }
                    
                }            
            }
            return valid;
        }

        public IResponse Handle(IRequest req)
        {
            /// <summary>
            /// Called by the server when the plugin should handle the request.
            /// Reads a file from a directory and returns the content.
            /// </summary>
            /// <param name="req"></param>
            /// <returns>A new response object containing the file content.</returns>
            Response resp = new Response();            

            if (req.IsValid == true && this.CanHandle(req) > 0)
            {
                resp.StatusCode = 200;

                /*
                string raw;
                string file;
                
                file = req.Url.Segments[1];

                raw = Path.Combine("E:\\FH\\WS2016\\SWE\\SWE1-CS", "deploy");
                raw = Path.Combine(raw, "static-files");
                raw = Path.Combine(raw, file);*/

                string path = getFilePath(req.Url.Segments[1]);

                generateBody(path, req.Url.Segments[1], ref resp);                
            }
            else
            {
                resp.StatusCode = 404;

                string temp = "Plugin fail";
                resp.SetContent(temp);
            }

            resp.AddHeader("Content-Length", resp.ContentLength.ToString());
            resp.AddHeader("Connection", "keep alive");

            return resp;
        }

        public void generateBody(string path, string file,ref Response resp)
        {
            /// <summary>
            /// Sets the Content and Content type according to file ending
            /// </summary>
            /// <param name="path"> Contains the full path to the static file folder </param>
            /// <param name="file"> Contains the file name separated from the path </param>
            /// <param name="resp"> Reference to the response object </param>
            if (file.EndsWith(".html"))
            {
                var fs = File.OpenRead(path);
                resp.SetContent(fs);
                resp.ContentType = "text/html";
            }
            else if (file.EndsWith(".css"))
            {
                var fs = File.OpenRead(path);
                resp.SetContent(fs);
                resp.ContentType = "text/css";
            }
            else if (file.EndsWith(".js"))
            {
                var fs = File.OpenRead(path);
                resp.SetContent(fs);
                resp.ContentType = "text/javascript";
            }
            else if (file.EndsWith(".jpg") || file.EndsWith(".png"))
            {
                string respBody = "<html><body><img src = \"" + path + "\" ></body></html>";
                resp.SetContent(respBody);
                resp.ContentType = "text/html";
            }
            else if(file.EndsWith(".txt"))
            {
                var fs = File.OpenRead(path);
                resp.SetContent(fs);
                resp.ContentType = "text/plain";
            }
        }

        public string getFilePath(string file)
        {
            string raw;

            //raw = Path.Combine("E:\\FH\\WS2016\\SWE\\SWE1-CS", "deploy");
            raw = Path.Combine("C:\\Users\\Verena\\Desktop\\SWE1\\BIF-WS16-SWE1", "deploy");
            raw = Path.Combine(raw, "static-files");
            raw = Path.Combine(raw, file);

            return raw;
        }
    }
}
