using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace BallGameServer
{
    public class ClientHandler
    {
        public static readonly Object sync = new object();
        public static HashSet<int> inGamePlayerIds = new HashSet<int>();
        private static HashSet<int> allPlayerIds = new HashSet<int>();
        public BlockingCollection<String> responseQueue = new BlockingCollection<String>();
        public StreamReader reader;
        public int playerId;
        private StreamWriter writer;
        private Thread responseControllerThread;

        public ClientHandler(TcpClient tcpClient)
        {
            Stream stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            playerId = GeneratePlayerId();
        }

        private int GeneratePlayerId()
        {
            int ID_BOUND = 10000;
            int playerId = (new Random()).Next(ID_BOUND);
            while (allPlayerIds.Contains(playerId))
            {
                playerId = (new Random()).Next(ID_BOUND);
            }
            allPlayerIds.Add(playerId);
            inGamePlayerIds.Add(playerId);
            return playerId;
        }

        private void BroadcastMessage(String message)
        {

            foreach (ClientHandler client in ServerProgram.clients)
            {
                try
                {
                    client.writer.WriteLine(message);
                    client.writer.Flush();
                }
                catch (IOException)
                {
                    // Client has disconnected
                }
            }

        }

        private void PrintAllPlayers()
        {
            Console.WriteLine("In-game players:");
            foreach (int id in inGamePlayerIds)
            {
                Console.WriteLine(id);
            }
        }

        private int GetNextAvailablePlayer()
        {
            int nextPlayer = 0;
            foreach (int id in inGamePlayerIds)
            {
                if (id != playerId)
                {
                    nextPlayer = id;
                    break;
                }
            }
            return nextPlayer;
        }

        private void RemovePlayerFromInGameSet(int playerId)
        {
            inGamePlayerIds.Remove(playerId);
        }

        public void PlayerDisconnected()
        {
            BroadcastMessage(playerId + " has disconnected#addBroadcast:broadcast");
            BroadcastMessage(playerId + "#removeId:broadcast");
            Console.WriteLine("\nPlayer " + playerId + " has disconnected");

            if (Ball.GetBallHolder() == playerId)  // Disconnecting player has the ball
            {
                if (inGamePlayerIds.Count() > 1)  // There is someone to pass ball to
                {
                    int newBallHolder = GetNextAvailablePlayer();
                    Ball.SetBallHolder(newBallHolder);
                    RemovePlayerFromInGameSet(playerId);
                    PrintAllPlayers();
                    BroadcastMessage(playerId + " disconnected. New ball holder is " + newBallHolder + "#addBroadcast:broadcast");
                    BroadcastMessage(newBallHolder + "#setBallHolder:broadcast");
                    Console.WriteLine("\nBall holder (" + playerId + ") has left the game, ball has been automatically passed to " + newBallHolder);
                }
                else  // There isn't someone to pass ball to
                {
                    Ball.SetBallHolder(-1);
                    RemovePlayerFromInGameSet(playerId);
                    PrintAllPlayers();
                    Console.WriteLine("\nLast player (" + playerId + ") has left, ball will be automatically passed to the next player that joins");
                }
            }
            else  // Disconnecting player does not have ball
            {
                RemovePlayerFromInGameSet(playerId);
                PrintAllPlayers();
            }

            ServerProgram.clients.Remove(this);
            reader.Close();
            writer.Close();
        }

        public void Run()
        {
            writer.WriteLine(playerId + ":individual");
            writer.Flush();

            Console.WriteLine("\nPlayer " + playerId + " has joined the game");
            PrintAllPlayers();

            writer.WriteLine("SUCCESS:individual");
            writer.Flush();
            BroadcastMessage("Player " + playerId + " has joined the game#addBroadcast:broadcast");
            BroadcastMessage(playerId + "#addId:broadcast");

            if (Ball.GetBallHolder() == -1)
            {
                Console.WriteLine("\n" + playerId + " has been passed the ball by the server");
                Ball.SetBallHolder(playerId);
            }

            responseControllerThread = new Thread(new ResponseController(this).Run);
            responseControllerThread.Start();

            while (true)
            {
                String clientRequest = responseQueue.Take();
                switch (clientRequest)
                {
                    case "ball_holder":
                        writer.WriteLine(Ball.GetBallHolder() + ":individual");
                        writer.Flush();
                        break;
                    case "pass_ball":
                        int newBallHolder = int.Parse(responseQueue.Take());
                        Console.WriteLine("\nBall passed: " + Ball.GetBallHolder() + " -> " + newBallHolder);
                        BroadcastMessage("Ball passed " + Ball.GetBallHolder() + " -> " + newBallHolder + "#addBroadcast:broadcast");
                        BroadcastMessage(newBallHolder + "#setBallHolder:broadcast");
                        Ball.SetBallHolder(newBallHolder);
                        break;
                    case "get_players":
                        writer.WriteLine(inGamePlayerIds.Count() + ":individual");
                        writer.Flush();
                        foreach (int id in inGamePlayerIds)
                        {
                            writer.WriteLine(id + ":individual");
                            writer.Flush();
                        }
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Client:" + playerId;
        }
    }
}
