using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ObligatoriskOpgave4_5_TCP_Servers
{
    public class JsonServer
    {
        public int port = 8888;

        public void run()
        {
            Console.WriteLine("TCP Server Json");

            TcpListener listener = new TcpListener(IPAddress.Any, 8888);

            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client is connected: " + clientEndPoint.Address);

                Task.Run(() => HandleClient(socket));
            }

            listener.Stop();
        }

        void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            while (socket.Connected)
            {
                
                string? jsonString = reader.ReadLine();

                try
                {
                    JsonCommand commandObject = JsonSerializer.Deserialize<JsonCommand>(jsonString);

                    string command = commandObject.Method;

                    Console.WriteLine($"Method: {commandObject.Method}, Tal1: {commandObject.Number1}, Tal2: {commandObject.Number2}"); // Use while dev

                    // Is Client Input a Command?
                    if (command == "Random" || command == "Add" || command == "Subtract")
                    {
                        Console.WriteLine($"Command: {command}"); // Use while developing

                        // Parse Client Input is Int 
                        int number1 = commandObject.Number1;
                        int number2 = commandObject.Number2;

                        // For debugging in console
                        Console.WriteLine("Number 1: " + number1 + " Number 2: " + number2);

                        string result = "";

                        switch (command)
                        {
                            case "Random":

                                Random rand = new Random();
                                result = rand.Next(number1, number2).ToString();

                                break;
                            case "Add":

                                result = (number1 + number2).ToString();

                                break;
                            case "Subtract":

                                result = (number1 - number2).ToString();

                                break;
                            default:


                                break;
                        }

                        Console.WriteLine($"Command: {command} Result: {result}");

                        //Result
                        writer.WriteLine(result);
                        writer.Flush();
                    }

                    if (command == "stop")
                    {
                        socket.Close();
                    }

                }
                catch (JsonException ex)
                {
                    writer.WriteLine(ex.Message);
                    writer.Flush();
                }


            }
        }



    }
}
