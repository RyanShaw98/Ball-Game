using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BallGameClient
{
    public partial class ClientGUI : Form
    {
        private Client client;

        public ClientGUI()
        {
            InitializeComponent();
            this.CreateHandle();

            client = new Client(this);

            SetIdLabel(client.GetPlayerId());
            SetBallHolderLabel(client.GetBallHolder());

            Client.inGamePlayers = client.GetPlayers();
            foreach (int id in Client.inGamePlayers)
            {
                if (id != client.GetPlayerId())
                {
                    AddIdToGui(id);
                }
            }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientGUI());
        }

        private void passBtn_Click(object sender, EventArgs e)
        {
            if (client.GetPlayerId() == client.GetBallHolder())
            {
                int passBallTo;
                try
                {
                    passBallTo = int.Parse(idField.Text);
                    if (client.IsValidPlayer(passBallTo))
                    {
                        client.PassBall(int.Parse(idField.Text));
                    }
                    else
                    {
                        MessageBox.Show("That player is not in the game!");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid ID");
                }
            }
            else
            {
                MessageBox.Show("You do not have the ball");
            }
        }

        public void AddIdToGui(int id)
        {
            try
            {
                String idString = (id == client.GetPlayerId()) ? id + " (You)\n" : id + "\n";
                playersArea.Invoke(new Action(() => playersArea.AppendText(idString)));
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Error adding ID to GUI");
            }
        }

        public void RemoveIdFromGui(int id)
        {
            playersArea.Invoke(new Action(() =>
            {
                string[] allIdsArr = playersArea.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                List<string> allIdsList = new List<string>();
                foreach (string playerId in allIdsArr)
                {
                    Console.WriteLine($"playerId = {playerId}");
                    string newPlayerId = (playerId.Contains("You")) ? playerId.Split(' ')[0] : playerId;
                    Console.WriteLine($"newPlayerId = {newPlayerId}");
                    allIdsList.Add(newPlayerId);
                }
                allIdsList.Remove(id.ToString());
                playersArea.Invoke(new Action(() => playersArea.Text = ""));
                foreach (string playerId in allIdsList)
                {

                    AddIdToGui(int.Parse(playerId));
                }
            }));
        }

        public void AddBroadcastToGui(string broadcastMessage)
        {
            try
            {
                logArea.Invoke(new Action(() => logArea.AppendText(broadcastMessage + "\n")));
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Error while appending to log\n" + e.Message);
            }
        }

        public void SetIdLabel(int id)
        {
            idLabel.Invoke(new Action(() => idLabel.Text = "Your ID: " + id));
        }

        public void SetBallHolderLabel(int id)
        {
            string ballHolder = (id == client.GetPlayerId()) ? "Ball Holder: You" : "Ball Holder: " + id;
            ballHolderLabel.Invoke(new Action(() => ballHolderLabel.Text = ballHolder));
        }

        private void ClientGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
