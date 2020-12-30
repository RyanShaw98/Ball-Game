using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace BallGameServer.Tests
{
    [TestClass()]
    public class ClientHandlerTests
    {
        private ClientHandler ch;
        private Process serverProcess;


        [TestInitialize]
        public void TestInitialize()
        {
            serverProcess = new Process();
            serverProcess.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\lib\\BallGameServer.exe";
            serverProcess.StartInfo.CreateNoWindow = true;
            serverProcess.Start();

            ch = new ClientHandler(new TcpClient("localhost", 8888));
        }

        [TestMethod()]
        public void GeneratePlayerIdTest()
        {
            Assert.IsTrue(ch.playerId > 0 && ch.playerId < 10000);
        }

        [TestMethod()]
        public void CanPlayersConnectTest()
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\lib\\BallGameClient.exe";
            p1.StartInfo.CreateNoWindow = true;
            p1.Start();

            Process p2 = new Process();
            p2.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\lib\\BallGameClient.exe";
            p2.StartInfo.CreateNoWindow = true;
            p2.Start();

            Process p3 = new Process();
            p3.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\lib\\BallGameClient.exe";
            p3.StartInfo.CreateNoWindow = true;
            p3.Start();

            Assert.IsTrue(!p1.HasExited);
            Assert.IsTrue(!p2.HasExited);
            Assert.IsTrue(!p3.HasExited);

            p1.Kill();
            p2.Kill();

            Assert.IsTrue(p1.HasExited);
            Assert.IsTrue(p2.HasExited);
            Assert.IsTrue(!p3.HasExited);

            p3.Kill();

            Assert.IsTrue(p3.HasExited); ;
        }

        [TestCleanup]
        public void KillProcesses()
        {
            serverProcess.Kill();

        }
    }
}