using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{

    class TemperaturePlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            float valid = 0;
            if(req.IsValid)
            {
                if(req.Url.Segments[0] == "temperature")
                {
                    valid = 1;
                }
                else if(req.Url.Parameter.ContainsKey("temperature") && req.Url.Parameter["temperature"] == "true")
                {
                    valid = 1;
                }
            }

            return valid;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            Response resp = new Response();            

            if (req.IsValid == true && this.CanHandle(req) > 0)
            {
                connect();

                resp.StatusCode = 200;

                Segments = req.Url.Segments;

                if(req.Url.Segments[1] == "rest")
                {
                    //handle rest call
                    resp.AddHeader("Content-Type", "text/xml");
                    resp.ContentType = "text/xml";

                    createBody(true);
                }
                else
                {
                    //handle regular call
                    resp.AddHeader("Content-Type", "text/html");
                    resp.ContentType = "text/html";

                    createBody(false);
                }
                
                resp.SetContent(RespBody);
                
            }
            else
            {
                resp.StatusCode = 404;

                string temp = "<html><body>Testplugin fail</body></html>";
                resp.ContentType = "text/html";
                resp.SetContent(temp);
            }
            resp.AddHeader("Content-Length", resp.ContentLength.ToString());
            resp.AddHeader("Connection", "keep alive");

            return resp;
        }

        public void connect()
        {
            ///<summary>
            /// Starts the connection to the MS SQL Server.
            /// </summary>
            connection = new SqlConnection("user id=WebServerTemp;" +
                                           "password=1234;server=localhost;" +
                                           "Trusted_Connection=yes;" +
                                           "database=TempData; " +
                                           "connection timeout=30");
            connection.Open();
        }

        public void createBody(bool rest)
        {
            /// <summary>
            /// Generates the body of the response.
            /// </summary>
            /// <param name="rest"> checks if output should be xml formatted </param>
            int counter, from, to;

            int segCount = Segments.Count();

            from = 0;
            to = 10;

            if (rest == false)
            {
                counter = 4;

                if(segCount > 5)
                {
                    if (Int32.TryParse(Segments[4], out from))
                    {
                        to = from + 10;
                    }                    
                }
            }
            else
            {
                counter = 5;
            }

            SqlCommand command = null;
            SqlDataReader myReader = null;

            if (Segments[counter - 3] == "day")
            {
                command = new SqlCommand("Select zeitpunkt, temperatur from Tempdaten Where zeitpunkt like @param1 order by zeitpunkt", connection);
                command.Parameters.AddWithValue("@param1", Segments[counter-2] + "%");
            }
            else if (Segments[counter - 3] == "span")
            {
                command = new SqlCommand("Select zeitpunkt, temperatur from Tempdaten Where zeitpunkt between @param1 and @param2 order by zeitpunkt", connection);
                command.Parameters.AddWithValue("@param1", Segments[counter-2] + " 00:00:00.000");
                command.Parameters.AddWithValue("@param2", Segments[counter-1] + " 00:00:00.000");
            }
            else
            {
                command = new SqlCommand("Select zeitpunkt, temperatur from Tempdaten Order By Zeitpunkt", connection);
            }

            myReader = command.ExecuteReader();

            if(rest == false)
            {
                RespBody = "<html><body>";
                int n = 0;

                while (myReader.Read() && n < to)
                {
                    if(n >= from)
                    {
                        RespBody += "<p>";
                        RespBody += myReader["Zeitpunkt"].ToString() + ": ";
                        RespBody += myReader["Temperatur"].ToString() + " Grad Celsius";
                        RespBody += "</p>";
                    }                    
                    n++;
                }

                if (from >= 10)
                {
                    RespBody += "<form method = \"get\" action = \"localhost:8080/temperature/span/" + Segments[counter - 2] + "/" + Segments[counter - 1] + "/" + (from - 10) + "/" + (to - 10) + "/ \">";
                    RespBody += "<button type = \"submit\">zurueck</button>";
                    RespBody += "</form>";
                }
                if ( n >= to)
                {
                    RespBody += "<form method = \"get\" action = \"localhost:8080/temperature/span/" + Segments[counter - 2] + "/" + Segments[counter - 1] + "/" + (from + 10) + "/" + (to + 10) + "/ \">";
                    RespBody += "<button type = \"submit\">vor</button>";
                    RespBody += "</form>";
                }

                RespBody += "</body></html>";
            }
            else
            {
                RespBody = "<temperatures>\n";
                while (myReader.Read())
                {
                    RespBody += "  <entry>\n";
                    RespBody += "    <time>" + myReader["Zeitpunkt"].ToString() + "</time>\n";
                    RespBody += "    <temp>" + myReader["Temperatur"].ToString() + "</temp>\n";
                    RespBody += "  </entry>\n";
                }
                RespBody += "</temperatures>\n";
            }            
        }

        public SqlConnection connection;
        public string RespBody;
        public string[] Segments;
    }
}
