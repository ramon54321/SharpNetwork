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

        /**
         * Set up tcp client for client and get network stream.
         */
        public ServerClient(TcpClient tcpClient, Guid guid)
        {
            // -- Define private variables
            _tcpClient = tcpClient;
            _guid = guid;

            // -- Get stream
            _networkStream = _tcpClient.GetStream();
        }

        /**
         * Send raw bytes to client.
         */
        public virtual void SendMessage(byte[] messageBytes)
        {
            _networkStream.Write(messageBytes, 0, messageBytes.Length);
        }
    }
}
