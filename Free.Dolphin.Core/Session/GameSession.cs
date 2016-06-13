using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Free.Dolphin.Core
{
    public class GameSession
    {
        internal GameSession() {
        }

        public IGameUser User { get; set; }

        public WebSocket SocketClient { get; set; }

        public static GameSession Parse(WebSocket socket)
        {
            GameSession session = new GameSession();
            session.SocketClient = socket;
            session.User = null;
            return session;
        }
    }
}
