using Free.Dolphin.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Free.Dolphin.Core
{

    class DefaultBehavior : WebSocketBehavior
    {
        protected override Task OnClose(CloseEventArgs e)
        {
            GameSessionManager.RemoveSession(Context.WebSocket);

            return base.OnClose(e);
        }

        protected override Task OnError(ErrorEventArgs e)
        {
            byte[] array =  WebSocketServer.OnErrorMessage(e.Message, e.Exception);
            List<byte> list = new List<byte>();
            list.Add((byte)(9999 >> 8));
            list.Add((byte)(9999 & 0xFF));
            list.AddRange(array);
            Send(list.ToArray());
            return base.OnError(e);
        }

        protected override Task OnMessage(MessageEventArgs e)
        {
            try
            {
                string message = StreamUtil.ReadStringToEnd(e.Data);
                WebSocketServer.OnRevice(message);
                if (message == "ping")
                {
                    return base.OnMessage(e);
                }
                Dictionary<string, string> keyValue = WebSocketPackage.UnPackage(message);
                ControllerContext context = new ControllerContext(keyValue);
                context.Session = GameSessionManager.GetSession(Context.WebSocket);
                ControllerBase controller = ControllerFactory.CreateController(context);
                if (controller.IsAuth() && !controller.IsLogin())
                {
                    if (controller.Login())
                    {
                        byte[] sendByte = controller.ProcessAction();
                        List<byte> list = new List<byte>();
                        list.Add((byte)(context.ProtocolId >> 8));
                        list.Add((byte)(context.ProtocolId & 0xFF));
                        list.AddRange(sendByte);
                        WebSocketServer.OnSend(list.ToArray());
                        Send(list.ToArray());
                    }
                    else
                    {
                        Error("断线重登处理失败", new Exception("断线重登处理失败"));
                    }
                }
                else
                {
                    byte[] sendByte = controller.ProcessAction();
                    List<byte> list = new List<byte>();
                    list.Add((byte)(context.ProtocolId >> 8));
                    list.Add((byte)(context.ProtocolId & 0xFF));
                    list.AddRange(sendByte);
                    WebSocketServer.OnSend(list.ToArray());
                    Send(list.ToArray());
                }
            }
            catch (Exception ex)
            {
                Error("处理请求出错", ex);
            }
            return base.OnMessage(e);
        }

        protected override Task OnOpen()
        {
            GameSessionManager.AddSession(GameSession.Parse(Context.WebSocket));
            WebSocketServer.OnOpen(Context.UserEndPoint.ToString());
            return base.OnOpen();
        }
    }

    public class WebSocketServer
    {

        public static Func<string,Exception,byte[]> OnErrorMessage { get; set; }

        public static Action<string> OnRevice { get; set; }

        public static Action<Byte[]> OnSend { get; set; }

        public static Action<string> OnOpen { get; set; }
        public static void Init(string ip, int port)
        {
            var wssv = new WebSocketSharp.Server.WebSocketServer(IPAddress.Parse(ip), port);
            wssv.AddWebSocketService<DefaultBehavior>("/");
            wssv.Start();
        }
    }
}
