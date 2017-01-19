using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class ClientHandle
    {
        public static void HandleClients(PluginManager PluginManager, TcpListener listener)
        {
            Console.WriteLine("Ready for clients to connect");

            while (true)
            {
                ///beginAcceptTcpCLient in methode auslagern
                Socket client = listener.AcceptSocket();
                Console.WriteLine("Client connected");
                NetworkStream networkStream = new NetworkStream(client);

                ThreadPool.QueueUserWorkItem(_ => ThreadHandler(networkStream, PluginManager));
                Thread.Sleep(1000);
                //Thread ctThread = new Thread(_ => ThreadHandler(networkStream, PluginManager));
                //ctThread.Start();
            }
        }

        /// <summary>
        /// Handles thread for every client.
        /// </summary>
        /// <param name="networkStream"></param>
        /// <param name="PluginManager"></param>
        public static void ThreadHandler(NetworkStream networkStream, PluginManager PluginManager)
        {
            while (true)
            {
                Console.WriteLine("Waiting for request... ");
                Request request = new Request(networkStream);
                Console.WriteLine("Request received... ");

                IPlugin plugin = PluginManager.GetRequestedPlugin(request);

                Response response = (Response)plugin.Handle(request);
                response.Send(networkStream);
                Console.WriteLine("Response sent.\n");
            }
        }
    }
}
