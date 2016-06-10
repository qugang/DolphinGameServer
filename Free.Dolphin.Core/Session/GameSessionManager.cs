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
        private static ConcurrentDictionary<Guid, GameSession> _sessions = new ConcurrentDictionary<Guid, GameSession>();

        public static GameSession UpdateOrAddSession(Guid sessionId)
        {
            return _sessions.AddOrUpdate(sessionId, GameSession.Parse(sessionId), (sessionKey, gameSession) =>
             {
                 gameSession.SessionState = GameSessionState.OnLine;
                 return gameSession;
             });
        }

        public static GameSession RemoveSession(Guid sessionId)
        {
            GameSession session = new GameSession();
            _sessions.TryRemove(sessionId, out session);
            return session;
        }


    }
}
