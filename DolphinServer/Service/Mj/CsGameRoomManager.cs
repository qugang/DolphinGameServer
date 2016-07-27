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

        static ConcurrentDictionary<string, CsMjGameRoom> userRooms = new ConcurrentDictionary<string, CsMjGameRoom>();

        private static int maxRoomeId = 0;

        public static CsMjGameRoom CreateRoom(GameUser user,int jushu)
        {
            CsMjGameRoom room = new CsMjGameRoom(jushu);
            room.RoomId = getRoomId();
            room.Players = new LinkedList<CsGamePlayer>();
            room.Players.AddLast(new CsGamePlayer(user));
            rooms.TryAdd(room.RoomId, room);

            userRooms.TryAdd(user.Uid, room);
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

        public static CsMjGameRoom GetRoomById(int roomId)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomId, out room);
            return room;
        }

        public static CsMjGameRoom JoinRoom(GameUser user, int roomID)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null && room.Players.Count < 4)
            {
                room.JoinRoom(user);

                userRooms.TryAdd(user.Uid, room);

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

            foreach (var row in room.Players)
            {
                CsMjGameRoom removeRoom = null;
                userRooms.TryRemove(row.PlayerUser.Uid, out removeRoom);
            }

            return room;
        }

        public static CsMjGameRoom GetRoomByUserId(string userId)
        {
            CsMjGameRoom room = null;
            userRooms.TryGetValue(userId, out room);
            return room;
        }
    }
}
