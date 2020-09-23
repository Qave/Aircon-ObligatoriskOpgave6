using System;

namespace AirconTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AirconServer server = new AirconServer();

            server.Start();
            Console.ReadLine();
        }
    }
}
