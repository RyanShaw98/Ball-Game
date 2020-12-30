import java.io.IOException;
import java.util.NoSuchElementException;

public class ResponseController implements Runnable {

    static boolean serverHasRestarted;
    private final Client client;

    ResponseController(Client client) {
        this.client = client;
    }

    @Override
    @SuppressWarnings("InfiniteLoopStatement")
    public void run() {
        while (true) {
            try {
                String serverResponse = client.reader.nextLine();
                String[] serverResponseArr = serverResponse.split(":");
                boolean responseIsBroadcast = serverResponseArr[1].equals("broadcast");
                String response = serverResponseArr[0];
                if (responseIsBroadcast) {
                    client.broadcastQueue.add(response);
                } else {
                    client.responseQueue.add(response);
                }
            } catch (NoSuchElementException e) {
                System.err.println("Server killed");
                try {
                    if (Client.ballHolder == client.getPlayerId()) {
                        try {
                            ProcessBuilder pb = new ProcessBuilder("java", "-jar", ".\\lib\\BallGameServer.jar");
                            pb.start();
                            System.out.println("New server instance created");
                            Thread.sleep(2000);
                        } catch (IOException | InterruptedException exc) {
                            System.err.println(e.getMessage());
                        }
                    }
                    client.setUpConnections();
                    System.out.println("Joined new server instance");
                } catch (IOException ex) {
                    System.err.println("Error creating communication objects\n" + ex.getMessage());
                }
                serverHasRestarted = true;
            }
        }
    }
}
