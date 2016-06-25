using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Free.Dolphin.Core.Session
{
    public class GameUserManager
    {
        private static ConcurrentDictionary<string, GameSession> _Users = new ConcurrentDictionary<string, GameSession>();

        public static void AddOrUpdateUser(string userId,GameSession session)
        {
            _Users.AddOrUpdate(userId, session, (key, oSession) => {
                return session;
            });
        }
        

        public static WebSocket GetUserWebSocket(string userId)
        {
            GameSession session = null;
            _Users.TryGetValue(userId, out session);
            if (session.SocketClient.ReadyState == WebSocketSharp.WebSocketState.Connecting ||
                session.SocketClient.ReadyState == WebSocketState.Open)
            {
                return session.SocketClient;
            }
            return null;
        }

        public static void SendPackgeWithUser(string userId,byte[] list)
        {
            WebSocket client = GetUserWebSocket(userId);
            if (client != null)
            {
                client.Send(list);
            }
        }
    }
}
