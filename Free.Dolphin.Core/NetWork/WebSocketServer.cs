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
            return base.OnError(e);
        }

        protected override Task OnMessage(MessageEventArgs e)
        {
            Dictionary<string, string> keyValue = WebSocketPackage.UnPackage(e.Data);

            ControllerContext context = new ControllerContext(keyValue);

            GameSession session = GameSessionManager.UpdateOrAddSession(Context.WebSocket);
            context.Session = session;
            ControllerBase controller = ControllerFactory.CreateController(context);
            if (controller.IsAuth() && !controller.IsLogin())
            {
                //TODO : 没有登录处理
            }
            else
            {
                byte[] sendByte = controller.ProcessAction();
                List<byte> list = new List<byte>();
                list.Add((byte)(context.ProtocolNumber >> 8));
                list.Add((byte)(context.ProtocolNumber & 0xFF));
                list.AddRange(sendByte);
                Send(sendByte);
            }
            return base.OnMessage(e);
        }

        protected override Task OnOpen()
        {
            return base.OnOpen();
        }
    }

    public class WebSocketServer
    {
        public static void Init(string ip, int port)
        {
            var wssv = new WebSocketSharp.Server.WebSocketServer(IPAddress.Parse(ip), port);
            wssv.AddWebSocketService<DefaultBehavior>("/");
            wssv.Start();
        }
    }
}
