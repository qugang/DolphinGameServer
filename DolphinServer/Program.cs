using DolphinDB.Redis;
using DolphinServer.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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

            RedisContext context = RedisContext.CreateRedisContext("localhost,allowAdmin=true", Assembly.GetAssembly(typeof(Program)));

            DateTime dt = DateTime.Now;

            //for (int i = 0; i < 100000; i++)
            //{
            //    GameUser user = new GameUser();
            //    user.UserID = Guid.NewGuid().ToString();
            //    user.UserName = "qugang";
            //    context.AddHashEntity(user);
            //}


            Console.WriteLine(DateTime.Now - dt);


            dt = DateTime.Now;
            foreach (var row in context.FindEntityAll<GameUser>())
            {

            }


            Console.WriteLine(DateTime.Now - dt);
            
        }
    }
}
