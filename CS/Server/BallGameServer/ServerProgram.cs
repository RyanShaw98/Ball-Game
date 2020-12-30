using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BallGameServer
{
    class ServerProgram
    {
        private const int PORT = 8888;
        public static List<ClientHandler> clients = new List<ClientHandler>();
        private static void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, PORT);
            listener.Start();
            Console.WriteLine("Waiting for connections...");
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                ClientHandler newClient = new ClientHandler(tcpClient);
                new Thread(newClient.Run).Start();
                clients.Add(newClient);
            }
        }

        static void Main(string[] args)
        {
            RunServer();
        }
    }
}
