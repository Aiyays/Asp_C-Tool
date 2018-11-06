using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bill_Server
{
    class SocketClient
    {
        private static byte[] result = new byte[1024];
        private static Socket clientSocket;
        private static void ListenServer()
        {
            while (true)
            {
                result = new byte[1024];
                int receiveLength = clientSocket.Receive(result);

                Console.WriteLine("{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            }

        }
        static void Main(string[] args)
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(ip, 8885);
                Console.WriteLine("连接成功！");

            }
            catch (Exception e)
            {
                Console.WriteLine("连接失败！");
                return;
            }
            Thread threadRead = new Thread(ListenServer);
            threadRead.Start();


            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    string sendMessage = Console.ReadLine();
                    clientSocket.Send(Encoding.ASCII.GetBytes("Sylvia:" + sendMessage));

                }
                catch (Exception ex)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }

            }
            Console.WriteLine("发送完毕 按回车退出");
            Console.ReadKey();
        }
    }
}
