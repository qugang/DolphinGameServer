using DolphinDB.Redis;
using DolphinNetWork;
using DolphinServer.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeisureComplexServer
{
    class Program
    {
        static void Main(string[] args)
        {
            
          
            ControllerFactory.InitController(Assembly.GetAssembly(typeof(Program)));
            WebSocketServer.Init("192.168.0.105", 9001);
            Console.ReadKey();

        }
    }
}
