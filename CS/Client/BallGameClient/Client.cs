using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace BallGameClient
{
    class Client : IDisposable
    {
        private const int PORT = 8888;
        private int playerId;
        public StreamReader reader;
        public StreamWriter writer;
        public BlockingCollection<String> responseQueue;
        public BlockingCollection<String> broadcastQueue;
        public static int ballHolder;
        public static List<int> inGamePlayers;

        public Client(ClientGUI app)
        {
            SetUpConnections();

            responseQueue = new BlockingCollection<String>();
            broadcastQueue = new BlockingCollection<String>();
            new Thread((new ResponseController(this)).Run).Start();

            playerId = int.Parse(responseQueue.Take());

            string resultString = responseQueue.Take();
            bool connectionSuccessful = resultString.Trim().ToLower().CompareTo("success") == 0;
            if (!connectionSuccessful)
            {
                Application.Exit();
            }


            new Thread(() =>
            {
                while (true)
                {
                    if (broadcastQueue.Count() > 0)
                    {
                        string broadcastLine = broadcastQueue.Take() + "\n";
                        string[] broadcastSplit = broadcastLine.Split('#');

                        string broadcastFunction = broadcastSplit[1].Trim();
                        string broadcastMessage = broadcastSplit[0].Trim();

                        int newPlayerId;

                        switch (broadcastFunction)
                        {
                            case "addBroadcast":
                                app.AddBroadcastToGui(broadcastMessage);
                                break;
                            case "addId":
                                if (ResponseController.serverHasRestarted)
                                {
                                    ResponseController.serverHasRestarted = false;
                                    newPlayerId = int.Parse(broadcastMessage);
                                    foreach (int id in inGamePlayers)
                                    {
                                        app.RemoveIdFromGui(id);
                                    }
                                    playerId = newPlayerId;
                                    app.SetIdLabel(newPlayerId);
                                    app.AddBroadcastToGui($"Server change - your new ID is {newPlayerId}");
                                    responseQueue.Take();
                                    responseQueue.Take();
                                }
                                app.AddIdToGui(int.Parse(broadcastMessage));
                                break;
                            case "removeId":
                                app.RemoveIdFromGui(int.Parse(broadcastMessage));
                                break;
                            case "setBallHolder":
                                app.SetBallHolderLabel(int.Parse(broadcastMessage));
                                break;
                        }
                    }
                }
            }).Start();
        }

        public void SetUpConnections()
        {
            TcpClient tcpClient = new TcpClient("localhost", PORT);
            NetworkStream stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
        }

        public int GetBallHolder()
        {
            writer.WriteLine("ball_holder");
            writer.Flush();
            string ballHolderString = responseQueue.Take();
            ballHolder = int.Parse(ballHolderString);
            return ballHolder;
        }

        public void PassBall(int playerId)
        {
            writer.WriteLine("pass_ball");
            writer.Flush();
            writer.WriteLine(playerId);
            writer.Flush();
        }

        public bool IsValidPlayer(int playerId)
        {
            return GetPlayers().Contains(playerId);
        }

        public List<int> GetPlayers()
        {
            writer.WriteLine("get_players");
            writer.Flush();
            int numberOfPlayers = int.Parse(responseQueue.Take());
            List<int> allPlayersIds = new List<int>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                allPlayersIds.Add(int.Parse(responseQueue.Take()));
            }
            return allPlayersIds;
        }

        public int GetPlayerId()
        {
            return playerId;
        }

        public void Dispose()
        {
            reader.Close();
            writer.Close();
        }
    }
}
