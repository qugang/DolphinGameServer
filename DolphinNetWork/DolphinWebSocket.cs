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
            return base.OnClose(e);
        }

        protected override Task OnError(ErrorEventArgs e)
        {
            return base.OnError(e);
        }

        protected override Task OnMessage(MessageEventArgs e)
        {
            return base.OnMessage(e);
        }

        protected override Task OnOpen()
        {
            return base.OnOpen();
        }
    }

    public class DolphinWebSocket
    {
        public void Init()
        {
            var wssv = new WebSocketServer(IPAddress.Parse("192.168.0.105"), 9001);
            wssv.AddWebSocketService<DefaultBehavior>("/");
            wssv.Start();
        }
    }
}
