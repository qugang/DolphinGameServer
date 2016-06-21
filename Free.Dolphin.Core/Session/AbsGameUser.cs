using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public abstract class AbsGameUser
    {
        public abstract string Uid { get; set; }
        public abstract string Pwd { get; set; }
        public abstract Guid Sid { get; set; }
        public abstract DateTime OnlimeDate { get; set; }
        public abstract GameUserState UserState { get; set; }

        public GameSession CurrentSession { get; set; }

        public abstract String Name { get; set; }

        public abstract AbsGameUser Login(string uid, string pwd);

        public WebSocketSharp.WebSocket GetSocket()
        {
            if (this.CurrentSession != null &&
                (this.CurrentSession.SocketClient.ReadyState == WebSocketSharp.WebSocketState.Connecting ||
                 this.CurrentSession.SocketClient.ReadyState == WebSocketSharp.WebSocketState.Open))
            {
                return this.CurrentSession.SocketClient;
            }
            return null;
        }

    }
}
