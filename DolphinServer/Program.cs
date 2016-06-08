using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LeisureComplexServer
{
    public class Laputa : WebSocketBehavior
    {
        protected override Task OnMessage(MessageEventArgs e)
        {

            // Send("HelloWorld");
            Console.WriteLine("收到连接");

            StreamReader reader = new StreamReader(e.Data);

            Console.WriteLine(reader.ReadToEnd());

            return base.OnMessage(e);
        }


        protected override Task OnOpen()
        {
            Console.WriteLine(this.Id);
            Console.WriteLine("Open");
            return base.OnOpen();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer(IPAddress.Parse("192.168.0.105"), 9001);
           
            wssv.AddWebSocketService<Laputa>("/Laputa");
            wssv.Start();
            Console.WriteLine("启动成功");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
