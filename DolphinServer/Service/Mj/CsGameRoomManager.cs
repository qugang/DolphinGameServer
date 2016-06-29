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

        public static CsMjGameRoom Ready(int roomId, GameUser user)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomId, out room);
            room.BeginGame(user.Uid);
            return room;
        }

        private static int getRoomId()
        {
            return Interlocked.Increment(ref maxRoomeId);
        }

        public static CsMjGameRoom GetRoomById(int roomId) {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomId, out room);
            return room;
        }

        public static CsMjGameRoom JoinRoom(GameUser user, int roomID)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null)
            {
                room.JoinRoom(user);
                return room;
            }
            else
            {
                return room;
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
