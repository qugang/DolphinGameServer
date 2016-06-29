using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Free.Dolphin.Core
{
    public class GameSession
    {
        internal GameSession() {
        }

        public IGameUser User { get; set; }

        public IWebSocketConnection SocketClient { get; set; }

        public static GameSession Parse(IWebSocketConnection socket)
        {
            GameSession session = new GameSession();
            session.SocketClient = socket;
            session.User = null;
            return session;
        }
    }
}
