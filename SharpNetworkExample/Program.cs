using System;
using System.Text;
using SharpNetwork;

namespace SharpNetworkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // -- Some trickery to deal with no arguments - DEVELOPMENT ONLY
            if (args.Length < 1)
            {
                return;
                args = new string[1];
                args[0] = "client";
            }

            /**
             * Start the app in correct mode given the argument.
             */
            if (args[0] == "server")
            {
                Console.WriteLine("\tStarting Server");
                StartServer();
            }
            else if (args[0] == "client")
            {
                Console.WriteLine("\tStarting Client");
                StartClient();
            }

        }

        /**
         * Start as Server.
         */
        static void StartServer()
        {
            NetworkServer networkServer = new NetworkServer("127.0.0.1", 7788);

            Console.WriteLine("End");

            while (true)
            {
                string command = Console.ReadLine();

                if(command == String.Empty)
                    break;

                foreach (var serverClientKeyValuePair in networkServer.GetServerClients())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(command);
                    serverClientKeyValuePair.Value.SendMessage(messageBytes);
                }
            }

            networkServer.Close();
        }

        /**
         * Start as Client.
         */
        static void StartClient()
        {
            NetworkClient networkClient = new NetworkClient("127.0.0.1", 7788);

            Console.WriteLine("End");

            while (true)
            {
                string command = Console.ReadLine();

                if (command == String.Empty)
                    break;

                byte[] messageBytes = Encoding.UTF8.GetBytes(command);
                networkClient.SendMessage(messageBytes);
            }

            networkClient.Close();
        }
    }
}
