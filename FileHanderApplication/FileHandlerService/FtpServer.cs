using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileHandlerService
{
    public class FtpServer
    {
        private TcpListener _listener;

        public FtpServer()
        {
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 21);
            _listener.Start();
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
        }

        public void Stop()
        {
            _listener?.Stop();
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            TcpClient client = _listener.EndAcceptTcpClient(result);
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);

            Task.Factory.StartNew(() =>
            {
                ProcessClientConnection connection = new ProcessClientConnection(client);
                connection.HandleClient();
            });

            //ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
        }
    }
}
