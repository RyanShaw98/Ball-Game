using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallGameServer.Tests
{
    [TestClass()]
    public class BallTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Ball.SetBallHolder(150);
        }

        [TestMethod()]
        public void GetBallHolderTest()
        {
            Assert.AreEqual(150, Ball.GetBallHolder());
        }

        [TestMethod()]
        public void GetPrevBallHolderTest()
        {
            Ball.SetBallHolder(250);
            Assert.AreEqual(150, Ball.GetPrevBallHolder());
        }

        [TestMethod()]
        public void SetBallHolderTest()
        {
            Ball.SetBallHolder(250);
            Assert.AreEqual(250, Ball.GetBallHolder());
        }
    }
}