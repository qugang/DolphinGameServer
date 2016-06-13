using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service
{
    public static class GameRoomManager
    {
        static ConcurrentDictionary<int, GameRoom> rooms = new ConcurrentDictionary<int, GameRoom>();

        public static void CreateRoom(GameSession user)
        {
            int roomId = rooms.Max(p => p.Key);

            GameRoom room = new GameRoom();
            room.RoomId = roomId;
            room.UserID.Add(user);
        }

        public static GameRoom JoinRoom(GameSession user, int roomID)
        {
            GameRoom room = null;
            rooms.TryGetValue(roomID, out room);
            if (room != null)
            {
                room.UserID.Add(user);
                return room;
            }
            else
            {
                return null;
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
                room.UserID.Remove(userID);
            }
            return room;
        }
    }
}
