import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.HashSet;
import java.util.Random;
import java.util.Scanner;
import java.util.Set;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class ClientHandler implements Runnable {

    final static Object sync = new Object();
    static Set<Integer> inGamePlayerIds = new HashSet<>();
    private static Set<Integer> allPlayerIds = new HashSet<>();
    BlockingQueue<String> responseQueue = new LinkedBlockingQueue<>();
    Scanner reader;
    int playerId;
    private PrintWriter writer;

    ClientHandler(Socket socket) throws IOException {
        reader = new Scanner(socket.getInputStream());
        writer = new PrintWriter(socket.getOutputStream(), true);
        playerId = generatePlayerId();
    }

    private int generatePlayerId() {
        final int ID_BOUND = 10000;
        int playerId = (new Random()).nextInt(ID_BOUND);
        while (allPlayerIds.contains(playerId)) {
            playerId = (new Random()).nextInt(ID_BOUND);
        }
        allPlayerIds.add(playerId);
        inGamePlayerIds.add(playerId);
        return playerId;
    }

    private void broadcastMessage(String message) {
        for (ClientHandler client : ServerProgram.clients) {
            client.writer.println(message);
        }
    }

    private void printAllPlayers() {
        System.out.println("In-game players:");
        for (int id : inGamePlayerIds) {
            System.out.println(id);
        }
    }

    private int getNextAvailablePlayer() {
        int nextPlayer = 0;
        for (int id : inGamePlayerIds) {
            if (id != playerId) {
                nextPlayer = id;
                break;
            }
        }
        return nextPlayer;
    }

    private void removePlayerFromInGameSet(int playerId) {
        inGamePlayerIds.removeIf(id -> id.equals(playerId));
    }

    void playerDisconnected() {
        broadcastMessage(playerId + " has disconnected#addBroadcast:broadcast");
        broadcastMessage(playerId + "#removeId:broadcast");
        System.out.println("\nPlayer " + playerId + " has disconnected");

        if (Ball.getBallHolder() == playerId) {  // Disconnecting player has the ball
            if (inGamePlayerIds.size() > 1) {  // There is someone to pass ball to
                int newBallHolder = getNextAvailablePlayer();
                Ball.setBallHolder(newBallHolder);
                removePlayerFromInGameSet(playerId);
                printAllPlayers();
                broadcastMessage(playerId + " disconnected. New ball holder is " + newBallHolder + "#addBroadcast:broadcast");
                broadcastMessage(newBallHolder + "#setBallHolder:broadcast");
                System.out.println("\nBall holder (" + playerId + ") has left the game, ball has been automatically passed to " + newBallHolder);
            } else {  // There isn't someone to pass ball to
                Ball.setBallHolder(-1);
                removePlayerFromInGameSet(playerId);
                printAllPlayers();
                System.out.println("\nLast player (" + playerId + ") has left, ball will be automatically passed to the next player that joins");
            }
        } else {  // Disconnecting player does not have ball
            removePlayerFromInGameSet(playerId);
            printAllPlayers();
        }

        ServerProgram.clients.remove(this);
        reader.close();
        writer.close();
    }

    @Override
    @SuppressWarnings("InfiniteLoopStatement")
    public void run() {
        try {
            writer.println(playerId + ":individual");

            System.out.println("\nPlayer " + playerId + " has joined the game");
            printAllPlayers();

            writer.println("SUCCESS:individual");
            broadcastMessage("Player " + playerId + " has joined the game#addBroadcast:broadcast");
            broadcastMessage(playerId + "#addId:broadcast");

            if (Ball.getBallHolder() == -1) {
                System.out.println("\n" + playerId + " has been passed the ball by the server");
                Ball.setBallHolder(playerId);
            }

            new Thread(new ResponseController(this)).start();

            while (true) {
                String clientRequest = responseQueue.take();
                switch (clientRequest) {
                    case "ball_holder":
                        writer.println(Ball.getBallHolder() + ":individual");
                        break;
                    case "pass_ball":
                        int newBallHolder = Integer.parseInt(responseQueue.take());
                        System.out.println("\nBall passed: " + Ball.getBallHolder() + " -> " + newBallHolder);
                        broadcastMessage("Ball passed " + Ball.getBallHolder() + " -> " + newBallHolder + "#addBroadcast:broadcast");
                        broadcastMessage(newBallHolder + "#setBallHolder:broadcast");
                        Ball.setBallHolder(newBallHolder);
                        break;
                    case "get_players":
                        writer.println(inGamePlayerIds.size() + ":individual");
                        for (int id : inGamePlayerIds) {
                            writer.println(id + ":individual");
                        }
                        break;
                }
            }
        } catch (InterruptedException e) {
            System.err.println("Error while removing item from response queue\n" + e.getMessage());
        }
    }
}
