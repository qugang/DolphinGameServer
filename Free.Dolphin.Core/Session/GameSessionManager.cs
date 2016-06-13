using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Free.Dolphin.Core
{
    public class GameSessionManager
    {
        private static ConcurrentDictionary<WebSocket, GameSession> _sessions = new ConcurrentDictionary<WebSocket, GameSession>();

        public static GameSession UpdateOrAddSession(WebSocket socket)
        {
            return _sessions.AddOrUpdate(socket, GameSession.Parse(socket), (sessionKey, gameSession) =>
             {
                 return gameSession;
             });
        }

        public static GameSession RemoveSession(WebSocket socket)
        {
            GameSession session = new GameSession();
            _sessions.TryRemove(socket, out session);
            return session;
        }
    }
}
