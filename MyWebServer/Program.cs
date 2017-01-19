using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace MyWebServer
{
    class Program
    {
        /// <summary>
        /// Starts server, calls ClientHandle and starts the thread that produces temperature data.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            PluginManager PluginManager = new PluginManager();
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            Sensor dataInput = new Sensor();
            Thread myThread = new Thread(new ThreadStart(dataInput.generateData));
            myThread.Start();

            Console.WriteLine("Webserver started");

            

            ClientHandle.HandleClients(PluginManager, listener);         
        }
    }
}
