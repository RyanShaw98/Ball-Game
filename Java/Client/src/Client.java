import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class Client implements AutoCloseable {

    private final static int PORT = 8888;
    static int ballHolder;
    static List<Integer> inGamePlayers;
    Scanner reader;
    PrintWriter writer;
    BlockingQueue<String> responseQueue;
    BlockingQueue<String> broadcastQueue;
    private int playerId;

    Client(ClientGUI app) throws IOException, InterruptedException {
        setUpConnections();

        responseQueue = new LinkedBlockingQueue<>();
        broadcastQueue = new LinkedBlockingQueue<>();
        new Thread(new ResponseController(this)).start();

        playerId = Integer.parseInt(responseQueue.take());

        String resultString = responseQueue.take();
        boolean connectionSuccessful = resultString.trim().compareToIgnoreCase("success") == 0;
        if (!connectionSuccessful) {
            System.exit(-1);
        }

        new Thread(() -> {
            while (true) {
                try {
                    if (broadcastQueue.size() > 0) {
                        String broadcastLine = broadcastQueue.take() + "\n";
                        String[] broadcastSplit = broadcastLine.split("#");

                        String broadcastFunction = broadcastSplit[1].trim();
                        String broadcastMessage = broadcastSplit[0].trim();

                        int newPlayerId;

                        switch (broadcastFunction) {
                            case "addBroadcast":
                                app.addBroadcastToGui(broadcastMessage);
                                break;
                            case "addId":
                                if (ResponseController.serverHasRestarted) {
                                    ResponseController.serverHasRestarted = false;
                                    newPlayerId = Integer.parseInt(broadcastMessage);
                                    for (int id : inGamePlayers) {
                                        app.removeIdFromGui(id);
                                    }
                                    playerId = newPlayerId;
                                    app.setIdLabel(newPlayerId);
                                    app.addBroadcastToGui("Server change - your new ID is " + newPlayerId);
                                    responseQueue.take();
                                    responseQueue.take();
                                }
                                app.addIdToGui(Integer.parseInt(broadcastMessage));
                                break;
                            case "removeId":
                                app.removeIdFromGui(Integer.parseInt(broadcastMessage));
                                break;
                            case "setBallHolder":
                                app.setBallHolderLabel(Integer.parseInt(broadcastMessage));
                                break;
                        }
                    }
                } catch (InterruptedException e) {
                    System.err.println("Error while removing item from broadcast queue\n" + e.getMessage());
                }
            }
        }).start();
    }

    void setUpConnections() throws IOException {
        Socket socket = new Socket("localhost", PORT);
        reader = new Scanner(socket.getInputStream());
        writer = new PrintWriter(socket.getOutputStream(), true);
    }

    int getBallHolder() {
        try {
            writer.println("ball_holder");
            String ballHolderString = responseQueue.take();
            ballHolder = Integer.parseInt(ballHolderString);
            return ballHolder;
        } catch (InterruptedException e) {
            System.err.println("Error while removing item from response queue\n" + e.getMessage());
        }
        return 0;
    }

    void passBall(int playerId) {
        writer.println("pass_ball");
        writer.println(playerId);
    }

    boolean isValidPlayer(int playerId) {
        return getPlayers().contains(playerId);
    }

    List<Integer> getPlayers() {
        try {
            writer.println("get_players");
            int numberOfPlayers = Integer.parseInt(responseQueue.take());
            List<Integer> allPlayerIds = new ArrayList<>();
            for (int i = 0; i < numberOfPlayers; i++) {
                allPlayerIds.add(Integer.parseInt(responseQueue.take()));
            }
            return allPlayerIds;
        } catch (InterruptedException e) {
            System.err.println("Error while removing item from response queue\n" + e.getMessage());
        }
        return new ArrayList<>();
    }

    int getPlayerId() {
        return playerId;
    }

    @Override
    public void close() {
        reader.close();
        writer.close();
    }
}
