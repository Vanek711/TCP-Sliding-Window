using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace ExampleTcpClient
{
    class TCPClient
    {

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
            Thread.Sleep(1000);
            short port = 8080;
            var pathDirectory = Environment.CurrentDirectory;
            TcpClient client = new TcpClient(address, port);
            string messageSynchronization = "Synchronization . . . ";
            byte[] dataSynch = System.Text.Encoding.ASCII.GetBytes(messageSynchronization);
            NetworkStream stream = client.GetStream();
            stream.Write(dataSynch, 0, dataSynch.Length);
            Console.WriteLine("Sent: {0} ", messageSynchronization);
            dataSynch = new byte[1024];
            int bytes = stream.Read(dataSynch, 0, dataSynch.Length);
            string responseData = System.Text.Encoding.ASCII.GetString(dataSynch, 0, bytes); 
            if (responseData != "Synchronization accepted")
            {
                throw new Exception("No connection ");
            }
            Console.WriteLine("Received: {0}", responseData);

            string pathTextFile = Path.Combine(pathDirectory, @"Help\help1.doc");
            using(StreamReader reader = new StreamReader(pathTextFile))
            {
                string? text = await reader.ReadToEndAsync();
                if (String.IsNullOrEmpty(text)) { throw new Exception("File is Empty."); }
            }
            byte[] data = new byte[1024];
            data =  System.Text.Encoding.ASCII.GetBytes(text);//Из коннекта будет вызываться метод парсинга, а потом в коннекте цикл 
            char[] textArray = String.ToCharArray(text);
            for (int i =0; i < textArray.Length; i++)
            {
                stream.Write(textArray[i]);
                Console.WriteLine("Sent: {0} ", textArray[i]);
                if (i % 10 == 0) stream.Read(data,0,data.Length);

            }
            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
            // return 0;
        }
    }

}