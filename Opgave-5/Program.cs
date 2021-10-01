using FootBall;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

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

                Task.Run(() =>
                {
                    HandleClient(socket);
                }
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
                Console.WriteLine("Request " + message);


                if (message.Contains("Hent Alle"))
                {
                    foreach (FootBallPlayer footBallPlayer in Flist)
                    {
                        writer.WriteLine();
                        writer.WriteLine($" Id: {footBallPlayer.Id} Name: { footBallPlayer.Name} ShirtNumber {footBallPlayer.ShirtNumber} Price {footBallPlayer.Price}");
                        writer.WriteLine(footBallPlayer);                   
                    }

                    writer.Flush();

                }

                else if (message.Contains("Gem"))
                {
                    FootBallPlayer FromJson = JsonSerializer.Deserialize<FootBallPlayer>(message);

                    
                }

            }
           
        }
        //public static List<FootBallPlayer> footBallPlayers;

      

    }
    }
