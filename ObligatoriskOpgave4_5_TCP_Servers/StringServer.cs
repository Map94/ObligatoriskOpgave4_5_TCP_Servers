////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Net;
////using System.Net.Sockets;
////using System.Text;
////using System.Threading.Tasks;

////namespace ObligatoriskOpgave4_5_TCP_Servers
////{
////    public class StringServer
////    {

////        public int port = 8888;

////        public void run()
////        {
////            Console.WriteLine("TCP Server String");

////            TcpListener listener = new TcpListener(IPAddress.Any, port);

////            listener.Start();

////            while (true)
////            {
////                TcpClient socket = listener.AcceptTcpClient();
////                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
////                Console.WriteLine("Client is connected: " + clientEndPoint.Address);

////                Task.Run(() => HandleClient(socket));
////            }

////            listener.Stop();
////        }

////        void HandleClient(TcpClient socket)
////        {
////            NetworkStream ns = socket.GetStream();
////            StreamReader reader = new StreamReader(ns);
////            StreamWriter writer = new StreamWriter(ns);

////            while (socket.Connected)
////            {
////                // Client Input
////                string? command = reader.ReadLine();

////                if (command == null)
////                {
////                    writer.Flush();
////                }


////                if (command == "Random" || command == "Add" || command == "Subtract")
////                {
////                    Console.WriteLine($"Command: {command}");

////                    writer.WriteLine("Input numbers");
////                    writer.Flush();

////                    // Expected Client Input number1 space  number2 
////                    string? commandResponse = reader.ReadLine();
////                    string[]? commandParts = commandResponse.Split(' ');

////                    // Parse Client Input is Int 
////                    int number1 = int.Parse(commandParts[0]);
////                    int number2 = int.Parse(commandParts[1]);



////                    // For debugging in console
////                    Console.WriteLine("Number 1: " + number1 + " Number 2: " + number2); 

////                    string result = "";

////                    switch (command)
////                    {
////                        case "Random":

////                            Random rand = new Random();
////                            result = rand.Next(number1, number2).ToString();

////                            break;

////                        case "Add":

////                            result = (number1 + number2).ToString();

////                            break;

////                        case "Subtract":

////                            result = (number1 - number2).ToString();

////                            break;

////                        default:
////                            break;
////                    }

////                    Console.WriteLine($"Command: {command} Result: {result}"); 

////                    // Result
////                    writer.WriteLine(result);
////                    writer.Flush();
////                }

////                writer.Flush();

////                if (command == "stop")
////                {
////                    socket.Close();
////                }
////            }
////        }

////    }
////}
//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.IO;
//using System.Threading.Tasks;

//namespace ObligatoriskOpgave4_5_TCP_Servers
//{
//    public class StringServer
//    {
//        public int port = 8888;

//        public void run()
//        {
//            Console.WriteLine("TCP Server String");
//            TcpListener listener = new TcpListener(IPAddress.Any, port);
//            listener.Start();

//            while (true)
//            {
//                TcpClient socket = listener.AcceptTcpClient();
//                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
//                Console.WriteLine("Client is connected: " + clientEndPoint.Address);

//                Task.Run(() => HandleClient(socket));
//            }

//            listener.Stop();
//        }

//        void HandleClient(TcpClient socket)
//        {
//            using (NetworkStream ns = socket.GetStream())
//            using (StreamReader reader = new StreamReader(ns))
//            using (StreamWriter writer = new StreamWriter(ns) { AutoFlush = true })
//            {
//                while (true)
//                {
//                    try
//                    {
//                        // Client Input
//                        string command = reader.ReadLine();
//                        if (command == null) break; // Client disconnected

//                        Console.WriteLine($"Command: {command}");

//                        if (command == "Random" || command == "Add" || command == "Subtract")
//                        {
//                            writer.WriteLine("Input numbers");

//                            // Expected Client Input number1 space number2 
//                            string commandResponse = reader.ReadLine();
//                            if (commandResponse == null) break; // Client disconnected

//                            string[] commandParts = commandResponse.Split(' ');
//                            if (commandParts.Length < 2) continue; // Handle error for invalid input

//                            // Parse Client Input as Int
//                            if (int.TryParse(commandParts[0], out int number1) &&
//                                int.TryParse(commandParts[1], out int number2))
//                            {
//                                string result = "";

//                                switch (command)
//                                {
//                                    case "Random":
//                                        Random rand = new Random();
//                                        result = rand.Next(number1, number2 + 1).ToString(); // Include number2
//                                        break;
//                                    case "Add":
//                                        result = (number1 + number2).ToString();
//                                        break;
//                                    case "Subtract":
//                                        result = (number1 - number2).ToString();
//                                        break;
//                                }

//                                Console.WriteLine($"Command: {command} Result: {result}");
//                                writer.WriteLine(result);
//                            }
//                            else
//                            {
//                                writer.WriteLine("Invalid numbers");
//                            }
//                        }

//                        if (command == "stop") break; // Stop the client session
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"Error: {ex.Message}");
//                        break; // Exit on error
//                    }
//                }

//                // Clean up
//                socket.Close();
//                Console.WriteLine("Client disconnected.");
//            }
//        }
//    }
//}
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace ObligatoriskOpgave4_5_TCP_Servers
{
    public class StringServer
    {
        public int port = 8888;

        public void run()
        {
            Console.WriteLine("TCP Server String");
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client connected: " + clientEndPoint.Address);
                Task.Run(() => HandleClient(socket));
            }

            listener.Stop();
        }

        void HandleClient(TcpClient socket)
        {
            using (NetworkStream ns = socket.GetStream())
            using (StreamReader reader = new StreamReader(ns))
            using (StreamWriter writer = new StreamWriter(ns) { AutoFlush = true })
            {
                while (true)
                {
                    try
                    {
                        // Read command from client
                        string command = reader.ReadLine();
                        if (command == null) break; // Client disconnected

                        Console.WriteLine($"Command: {command}");

                        // Handle valid commands
                        if (command == "Random" || command == "Add" || command == "Subtract")
                        {
                            writer.WriteLine("Input numbers");

                            // Read the numbers from client
                            string commandResponse = reader.ReadLine();
                            if (commandResponse == null) break; // Client disconnected

                            string[] commandParts = commandResponse.Split(' ');
                            if (commandParts.Length < 2) continue; // Handle error for invalid input

                            // Try to parse the input numbers
                            if (int.TryParse(commandParts[0], out int number1) &&
                                int.TryParse(commandParts[1], out int number2))
                            {
                                string result = "";

                                // Perform calculations based on the command
                                switch (command)
                                {
                                    case "Random":
                                        Random rand = new Random();
                                        result = rand.Next(number1, number2 + 1).ToString(); // Include number2
                                        break;
                                    case "Add":
                                        result = (number1 + number2).ToString();
                                        break;
                                    case "Subtract":
                                        result = (number1 - number2).ToString();
                                        break;
                                }

                                Console.WriteLine($"Command: {command} Result: {result}");
                                writer.WriteLine(result);
                            }
                            else
                            {
                                writer.WriteLine("Invalid numbers");
                            }
                        }

                        if (command == "stop") break; // Stop the client session
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        break; // Exit on error
                    }
                }

                // Clean up
                socket.Close();
                Console.WriteLine("Client disconnected.");
            }
        }
    }
}


