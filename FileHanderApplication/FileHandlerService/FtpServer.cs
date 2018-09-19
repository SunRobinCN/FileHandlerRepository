using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

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
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            var builder = new ConfigurationBuilder().
                SetBasePath(pathToContentRoot).
                AddJsonFile("appSettings.json");
            IConfigurationRoot config = builder.Build();
            int ftpPort = config.GetValue<int>("ftpPort");
            _listener = new TcpListener(IPAddress.Any, ftpPort);
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
            Log.Information("A client is connected with IP " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    ProcessClientConnection connection = new ProcessClientConnection(client);
                    connection.HandleClient();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            });

            //ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
        }
    }
}
