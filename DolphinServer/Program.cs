using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service.Mj;
using Free.Dolphin.Core;
using Free.Dolphin.Core.Util;
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
using System.Configuration;
using Free.Dolphin.Core.MQ;
using DolphinServer.MQMessage;
using Free.Dolphin.Common;

namespace LeisureComplexServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisContext.InitRedisContext("localhost,allowAdmin=true", Assembly.GetAssembly(typeof(Program)));
            ControllerFactory.InitController(Assembly.GetAssembly(typeof(Program)));
            ControllerBase.InitGameUserType<GameUser>();
            WebSocketServerWrappe.Init(ConfigurationManager.AppSettings["ip"], int.Parse(ConfigurationManager.AppSettings["port"]));
            WebSocketServerWrappe.OnErrorMessage = (message, exption) =>
            {
                LogManager.Log.Error(message, exption);
                A9999DataErrorResponse.Builder response = A9999DataErrorResponse.CreateBuilder();
                response.ErrorCode = 9999;
                response.ErrorInfo = "服务器繁忙!";
                return response.Build().ToByteArray();
            };

            WebSocketServerWrappe.OnRevice = (message) =>
            {
                LogManager.Log.Debug("收到消息:" + message);
            };

            WebSocketServerWrappe.OnSend = (message) =>
            {
                LogManager.Log.Debug("发送消息:" + ByteUtil.ByteToHex(message));
            };

            WebSocketServerWrappe.OnOpen = (message) =>
            {
                LogManager.Log.Debug("收到连接:" + message);
            };

            WebSocketServerWrappe.OnClose = (message, user) =>
            {
                LogManager.Log.Debug("客户端断开:" + message);
                if (user != null)
                {

                    CsMjGameRoom room = CsGameRoomManager.GetRoomByUserId(user.Uid);

                    if (room != null)
                    {
                        A9998ClientCloseResponse.Builder response = A9998ClientCloseResponse.CreateBuilder();
                        response.Uid = user.Uid;

                        var responseArray = response.Build().ToByteArray();

                        foreach (var row in room.Players)
                        {
                            WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 9998, responseArray);
                        }
                    }
                }
            };

            WebSocketServerWrappe.DesKey = "f1u2c3k4";
            WebSocketServerWrappe.DesIv = "N1Q2J3C4";
            WebSocketServerWrappe.SignKey = "3f261d4f2f8941eafffffcf7507f021b";


            MQServer.RunMQ((object o) =>
            {
                SerializerUtil.JavaScriptJosnDeserialize<AddUserRoomCard>(o.ToString()).Process();
            });

            LogManager.Log.Info("服务器启动成功端口9001");



            Console.ReadKey();

        }
    }
}
