using System;
using System.Net.Sockets;
using System.Text;

namespace SharpNetwork
{
    public class ServerClient
    {
        private readonly NetworkServer _networkServer;
        private readonly TcpClient _tcpClient;
        private readonly Guid _guid;
        private readonly NetworkStream _networkStream;

        private byte[] _readBuffer;
        private bool _isConnected;

        /**
         * Set up tcp client for client and get network stream, then init read buffer and start reading.
         */
        public ServerClient(NetworkServer networkServer, TcpClient tcpClient, Guid guid)
        {
            // -- Define private variables
            _networkServer = networkServer;
            _tcpClient = tcpClient;
            _guid = guid;

            // -- Call OnConnect
            InternalOnConnect();

            // -- Set manual connected flag
            _isConnected = true;

            // -- Get stream from tcp client
            _networkStream = _tcpClient.GetStream();

            // -- Init readBuffer
            _readBuffer = new byte[NetworkServer.BufferSize];

            // -- Start reading Async
            NextRead();
        }

        /**
         * Get the guid of the client.
         */
        public Guid GetGuid()
        {
            return _guid;
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
                Console.WriteLine("Exception in BeginRead");
                Close();
                return;
            }
        }

        /**
         * Delegate callback when data is received.
         */
        private void HandleRead(IAsyncResult result)
        {
            // -- Check manual connection flag
            if(!_isConnected)
                return;

            // -- Get number of bytes in received data
            int numberOfBytes = 0;
            try
            {
                numberOfBytes = _networkStream.EndRead(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in EndRead");
                Close();
                return;
            }

            // -- Create new array
            byte[] messageBytes = new byte[numberOfBytes];

            // -- Copy buffer to array
            Array.Copy(_readBuffer, messageBytes, numberOfBytes);

            // -- Call virtual method - OnMessageReceived
            OnMessageReceived(messageBytes);

            // -- Call read again
            NextRead();
        }

        /**
         * Send raw bytes to client.
         */
        public virtual void SendMessage(byte[] messageBytes)
        {
            try
            {
                _networkStream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception in Write");
                Close();
                return;
            }
        }

        /**
         * Virtual method to deal with received message.
         * Called from HandleRead delegate.
         */
        public virtual void OnMessageReceived(byte[] message)
        {
            string str = Encoding.UTF8.GetString(message);
            Console.WriteLine(_guid + " says: " + str);
        }

        /**
         * Called when the client is first connected. Called before server OnClientConnected.
         */
        public virtual void OnConnect()
        {
            Console.WriteLine("User, " + _guid + ", connected.");
        }

        /**
         * Called when the connection to the host is interupted.
         */
        public virtual void OnDisconnect()
        {
            Console.WriteLine("User, " + _guid + ", disconnected.");
        }

        /**
         * Internal methods are called by library.
         */
        private void InternalOnConnect()
        {
            OnConnect();
        }
        private void InternalOnDisconnect()
        {
            OnDisconnect();
            _networkServer.OnClientDisconnected(this);
        }

        /**
         * Closes client socket, stream and tcp client. Sets manual flag to false. Fires OnDisconnect.
         */
        public void Close()
        {
            if(!_isConnected)
                return;

            _tcpClient.Client.Close();
            _networkStream.Close();
            _tcpClient.Close();

            _isConnected = false;

            InternalOnDisconnect();
        }
    }
}
