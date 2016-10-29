using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
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
        static ConcurrentBag<int> roomRemoveKeyBag = new ConcurrentBag<int>();
        static ConcurrentDictionary<string, CsMjGameRoom> userRooms = new ConcurrentDictionary<string, CsMjGameRoom>();

        private static int maxRoomeId = 0;

        public static CsMjGameRoom CreateRoom(GameUser user,int RoomPeopleType)
        {
            if (RoomPeopleType == 0)
            {
                CsMjGameRoom room = new CsMjGameRoom(user.Uid);
                room.RoomId = getRoomId();
                room.Players = new LinkedList<CsGamePlayer>();
                room.Players.AddLast(new CsGamePlayer(user));
                rooms.TryAdd(room.RoomId, room);

                userRooms.TryAdd(user.Uid, room);
                return room;
            }
            else
            {
                CsGameRoomThree room = new CsGameRoomThree(user.Uid);
                room.RoomId = getRoomId();
                room.Players = new LinkedList<CsGamePlayer>();
                room.Players.AddLast(new CsGamePlayer(user));
                rooms.TryAdd(room.RoomId, room);
                userRooms.TryAdd(user.Uid, room);
                return room;
            }
        }

        public static CsMjGameRoom Ready(int roomId, GameUser user)
        {
            CsMjGameRoom room = null;
            rooms.TryGetValue(roomId, out room);
            room.ReadyGame(user.Uid);
            return room;
        }

        private static int getRoomId()
        {
            int item;
            if (roomRemoveKeyBag.TryTake(out item))
            {
                return item;
            }
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



            if (room != null && (room.Players.Count < 4 && room.RoomPeoPleType == 0))
            {
                room.JoinRoom(user);

                userRooms.TryAdd(user.Uid, room);

                return room;
            }
            else if (room != null && (room.Players.Count < 3 && room.RoomPeoPleType == 1))
            {
                room.JoinRoom(user);

                userRooms.TryAdd(user.Uid, room);

                return room;
            }
            else
            {
                return null;
            }
        }

        public static CsMjGameRoom Cancel(string uid,int roomID,int cancelType)
        {
            CsMjGameRoom room = GetRoomById(roomID);

            if (room != null)
            {
                LinkedListNode<CsGamePlayer> player = room.FindPlayer(uid);

                if (cancelType == 0)
                {
                    player.Value.Cancel = false;
                }
                else
                {
                    player.Value.Cancel = true;
                }
                player.Value.CancelState = true;
            }

            var listPlayer = room.Players.ToList();

            int count = listPlayer.Count(p => p.Cancel == true);

            int cancelStateCount = listPlayer.Count(p => p.CancelState == true);

            //Cancel类型0为通知，1为解散房间，2为不同意取消
            int isCancel = 0;

            if (count > listPlayer.Count() / 2 && cancelStateCount > listPlayer.Count() /2)
            {
                rooms.TryRemove(roomID, out room);
                roomRemoveKeyBag.Add(roomID);

                foreach (var row in room.Players)
                {
                    CsMjGameRoom tempRoom = null;
                    userRooms.TryRemove(row.PlayerUser.Uid, out tempRoom);
                    row.Cancel = false;
                    row.CancelState = false;
                }
                isCancel = 1;
            }
            else if (count < listPlayer.Count() / 2 && cancelStateCount > listPlayer.Count() / 2)
            {
                isCancel = 2;
                foreach (var row in room.Players)
                {
                    row.Cancel = false;
                    row.CancelState = false;
                }
            }

            A1004Response.Builder response = A1004Response.CreateBuilder();

            foreach (var row in listPlayer.FindAll(p => p.CancelState == true))
            {
                A1004User.Builder user = A1004User.CreateBuilder();
                user.SetUid(row.PlayerUser.Uid);
                user.SetIsCancel(row.Cancel ? 1 : 0);
                user.Name = row.PlayerUser.NickName;
                response.AddUsers(user);
            }
            response.SetCancelType(isCancel);

            byte[] responseArray = response.Build().ToByteArray();
            
            foreach (var row in room.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1004, responseArray);
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
