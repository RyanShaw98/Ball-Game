import javax.swing.*;
import java.awt.*;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class ClientGUI {

    private JTextField idField;
    private JButton passBtn;
    private JPanel panelMain;
    private JTextArea playersArea;
    private JTextArea logArea;
    private JLabel idLabel;
    private JLabel ballHolderLabel;
    private Client client;

    private ClientGUI() throws InterruptedException, IOException {

        logArea.setEditable(false);
        playersArea.setEditable(false);

        client = new Client(this);

        setIdLabel(client.getPlayerId());
        setBallHolderLabel(client.getBallHolder());

        Client.inGamePlayers = client.getPlayers();
        for (int id : Client.inGamePlayers) {
            if (id != client.getPlayerId()) {
                addIdToGui(id);
            }
        }

        passBtn.addActionListener(event -> {
            if (client.getPlayerId() == client.getBallHolder()) {
                int passBallTo;
                try {
                    passBallTo = Integer.parseInt(idField.getText());
                    if (client.isValidPlayer(passBallTo)) {
                        client.passBall(Integer.parseInt(idField.getText()));
                    } else {
                        JOptionPane.showMessageDialog(null, "That player is not in the game!");
                    }
                } catch (NumberFormatException e) {
                    JOptionPane.showMessageDialog(null, "Invalid ID!");
                }
            } else {
                JOptionPane.showMessageDialog(null, "You do not have the ball!");
            }
        });
    }

    public static void main(String[] args) throws InterruptedException, IOException {
        JFrame frame = new JFrame("CE303 Assignment");
        frame.setContentPane(new ClientGUI().panelMain);
        frame.setMinimumSize(new Dimension(500, 400));
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
        frame.setAlwaysOnTop(true);
    }

    void addIdToGui(int id) {
        String idString = (id == client.getPlayerId()) ? id + " (You)\n" : id + "\n";
        playersArea.append(idString);
        playersArea.repaint();
    }

    void removeIdFromGui(int id) {
        String[] allIdsArr = playersArea.getText().split("\n");
        List<String> allIdsList = new ArrayList<>();
        for (String playerId : allIdsArr) {
            playerId = (playerId.contains("You")) ? playerId.substring(0, playerId.length() - 6) : playerId;
            allIdsList.add(playerId);
        }
        allIdsList.remove(Integer.toString(id));
        playersArea.setText("");
        for (String playerId : allIdsList) {
            addIdToGui(Integer.parseInt(playerId));
        }
    }

    void addBroadcastToGui(String broadcastMessage) {
        logArea.append(broadcastMessage + "\n");
        logArea.repaint();
    }

    void setIdLabel(int id) {
        idLabel.setText("Your ID: " + id);
    }

    void setBallHolderLabel(int id) {
        String ballHolder = (id == client.getPlayerId()) ? "Ball Holder: You" : "Ball Holder: " + id;
        ballHolderLabel.setText(ballHolder);
        ballHolderLabel.repaint();
    }
}
