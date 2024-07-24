using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Messenger.Network;
using Messengerr.Network;

namespace Messenger
{
    public partial class ChatWindow : Window
    {
        private bool isServer;
        private string username;
        private string ipAddress;
        private TcpServer server;
        private MessengerClient client;
        private CancellationTokenSource cts;

        public ChatWindow(bool isServer, string username, string ipAddress)
        {
            InitializeComponent();
            this.isServer = isServer;
            this.username = username;
            this.ipAddress = ipAddress;

            if (isServer)
            {
                server = new TcpServer();
                cts = new CancellationTokenSource();
                Task.Run(() => server.StartListening(cts.Token));
            }
            else
            {
                client = new MessengerClient(ipAddress);
                cts = new CancellationTokenSource();
                Task.Run(() => client.StartListening(cts.Token));
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = new Message(username, MessageTextBox.Text);
            if (isServer)
            {
                server.BroadcastMessage(message.ToString());
            }
            else
            {
                client.SendMessage(message.ToString());
            }
            ChatListBox.Items.Add(message.ToString());
            MessageTextBox.Clear();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            if (isServer)
            {
                server.Stop();
            }
            else
            {
                client.Disconnect();
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
