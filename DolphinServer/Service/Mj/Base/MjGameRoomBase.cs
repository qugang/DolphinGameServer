using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    public abstract class MjGameRoomBase
    {
        public MjGameRoomBase() {
            this.JuShu = 1;
        }
        public int RoomId { get; set; }

        public int RoomType { get; set; }

        /// <summary>
        /// 局数
        /// </summary>
        public int JuShu { get; set; }
        protected SpinLock roomLock = new SpinLock();


        /// <summary>
        /// 玩家集合
        /// </summary>
        public LinkedList<CsGamePlayer> Players { get; set; }

        /// <summary>
        /// 打出的牌的状态
        /// </summary>
        protected OutCardState OutCardState
        {
            get; set;
        }



        protected LinkedListNode<CsGamePlayer> Player { get; set; }

        public LinkedListNode<CsGamePlayer> FindPlayer(string uid)
        {
            LinkedListNode<CsGamePlayer> next = Players.First;
            for (int i = 0; i < Players.Count; i++)
            {
                if (next.Value.PlayerUser.Uid == uid)
                {
                    return next;
                }
                next = next.Next;
            }
            return null;
        }

        public void SetPlayerReadyFalse()
        {
            foreach (var row in Players)
            {
                row.IsReady = false;
            }
        }



        protected abstract void SendCard(Boolean isReady);

        //public int[] cardArray = {
        //    0 | 0x10,1 | 0x10,2 | 0x10,3 | 0x10,4 | 0x10,5 | 0x10,6 | 0x10,7 | 0x10,8 | 0x10,
        //    0 | 0x10,1 | 0x10,2 | 0x10,3 | 0x10,4 | 0x10,5 | 0x10,6 | 0x10,7 | 0x10,8 | 0x10,
        //    0 | 0x10,1 | 0x10,2 | 0x10,3 | 0x10,4 | 0x10,5 | 0x10,6 | 0x10,7 | 0x10,8 | 0x10,
        //    0 | 0x10,1 | 0x10,2 | 0x10,3 | 0x10,4 | 0x10,5 | 0x10,6 | 0x10,7 | 0x10,8 | 0x10,

        //    0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
        //    0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
        //    0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
        //    0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
        //    0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
        //    0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
        //    0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
        //    0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
        //};

        public int[] cardArray = {
          0 | 0x10,
0 | 0x10,
0 | 0x10,
0 | 0x10,
1 | 0x10,
1 | 0x10,
1 | 0x10,
1 | 0x10,
2 | 0x10,
2 | 0x10,
2 | 0x10,
2 | 0x10,
3 | 0x10,
3 | 0x10,
3 | 0x10,
3 | 0x10,
4 | 0x10,
4 | 0x10,
4 | 0x10,
4 | 0x10,
5 | 0x10,
5 | 0x10,
5 | 0x10,
5 | 0x10,
6 | 0x10,
6 | 0x10,
6 | 0x10,
6 | 0x10,
7 | 0x10,
7 | 0x10,
7 | 0x10,
7 | 0x10,
8 | 0x10,
8 | 0x10,
8 | 0x10,
8 | 0x10,
0 | 0x10 | 0x80,
1 | 0x10 | 0x80,
2 | 0x10 | 0x80,
3 | 0x10 | 0x80,
4 | 0x10 | 0x80,
5 | 0x10 | 0x80,
6 | 0x10 | 0x80,
7 | 0x10 | 0x80,
8 | 0x10 | 0x80,
0 | 0x10 | 0x80,
1 | 0x10 | 0x80,
2 | 0x10 | 0x80,
3 | 0x10 | 0x80,
4 | 0x10 | 0x80,
5 | 0x10 | 0x80,
6 | 0x10 | 0x80,
7 | 0x10 | 0x80,
6 | 0x10,
0 | 0x10 | 0x100,
0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
0 | 0x10 | 0x80,1 | 0x10 | 0x80,2 | 0x10 | 0x80,3 | 0x10 | 0x80,4 | 0x10 | 0x80,5 | 0x10 | 0x80,6 | 0x10 | 0x80,7 | 0x10 | 0x80,8 | 0x10 | 0x80,
0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,8 | 0x10|0x100,
0 | 0x10 | 0x100,1 | 0x10|0x100,2 | 0x10|0x100,3 | 0x10|0x100,4 | 0x10|0x100,5 | 0x10|0x100,6 | 0x10|0x100,7 | 0x10|0x100,3 | 0x10,
        };


        public int CardIndex { get; set; }

        public virtual void BeginGame(string userId)
        {
            LinkedListNode<CsGamePlayer> player = FindPlayer(userId);

            if (Players.All(p => p.IsReady) && Players.Count == 4)
            {
                CardIndex = 0;
                // RandCard();
                SendCard(false);
            }
            else
            {
                //DOTO: 待重构,不够人数加入房间因为想只给客户端发一次，将1003与1006合并为一个接口
                int playerLen = this.Players.Count;

                A1003AndA1006Response.Builder responseBase = this.Create1003And1006Req(playerLen);

                var responseArray = responseBase.Build().ToByteArray();

                var tempPlayer = this.Players.First;

                for (int i = 0; i < playerLen; i++)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(tempPlayer.Value.PlayerUser.Uid, 10036, responseArray);
                    tempPlayer = tempPlayer.Next;
                }

            }
        }

        public virtual void ReadyGame(string userId)
        {
            LinkedListNode<CsGamePlayer> player = FindPlayer(userId);
            player.Value.IsReady = true;

            if (Players.All(p => p.IsReady) && Players.Count == 4)
            {
                CardIndex = 0;
                // RandCard();
                SendCard(true);
            }
            else
            {
                A1005Response.Builder a1005Response = A1005Response.CreateBuilder();
                a1005Response.SetUid(userId);

                byte[] responseArray = a1005Response.Build().ToByteArray();
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1005, responseArray);
                }
            }
        }



        protected void RandCard()
        {
            Random rd = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < cardArray.Length; i++)
            {
                int index = rd.Next(0, cardArray.Length - 1 - i);
                list.Add(cardArray[index]);
                cardArray[index] = cardArray[cardArray.Length - 1 - i];
            }
            cardArray = list.ToArray();
        }
        public int ReadCard()
        {
            var tempCard = cardArray[CardIndex];
            CardIndex++;
            return tempCard;
        }

        public void SetAllResetEvent()
        {
            foreach (var row in this.Players)
            {
                row.ResetEvent.Set();
            }
        }

        public void JoinRoom(GameUser user)
        {

            bool llNodeListLocked = false;
            try
            {
                roomLock.Enter(ref llNodeListLocked);
                this.Players.AddLast(new CsGamePlayer(user));
                this.BeginGame(user.Uid);

            }
            finally
            {
                if (llNodeListLocked)
                    roomLock.Exit();
            }
        }

        public void SendMessage(string uid, string message,string messageType)
        {
            A1100Response.Builder response = A1100Response.CreateBuilder();
            response.Uid = uid;
            response.Message = message;
            response.MessageType = messageType;
            byte[] responseByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1100, responseByte);
            }
        }

        protected A1003AndA1006Response.Builder Create1003And1006Req(int playerLen) {

            A1003AndA1006Response.Builder responseBase = A1003AndA1006Response.CreateBuilder();
            responseBase.RoomID = this.RoomId;
            responseBase.RoomType = this.RoomType;
            A1003Response.Builder response = A1003Response.CreateBuilder();
            response.RoomID = this.RoomId;
            response.RoomType = this.RoomType;

            var tempPlayer = Players.First;
            for (int i = 0; i < playerLen; i++)
            {
                var builder = A1003User.CreateBuilder();
                builder.Uid = tempPlayer.Value.PlayerUser.Uid;
                builder.Sore = 1000;
                builder.HatImage = "";
                builder.Sex = 0;
                builder.IsReady = tempPlayer.Value.IsReady ? 1: 0;
                response.AddUsers(builder.Build());
                tempPlayer = tempPlayer.Next;
            }

            responseBase.A1003Req = response.Build();
            return responseBase;
        }
    }
}

public enum OutCardState
{
    Normal = 0,
    Chi,
    Peng,
    Gang,
    Hu
}
