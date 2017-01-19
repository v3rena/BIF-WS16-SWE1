using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;


namespace MyWebServer
{
    class LowerPlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            float valid = 0;

            if (req.IsValid)
            {
                if (req.Url.Segments[0] == "tolower")
                {
                    valid = 0.4f;
                }
                else if (req.Url.Parameter.ContainsKey("tolower") && req.Url.Parameter["tolower"] == "true")
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
        /// Called by the server when the LowerPlugin should handle the request.
        /// Receives text input via POST and return the lowered text.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object containing an html body.</returns>
        public IResponse Handle(IRequest req)
        {
            Response resp = new Response();
            resp.AddHeader("Content-Type", "text/html");

            if (req.IsValid == true && this.CanHandle(req) > 0)
            {
                resp.StatusCode = 200;

                if (req.Method == "GET")
                {
                    string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/tolower' method='post' accept-charset='utf-8'>Type some text:<br><input type='text' name='textfield'><br><input type='submit'></form></body></html>";
                    resp.SetContent(body);
                    resp.ContentType = "text/html";
                }

                else if (req.Method == "POST")
                {
                    string input = Uri.UnescapeDataString(req.ContentString);
                    string[] splitInput = input.Split('=');
                    string loweredInput = splitInput[1].ToLower();

                    if (String.IsNullOrWhiteSpace(loweredInput))
                    {
                        string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/tolower' method='post' accept-charset='utf-8'>Type some text:<br><input type='text' name='textfield'><br><input type='submit'></form><pre>Bitte geben Sie einen Text ein<pre></body></html>";
                        resp.SetContent(body);
                    }
                    else
                    {
                        string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/tolower' method='post' accept-charset='utf-8'>Type some text:<br><input type='text' name='textfield'><br><input type='submit'></form><pre>" + loweredInput + "<pre></body></html>";
                        resp.SetContent(body);
                    }
                    
                    resp.ContentType = "text/html";
                }
            }

            else
            {
                resp.StatusCode = 404;

                string body = "<html><head><meta charset ='utf-8'/></head><body>LowerPlugin fail</body></html>";
                resp.SetContent(body);
            }
            resp.AddHeader("Content-Length", resp.ContentLength.ToString());
            resp.AddHeader("Connection", "keep alive");

            return resp;
        }
    }
}
