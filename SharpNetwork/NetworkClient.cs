using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SharpNetwork
{
    public class NetworkClient
    {
        private readonly string _hostname;
        private readonly int _port;

        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _networkStream;

        private byte[] _readBuffer;

        /**
         * Creates a new client and connects to the specified host, then sets the stream variable and starts listening.
         */
        public NetworkClient(string hostname, int port, int bufferSize = 4096)
        {
            // -- Define private variables
            _hostname = hostname;
            _port = port;

            // -- Set up tcp client
            _tcpClient = new TcpClient();

            // -- Connect to host
            _tcpClient.Connect(_hostname, _port);

            // -- Get stream from tcp client
            _networkStream = _tcpClient.GetStream();

            // -- Init readBuffer
            _readBuffer = new byte[bufferSize];

            // -- Start reading Async
            NextRead();
        }

        /**
         * Calls the async read.
         * Used just to allow a try catch around any call to read.
         */
        private void NextRead()
        {
            try
            {
                _networkStream.BeginRead(_readBuffer, 0, _readBuffer.Length, HandleRead, null);
            }
            catch (Exception e)
            {
                OnDisconnect();
            }
        }

        /**
         * Delegate callback when data is received.
         */
        private void HandleRead(IAsyncResult result)
        {
            // -- Call read again
            NextRead();

            // -- Checking
            if (!_tcpClient.Connected)
                return;

            if (!result.IsCompleted)
                return;

            // -- Get number of bytes in received data
            int numberOfBytes = _networkStream.EndRead(result);
            
            // -- Create new array
            byte[] messageBytes = new byte[numberOfBytes];
                
            // -- Copy buffer to array
            Array.Copy(_readBuffer, messageBytes, numberOfBytes);

            // -- Call virtual method - OnMessageReceived
            OnMessageReceived(messageBytes);
        }

        /**
         * Send raw bytes to host.
         */
        public void SendMessage(byte[] messageBytes)
        {
            _networkStream.Write(messageBytes, 0, messageBytes.Length);
        }

        /**
         * Virtual method to deal with received message.
         * Called from HandleRead delegate.
         */
        protected virtual void OnMessageReceived(byte[] message)
        {
            string str = Encoding.UTF8.GetString(message);
            Console.WriteLine(str);
        }

        /**
         * Called when the connection to the host is interupted.
         */
        protected virtual void OnDisconnect()
        {
            Console.WriteLine("Disconnected");
        }

        /**
         * Checks if the client is connected, and closes it if it is.
         */
        public void Close()
        {
            if(!_tcpClient.Connected)
                return;

            _tcpClient.Close();
        }
    }
}
