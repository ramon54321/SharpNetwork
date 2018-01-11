using System;
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
        private bool _isConnected;

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

            // -- Call OnConnect
            InternalOnConnect();

            // -- Set manual connected flag
            _isConnected = true;

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
            if (!_isConnected)
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
         * Send raw bytes to host.
         */
        public void SendMessage(byte[] messageBytes)
        {
            // TODO: Client keeps trying to write even after it disconnected
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
            Console.WriteLine(str);
        }

        /**
         * Called when the client connects.
         */
        public virtual void OnConnect()
        {
            Console.WriteLine("Connected.");
        }

        /**
         * Called when the connection to the host is interupted.
         */
        public virtual void OnDisconnect()
        {
            Console.WriteLine("Disconnected");
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
        }

        /**
         * Closes client socket, stream and tcp client. Sets manual flag to false. Fires OnDisconnect.
         */
        public void Close()
        {
            if (!_isConnected)
                return;

            _tcpClient.Client.Close();
            _networkStream.Close();
            _tcpClient.Close();

            _isConnected = false;

            InternalOnDisconnect();
        }
    }
}
