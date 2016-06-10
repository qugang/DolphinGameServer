using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DolphinNetWork
{

    class DefaultBehavior : WebSocketBehavior
    {
        protected override Task OnClose(CloseEventArgs e)
        {

            //TODO: 客户端断开连接时如何处理Session
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

            GameSession session = GameSessionManager.UpdateOrAddSession(context.Sid);

            ControllerFactory.CreateController(context).ProcessAction();
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
