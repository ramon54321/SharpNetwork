using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SharpNetwork
{
    public class ServerClient
    {
        private readonly TcpClient _tcpClient;
        private readonly Guid _guid;
        private readonly NetworkStream _networkStream;

        public ServerClient(TcpClient tcpClient, Guid guid)
        {
            // -- Define private variables
            _tcpClient = tcpClient;
            _guid = guid;

            // -- Get stream
            _networkStream = _tcpClient.GetStream();
        }

        public void SendMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _networkStream.Write(messageBytes, 0, messageBytes.Length);
        }
    }
}
