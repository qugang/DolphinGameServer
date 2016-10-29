using Fleck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core.Session
{
    public class GameUserManager
    {
        private static ConcurrentDictionary<string, GameSession> _Users = new ConcurrentDictionary<string, GameSession>();

        public static void AddOrUpdateUser(string userId, GameSession session)
        {
            _Users.AddOrUpdate(userId, session, (key, oSession) =>
            {
                oSession.SocketClient.Close();
                return session;
            });
        }


        public static IWebSocketConnection GetUserWebSocket(string userId)
        {
            GameSession session = null;
            _Users.TryGetValue(userId, out session);

            if (session == null)
                return null;

            return session.SocketClient;
        }

        public static void SendPackgeWithUser(string userId, byte[] list)
        {
            IWebSocketConnection client = GetUserWebSocket(userId);
            if (client != null)
            {
                client.Send(list);
            }
        }
    }
}
