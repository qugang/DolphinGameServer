using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinServer.Service
{
    public static class CsGameRoomManager
    {
        static ConcurrentDictionary<int, CsMjGameRoom> rooms = new ConcurrentDictionary<int, CsMjGameRoom>();

        private static int maxRoomeId = 0;

        public static CsMjGameRoom CreateRoom(IGameUser user)
        {
            CsMjGameRoom room = new CsMjGameRoom();
            room.RoomId = getRoomId();
            room.players = new Queue<CsGamePlayer>();
            room.players.Enqueue(new CsGamePlayer(user));
            rooms.TryAdd(room.RoomId, room);
            if (room.players.All(p => p.IsReady))
            {
                room.ReLoad();
            }
            return room;
        }

        private static int getRoomId()
        {
            return Interlocked.Increment(ref maxRoomeId);
        }

        public static Boolean JoinRoom(IGameUser user, int roomID)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null)
            {
                room.players.Enqueue(new CsGamePlayer(user));

                if (room.players.All(p => p.IsReady))
                {
                    room.ReLoad();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static CsMjGameRoom Cancel(int roomID)
        {
            CsMjGameRoom room = null;
            rooms.TryRemove(roomID, out room);
            return room;
        }
    }
}
