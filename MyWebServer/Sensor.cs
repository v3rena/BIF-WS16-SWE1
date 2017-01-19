using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MyWebServer
{
    public class Sensor
    {
        public Sensor()
        {
            connect();
        }

        public void generateData()
        {
            /// <summary>
            /// Generates random temperature data using the current time and an int between -10 and 35
            /// </summary>


            Random randomizer = new Random();
            int zahl;

            SqlCommand command = null;
            

            while (true)
            {
                zahl = randomizer.Next(-10, 35);
                DateTime zeit = DateTime.Now;
                var sqlFormattedDate = zeit.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
                command = new SqlCommand("insert into TempDaten (Zeitpunkt, Temperatur) values (@param1, @param2)", connection);
                command.Parameters.AddWithValue("@param1", sqlFormattedDate);
                command.Parameters.AddWithValue("@param2", zahl.ToString());

                command.ExecuteNonQuery();

                System.Threading.Thread.Sleep(10000);
            }
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

        public SqlConnection connection;
    }
}
