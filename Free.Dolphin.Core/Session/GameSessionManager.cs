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
        private static ConcurrentDictionary<string, GameSession> _users = new ConcurrentDictionary<string, GameSession>();

        public static GameSession AddSession(GameSession session)
        {
            if (!_sessions.ContainsKey(session.SocketClient))
            {
                _sessions.TryAdd(session.SocketClient, session);
            }
            return session;
        }

        public static GameSession GetSession(WebSocket socket)
        {
            return _sessions[socket];
        }

        public static GameSession AddSessionWithUser(GameSession session, string userId)
        {
            return _users.AddOrUpdate(userId, session, (oSession, nSession) =>
            {
                return nSession;
            });
        }

        public static GameSession RemoveSession(WebSocket socket)
        {
            GameSession session = null;
            GameSession userSession = null;
            _sessions.TryRemove(socket, out session);

            if (session.User != null)
            {
                _users.TryRemove(session.User.Uid, out userSession);
            }
            return session;
        }

        public static WebSocket GetWebSocketWithUser(string userId)
        {
            return _users[userId].SocketClient;
        }
    }
}
