using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using FootBall;

namespace Opgave_5
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SKYNET ONLINE");
            //IPAddress localAddr = IPAddress.Parse("192.168.104.145");
            //TcpListener listener = new TcpListener(localAddr, 2121);
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);

            listener.Start();
            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();

                Task.Run(() => { HandleClient(socket); }
                );
            }
        }

        public static List<FootBallPlayer> Flist = new List<FootBallPlayer>()
        {
            new FootBallPlayer(1, "Aleksander", 212, 23)
        };

        public static object HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            Console.WriteLine("Human being detected");
            while (true)
            {
                string message = reader.ReadLine();
                string message1 = reader.ReadLine();
                Console.WriteLine("Request " + message);


                //if (message.Contains("HentAlle"))
                //{
                //    foreach (FootBallPlayer footBallPlayer in Flist)
                //    {
                //        writer.WriteLine();
                //        writer.WriteLine($" Id: {footBallPlayer.Id} Name: {footBallPlayer.Name} ShirtNumber {footBallPlayer.ShirtNumber} Price {footBallPlayer.Price}");
                //    }

                //    writer.Flush();

                //}

                if (message.Contains("HentAlle"))
                {
                    string jsonString = JsonSerializer.Serialize(Flist);
                    writer.WriteLine();
                    writer.WriteLine(jsonString);

                    writer.Flush();
                }

                else if (message.ToLower().StartsWith("id"))
                {
                    writer.WriteLine("Pick an id please");
                    writer.Flush();
                    message = reader.ReadLine();
                    writer.WriteLine("id pick");
                    writer.Flush();
                    int number = int.Parse(message);
                    var result = Flist.Find(s => s.Id.Equals(number));

                    writer.WriteLine($" Id: {result.Id} - Name: {result.Name} - ShirtNumber {result.ShirtNumber} - Price {result.Price}");

                    writer.Flush();

                    writer.WriteLine();

                }

                else if (message.Contains("Gem"))
                {
                    FootBallPlayer FromJson = JsonSerializer.Deserialize<FootBallPlayer>(message1);
                    Flist.Add(FromJson);
                    writer.Flush();
                }

               


            }

        }

    }
}
    
