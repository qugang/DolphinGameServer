using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    public static class CsGameRoomManager
    {
        static ConcurrentDictionary<int, CsMjGameRoom> rooms = new ConcurrentDictionary<int, CsMjGameRoom>();

        private static int maxRoomeId = 0;

        public static CsMjGameRoom CreateRoom(GameUser user)
        {
            CsMjGameRoom room = new CsMjGameRoom();
            room.RoomId = getRoomId();
            room.Players = new LinkedList<CsGamePlayer>();
            room.Players.AddLast(new CsGamePlayer(user));
            rooms.TryAdd(room.RoomId, room);
            return room;
        }

        public static void Ready(int roomId, GameUser user)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomId, out room);
            room.BeginGame(user.Uid);
        }

        private static int getRoomId()
        {
            return Interlocked.Increment(ref maxRoomeId);
        }

        public static Boolean JoinRoom(GameUser user, int roomID)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null)
            {
                room.Players.AddLast(new CsGamePlayer(user));

                if (room.Players.All(p => p.IsReady))
                {

                    //TODO: 重置房间号
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
