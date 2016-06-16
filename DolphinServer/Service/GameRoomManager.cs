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
    public static class GameRoomManager
    {
        static ConcurrentDictionary<int, GameRoom> rooms = new ConcurrentDictionary<int, GameRoom>();

        private static int maxRoomeId = 0;

        public static GameRoom CreateRoom(GameSession user)
        {
            GameRoom room = new GameRoom();
            room.RoomId = getRoomId();
            room.players = new List<GameSession>();
            room.players.Add(user);
            rooms.TryAdd(room.RoomId, room);
            return room;
        }

        private static int getRoomId()
        {
            return Interlocked.Increment(ref maxRoomeId);
        }

        public static Boolean JoinRoom(GameSession user, int roomID)
        {
            GameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null)
            {
                room.players.Add(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static GameRoom Cancel(int roomID)
        {
            GameRoom room = null;
            rooms.TryRemove(roomID, out room);
            return room;
        }

        public static GameRoom Exit(int roomID, GameSession userID)
        {
            GameRoom room = null;
            rooms.TryGetValue(roomID, out room);

            if (room == null)
            {
                return room;
            }
            else
            {
                room.players.Remove(userID);
            }
            return room;
        }
    }
}
