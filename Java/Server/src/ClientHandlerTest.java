import org.junit.After;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.io.IOException;
import java.net.Socket;

public class ClientHandlerTest {

    private ClientHandler ch;
    private Process serverProcess;

    @Before
    public void setUp() throws Exception {
        ProcessBuilder pb = new ProcessBuilder("java", "-jar", ".\\lib\\BallGameServer.jar");
        serverProcess = pb.start();
        ch = new ClientHandler(new Socket("localhost", 8888));
    }

    @Test
    public void generatePlayerId() {
        Assert.assertTrue(ch.playerId > 0 && ch.playerId < 10000);
    }

    @Test
    public void canPlayersConnect() throws IOException {
        Process p1 = new ProcessBuilder("java", "-jar", ".\\lib\\BallGameClient.jar").start();
        Process p2 = new ProcessBuilder("java", "-jar", ".\\lib\\BallGameClient.jar").start();
        Process p3 = new ProcessBuilder("java", "-jar", ".\\lib\\BallGameClient.jar").start();

        Assert.assertTrue(p1.isAlive());
        Assert.assertTrue(p2.isAlive());
        Assert.assertTrue(p3.isAlive());

        p1.destroy();
        p2.destroy();

        Assert.assertFalse(p1.isAlive());
        Assert.assertFalse(p2.isAlive());
        Assert.assertTrue(p3.isAlive());

        p3.destroy();

        Assert.assertFalse(p3.isAlive());
    }

    @After
    public void tearDown() {
        serverProcess.destroy();
    }
}
