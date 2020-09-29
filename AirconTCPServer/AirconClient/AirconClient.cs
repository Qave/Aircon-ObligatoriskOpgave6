using AirconTCPServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AirconClient
{
    public class AirconClient
    {
        public void Start()
        {
            try
            {
                TcpClient socket = new TcpClient("localhost", 4646);
                using (socket)
                {
                    NetworkStream ns = socket.GetStream();
                    StreamReader sr = new StreamReader(ns);
                    StreamWriter sw = new StreamWriter(ns);
                    sw.AutoFlush = true;
                    while (true)
                    {
                        Console.WriteLine(sr.ReadLine());
                        try
                        {
                            // Word sent to the server
                            string lineSentToServer = Console.ReadLine();
                            switch (lineSentToServer)
                            {
                                case "HentAlle":
                                    sw.WriteLine(lineSentToServer);
                                    break;
                                case "Hent":
                                    sw.WriteLine(lineSentToServer);
                                    int n;
                                    Console.WriteLine(sr.ReadLine());
                                    var id = Console.ReadLine();
                                    if (int.TryParse(id, out n))
                                    {
                                        sw.WriteLine(id);
                                    }                                                                 
                                    break;
                                case "Gem":
                                    // id, navn, temp, fugt
                                    Console.WriteLine(sr.ReadLine());
                                    sw.WriteLine(lineSentToServer);
                                    
                                    string objToSave = Console.ReadLine();
                                    sw.WriteLine(objToSave);
                                    string returnMsg = sr.ReadLine();
                                    Console.WriteLine(returnMsg);
                                    break;
                                default:
                                    break;
                            }
                            // flush the streamWriter
                            sw.Flush();

                            string line = sr.ReadLine();
                            Console.WriteLine(line);
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Connection to the server cannot be made, is it running?");
                            return;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("No connection could be made to the server.");
                return;
            }
        }
    }
}
