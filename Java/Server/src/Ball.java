class Ball {

    private static int ballHolder = -1;
    private static int prevBallHolder = -1;

    static int getBallHolder() {
        return ballHolder;
    }

    static int getPrevBallHolder() {
        return prevBallHolder;
    }

    static void setBallHolder(int playerId) {
        prevBallHolder = ballHolder;
        ballHolder = playerId;
    }
}
