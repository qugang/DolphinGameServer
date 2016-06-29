using Fleck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public class GameSessionManager
    {
        private static ConcurrentDictionary<IWebSocketConnection, GameSession> _sessions = new ConcurrentDictionary<IWebSocketConnection, GameSession>();

        public static GameSession AddSession(GameSession session)
        {
            if (!_sessions.ContainsKey(session.SocketClient))
            {
                _sessions.TryAdd(session.SocketClient, session);
            }
            return session;
        }

        public static GameSession GetSession(IWebSocketConnection socket)
        {
            return _sessions[socket];
        }
        

        public static GameSession RemoveSession(IWebSocketConnection socket)
        {
            GameSession session = null;
            _sessions.TryRemove(socket, out session);
            return session;
        }
        
    }
}
