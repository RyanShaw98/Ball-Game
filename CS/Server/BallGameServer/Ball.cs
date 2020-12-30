namespace BallGameServer
{
    public class Ball
    {
        private static int ballHolder = -1;
        private static int prevBallHolder = -1;

        public static int GetBallHolder()
        {
            return ballHolder;
        }

        public static int GetPrevBallHolder()
        {
            return prevBallHolder;
        }

        public static void SetBallHolder(int playerId)
        {
            prevBallHolder = ballHolder;
            ballHolder = playerId;
        }
    }
}
