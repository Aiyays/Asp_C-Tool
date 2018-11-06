using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bill_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer socketServer = new SocketServer("127.0.0.1");
            socketServer.Listenin(1000);
            Console.ReadLine();
        }
    }
}
