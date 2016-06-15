using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using Free.Dolphin.Common;
using Free.Dolphin.Common.Util;
using Free.Dolphin.Core;
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
            RedisContext.InitRedisContext("localhost,allowAdmin=true", Assembly.GetAssembly(typeof(Program)));
            ControllerFactory.InitController(Assembly.GetAssembly(typeof(Program)));
            ControllerContext.InitGameUserType<GameUser>();
            WebSocketServer.Init("192.168.0.105", 9001);
            WebSocketServer.OnErrorMessage = (message, exption) =>
            {
                LogManager.Log.Error(message, exption);
                A9999DataErrorResponse.Builder response = A9999DataErrorResponse.CreateBuilder();
                response.ErrorCode = 999;
                response.ErrorInfo = "服务器繁忙!";
                List<byte> list = new List<byte>();
                list.Add((byte)(9999 >> 8));
                list.Add((byte)(9999 & 0xFF));
                list.AddRange(response.Build().ToByteArray());
                return list.ToArray();
            };

            WebSocketServer.OnRevice = (message) =>
            {
                LogManager.Log.Debug("收到消息:" + message);
            };

            WebSocketServer.OnSend = (message) =>
            {
                LogManager.Log.Debug("发送消息:" + ByteUtil.ByteToHex(message));
            };

            WebSocketServer.OnOpen = (message) =>
            {
                LogManager.Log.Debug("收到连接:" + message);
            };

            LogManager.Log.Info("服务器启动成功端口9001");
            Console.ReadKey();

        }
    }
}
