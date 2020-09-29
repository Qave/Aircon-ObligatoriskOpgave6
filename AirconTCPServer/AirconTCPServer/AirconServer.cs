using AirconLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AirconTCPServer
{
    public class AirconServer
    {
        public void Start()
        {
            TcpListener ServerListen = new TcpListener(IPAddress.Loopback, 4646);
            // start listening for clients on port 3001
            ServerListen.Start();
            Console.WriteLine("Listening for clients...");
            while (true)
            {
                TcpClient socket = ServerListen.AcceptTcpClient();
                Console.WriteLine("Client found");

                // For every new client, run a new task so they run simultaneously
                Task.Run(() =>
                {
                    // create a temp socket for the new client
                    TcpClient tempSocket = socket;
                    DoClient(tempSocket);
                });

            }
        }
        public double Result { get; set; } = 0;
        public string Operator { get; set; }
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public async void DoClient(TcpClient socket)
        {
            using (socket)
            {
                NetworkStream ns = socket.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);
                sw.AutoFlush = true;
                await Persistency.LoadDataAsync();
                while (true)
                {
                    // Word recieved from the client
                    sw.WriteLine("Hent FanOutput målinger, mulige commands: HentAlle, Hent, Gem");
                    try
                    {
                        // Word read from the client
                        string line = sr.ReadLine();
                        // Write the line to the server that is read from the client
                        Console.WriteLine(line);
                        if (line != null)
                        {
                            string result = "";
                            int a;
                            int b;
                            int c;
                            switch (line)
                            {
                                case "HentAlle":                                 
                                    foreach (var item in Persistency.FanReadings)
                                    {
                                        result += item+", ";
                                    }
                                    sw.WriteLine(result);
                                    continue;
                                case "Hent":
                                    sw.WriteLine("Vælg ID:");
                                    var id = sr.ReadLine();
                                    if (int.TryParse(id, out a))
                                    {
                                        FanOutput temp = Persistency.FanReadings.Find(i => i.Id == Convert.ToInt32(id));
                                        if (temp != null)
                                        {
                                            sw.WriteLine(temp);
                                            continue;
                                        }
                                        else
                                        {
                                            sw.WriteLine("Ingen FanOutput svarer til det givede ID");
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        sw.WriteLine("Forkert ID Format");
                                        continue;
                                    }
                                case "Gem":
                                    while (true)
                                    {
                                        sw.WriteLine("Tilføj ny måling via følgende format: Id, Navn, Temp(15-25), Fugt(30-80)");
                                        string objToSave = sr.ReadLine();
                                        string[] items = objToSave.Split(", ");
                                        if (int.TryParse(items[0], out a) && int.TryParse(items[2], out b) && int.TryParse(items[3], out c))
                                        {
                                            try
                                            {
                                                FanOutput gemtobj = new FanOutput() { Id = Convert.ToInt32(items[0]), Name = items[1], Temp = Convert.ToInt32(items[2]), Humidity = Convert.ToInt32(items[3]) };
                                                Persistency.FanReadings.Add(gemtobj);
                                            }
                                            catch (NullReferenceException e)
                                            {
                                                sw.WriteLine(e.Message);
                                                continue;
                                            }
                                            catch (ArgumentOutOfRangeException e)
                                            {
                                                sw.WriteLine(e.ParamName);
                                                continue;
                                            }
                                            sw.WriteLine("Ny måling gemt");
                                            break;
                                        }
                                        else
                                        {
                                            sw.WriteLine("Forkert format, ex: 7, NAVN, TEMP, FUGT");
                                            continue;
                                        }
                                    }
                                    break;
                                default:
                                    sw.WriteLine("Command ikke fundet");
                                    continue;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        socket.Dispose();
                        Console.WriteLine("Client disconnected");
                        return;
                    }
                }
            }
        }

        



        public bool CanCalculate(string streamLine)
        {
            string _operator = streamLine.Split(' ')[0];
            double firstNo = 0;
            double secondNo = 0;
            try
            {
                firstNo = double.Parse(streamLine.Split(' ')[1], new CultureInfo("en-UK"));
                secondNo = double.Parse(streamLine.Split(' ')[2], new CultureInfo("en-UK"));
            }
            catch (Exception)
            {
                return false;
            }

            switch (_operator)
            {
                case "add":
                    Result = firstNo + secondNo;
                    _operator = "";
                    break;
                case "mul":
                    Result = firstNo * secondNo;
                    _operator = "";
                    break;
                case "sub":
                    Result = firstNo - secondNo;
                    _operator = "";
                    break;
                case "div":
                    Result = firstNo / secondNo;
                    _operator = "";
                    break;
                default:
                    return false;
            }
            return true;
        }
        public bool CanConvertToDateTime(string streamLine)
        {
            DateTime date = new DateTime();
            try
            {
                date = DateTime.ParseExact(streamLine, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }
    }
}
