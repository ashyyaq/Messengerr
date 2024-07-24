using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Messengerr.Network
{
    public class MessengerClient
    {
        private System.Net.Sockets.TcpClient client;
        private string ipAddress;

        public MessengerClient(string ipAddress)
        {
            this.ipAddress = ipAddress;
            client = new System.Net.Sockets.TcpClient(ipAddress, 12345);
        }

        public void StartListening(CancellationToken token)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (!token.IsCancellationRequested && (bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message); 
            }
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Disconnect()
        {
            client.Close();
        }
    }
}
