import java.util.NoSuchElementException;

public class ResponseController implements Runnable {

    private ClientHandler ch;

    ResponseController(ClientHandler ch) {
        this.ch = ch;
    }

    @Override
    @SuppressWarnings("InfiniteLoopStatement")
    public void run() {
        try {
            while (true) {
                String clientRequest = ch.reader.nextLine();
                ch.responseQueue.add(clientRequest);

                // Checks ball hasn't been passed to a recently disconnected player
                boolean ballHeldByInGamePlayer = ClientHandler.inGamePlayerIds.contains(Ball.getBallHolder());
                if (!ballHeldByInGamePlayer) {
                    if (Ball.getPrevBallHolder() == -1) {
                        Ball.setBallHolder((int) ClientHandler.inGamePlayerIds.toArray()[0]);
                    } else {
                        Ball.setBallHolder(Ball.getPrevBallHolder());
                    }
                    System.err.println("Error passing ball - ball returned to " + Ball.getBallHolder());
                }
            }
        } catch (NoSuchElementException e) {
            synchronized (ClientHandler.sync) {
                ch.playerDisconnected();
            }
        }
    }
}
