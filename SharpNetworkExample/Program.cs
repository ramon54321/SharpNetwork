using System;
using SharpNetwork;

namespace SharpNetworkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
                return;

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

        static void StartServer()
        {
            NetworkServer networkServer = new NetworkServer("127.0.0.1", 7788);

            Console.WriteLine("End");
            Console.Read();

            networkServer.Close();
        }

        static void StartClient()
        {
            NetworkClient networkClient = new NetworkClient("127.0.0.1", 7788);

            Console.WriteLine("End");
            Console.Read();

            networkClient.Close();
        }
    }
}
