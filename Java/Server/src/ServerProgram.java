import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

public class ServerProgram {

    private static final int PORT = 8888;
    static List<ClientHandler> clients = new ArrayList<>();

    @SuppressWarnings("InfiniteLoopStatement")
    private static void runServer() {
        ServerSocket serverSocket;
        try {
            serverSocket = new ServerSocket(PORT);
            System.out.println("Waiting for connections...");
            while (true) {
                Socket socket = serverSocket.accept();
                ClientHandler newClient = new ClientHandler(socket);
                new Thread(newClient).start();
                clients.add(newClient);
            }
        } catch (Exception e) {
            System.err.println(e.getMessage());
        }
    }

    public static void main(String[] args) {
        runServer();
    }
}
