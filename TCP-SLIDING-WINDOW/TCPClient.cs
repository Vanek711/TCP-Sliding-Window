using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;

namespace ExampleTcpClient
{
    class TCPClient
    {
        private static int port = 2222; 
        static void Main(string[] args)
        {
            try
            {
                string addressName = Dns.GetHostName();
                Connect("192.168.1.83",addressName);
                Console.WriteLine("\n Press Enter to continue...");
                Console.Read();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("ArgumentOutOfRangeException: {0}", e);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch
            {
                Console.WriteLine("Exeption");
            }
        }
        
        static async void Connect(string address,string addressName)
        {
            string sData ="";
            Console.WriteLine("Synchronization . . . ");
            string messageSynchronization = "SYN";
            Byte[] data = new byte[1024];

            TcpClient client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();

            byte[] dataSynch = Encoding.ASCII.GetBytes(messageSynchronization);

            stream.Write(dataSynch, 0, dataSynch.Length);
            Console.WriteLine("Sent: {0} ", messageSynchronization);


            _ = stream.ReadAsync(data, 0, data.Length);

            string responseData = Encoding.ASCII.GetString(data, 0, data.Length);

            if (responseData != "Synchronization accepted")
            {
                Console.WriteLine("Connection Failed"); 
                Environment.Exit(0);
            }

            for (int i = 0; i < 2000; i++)
            {
                char[] message = new char[1];
                message[0] = (char)i;
                stream.Write(Encoding.ASCII.GetBytes(message), 0, 1);
                await data = stream.ReadAsync(data, 0, data.Length);
                sData = Encoding.ASCII.GetString(data, 0, data.Length);
                if (sData == "Error") i--;
            }

            stream.Close();
            client.Close();
            // return 0;
        }
    }

}
