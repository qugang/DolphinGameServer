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
        public Guid SessionId { get; set; }

        public IGameUser User { get; set; }

        public WebSocket SocketClient { get; set; }

        public GameSessionState SessionState { get; set; }

        internal static GameSession Parse(Guid session)
        {
            return new GameSession { SessionId = session, SessionState = GameSessionState.OnLine };
        }


    }
}
