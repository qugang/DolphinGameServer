using System;
using System.Collections.Generic;
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
            Send("HelloWorld");
            return base.OnMessage(e);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer(IPAddress.Parse("127.0.0.1"),9333);
            wssv.AddWebSocketService<Laputa>("/Laputa");
            wssv.Start();
            Console.WriteLine("启动成功");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
