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
            room.players = new LinkedList<CsGamePlayer>();
            room.players.AddLast(new CsGamePlayer(user));
            rooms.TryAdd(room.RoomId, room);
            room.BeginGame();
            return room;
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
                room.players.AddLast(new CsGamePlayer(user));

                if (room.players.All(p => p.IsReady))
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
