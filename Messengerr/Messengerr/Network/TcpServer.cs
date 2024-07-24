using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Network
{
    public class TcpServer
    {
        private TcpListener listener;
        private List<System.Net.Sockets.TcpClient> clients;
        private const int port = 12345;

        public TcpServer()
        {
            listener = new TcpListener(IPAddress.Any, port);
            clients = new List<System.Net.Sockets.TcpClient>();
        }

        public async Task StartListening(CancellationToken token)
        {
            listener.Start();
            Console.WriteLine("Сервер запущен...");

            while (!token.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                Console.WriteLine("Клиент подключен.");
                Task.Run(() => HandleClient(client, token));
            }
        }

        private async Task HandleClient(System.Net.Sockets.TcpClient client, CancellationToken token)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];

            while (!token.IsCancellationRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (bytesRead == 0)
                {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);  
                BroadcastMessage(message);
            }

            clients.Remove(client);
            client.Close();
            Console.WriteLine("Клиент отключен.");
        }

        public void BroadcastMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                var stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Stop()
        {
            listener.Stop();
            foreach (var client in clients)
            {
                client.Close();
            }
        }
    }
}
