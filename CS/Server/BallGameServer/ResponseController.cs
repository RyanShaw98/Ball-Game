using System;
using System.IO;
using System.Linq;

namespace BallGameServer
{
    class ResponseController
    {
        private ClientHandler ch;

        public ResponseController(ClientHandler ch)
        {
            this.ch = ch;
        }

        public void Run()
        {

            try
            {
                while (true)
                {
                    String clientRequest = ch.reader.ReadLine();

                    if (!clientRequest.Equals(null))
                    {
                        ch.responseQueue.Add(clientRequest);
                    }

                    // Checks ball hasn't been passed to a recently disconnected player
                    bool ballHeldByInGamePlayer = ClientHandler.inGamePlayerIds.Contains(Ball.GetBallHolder());
                    if (!ballHeldByInGamePlayer)
                    {
                        if (Ball.GetPrevBallHolder() == -1)
                        {
                            Ball.SetBallHolder(ClientHandler.inGamePlayerIds.ToArray<int>()[0]);
                        }
                        else
                        {
                            Ball.SetBallHolder(Ball.GetPrevBallHolder());
                        }
                        Console.WriteLine("Error passing ball - ball returned to " + Ball.GetBallHolder());
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"{ch} disconnected");
                lock (ClientHandler.sync)
                {
                    ch.PlayerDisconnected();
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("\nObjectDisposedException\n");
            }
        }
    }
}
