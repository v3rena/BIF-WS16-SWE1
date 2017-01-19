using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;


namespace MyWebServer
{
    class NaviPlugin : IPlugin
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
                if (req.Url.Segments[0] == "navi")
                {
                    valid = 0.4f;
                }
                else if (req.Url.Parameter.ContainsKey("navi") && req.Url.Parameter["navi"] == "true")
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
        /// Called by the server when the plugin should handle the request.
        /// Enables the user to search for streets and reload the map.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object containing a list of street names or the reloaded street map.</returns>
        public IResponse Handle(IRequest req)
        {
            Response resp = new Response();
            resp.AddHeader("Content-Type", "text/html");

            if (req.IsValid == true && this.CanHandle(req) > 0)
            {
                resp.StatusCode = 200;

                if (req.Method == "GET")
                {
                    string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/navi' method='post'>Strassenname suchen:<br><input type='text' name='street'><br><input type='submit'></form><form action='/navi' method='post'> Strassenkarte neu aufbereiten<br><input type ='submit' name ='reload' value ='Aufbereiten!'/></form ></body></html>";
                    resp.SetContent(body);
                    resp.ContentType = "text/html";
                }

                else if (req.Method == "POST")
                {
                    string[] splitInput = splitFormData(req.ContentString);
                    string streetKey = splitInput[0];
                    string streetValue = splitInput[1];

                    if (String.Equals(streetKey, "street"))
                    {
                        if (String.IsNullOrEmpty(streetValue))
                        {

                            string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/navi' method='post'>Strassenname suchen<br><input type='text' name='street'><br><input type='submit'></form><pre>Bitte geben Sie eine Anfrage ein</pre><form action='/navi' method='post'>Strassenkarte neu aufbereiten<br><input type = 'submit' name = 'reload' value = 'Aufbereiten!'/></form></body></html>";
                            resp.SetContent(body);
                            resp.ContentType = "text/html";
                        }

                        else
                        {
                            //street=Kirkjuvegur, Heygsbreyt
                            NaviDictionary AddressDictionary = new NaviDictionary();
                            SaxParser.Update(streetValue, AddressDictionary);
                            string citiesFound = AddressDictionary.GetCities();

                            if (String.Equals(citiesFound, "\n=>"))
                            {
                                string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/navi' method='post'>Strassenname suchen:<br><input type='text' name='street'><br><input type='submit'></form><pre>Sie suchten nach " + streetValue + ". Es wurden keine Orte gefunden.</pre><form action ='/navi' method ='post'>Strassenkarte neu aufbereiten<br><input type ='submit' name ='reload' value='Aufbereiten!'/></form></body></html>";
                                resp.SetContent(body);
                                resp.ContentType = "text/html";
                            }
                            else
                            {
                                string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/navi' method='post'>Strassenname suchen:<br><input type='text' name='street'><br><input type='submit'></form><pre>Sie suchten nach " + streetValue + ". Orte gefunden. " + citiesFound + "</pre><form action='/navi' method ='post'>Strassenkarte neu aufbereiten<br><input type ='submit' name ='reload' value='Aufbereiten!'/></form></body></html>";
                                resp.SetContent(body);
                                resp.ContentType = "text/html";
                            }
                        }
                    }
                    else if (String.Equals(streetKey, "reload"))
                    {
                        NaviDictionary Dictionary = new NaviDictionary();
                        SaxParser.Update(Dictionary);
                        string body = "<html><head><meta charset ='utf-8'/></head><body><form action='/navi' method='post'>Strassenname suchen:<br><input type='text' name='street'><br><input type='submit'></form><form action = '/navi' method = 'post' > Strassenkarte neu aufbereiten<br><input type ='submit' name ='reload' value ='Aufbereiten!'/><pre>Strassenkarte erfolgreich aufbereitet.</pre></form></body></html>";
                        resp.SetContent(body);
                        resp.ContentType = "text/html";
                    }   
                }
                else
                {
                    resp.StatusCode = 404;

                    string body = "<html><body>NaviPlugin fail</body></html>";
                    resp.SetContent(body);
                }
                resp.AddHeader("Content-Length", resp.ContentLength.ToString());
                resp.AddHeader("Connection", "keep alive"); 
            }
            return resp;
        }

        /// <summary>
        /// Splits key and value of form data.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string[] splitFormData (string input)
        {
            string decodedInput = Uri.UnescapeDataString(input);
            string[] splitInput = input.Split('=');
            return splitInput;
        }
    }
}
