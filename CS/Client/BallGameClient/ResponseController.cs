using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BallGameClient
{
    class ResponseController
    {
        public static bool serverHasRestarted;
        private Client client;

        public ResponseController(Client client)
        {
            this.client = client;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string serverResponse = client.reader.ReadLine();
                    if (serverResponse != null)
                    {
                        string[] serverResponseArr = serverResponse.Split(':');
                        bool responseIsBroadcast = serverResponseArr[1].Equals("broadcast");
                        string response = serverResponseArr[0];
                        if (responseIsBroadcast)
                        {
                            client.broadcastQueue.Add(response);
                        }
                        else
                        {
                            client.responseQueue.Add(response);
                        }
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Server Killed");
                    if (Client.ballHolder == client.GetPlayerId())
                    {
                        // Start new server instance
                        Process newServerProcess = new Process();
                        newServerProcess.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\lib\\BallGameServer.exe";
                        newServerProcess.StartInfo.CreateNoWindow = false;
                        newServerProcess.Start();
                        Console.WriteLine("New server instance created");
                        Thread.Sleep(2000);
                    }
                    client.SetUpConnections();
                    Console.WriteLine("Joined new server instance");
                    serverHasRestarted = true;
                    Console.WriteLine("Server restarted");
                }
            }
        }
    }
}
