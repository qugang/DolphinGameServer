using DolphinServer.ProtoEntity;
using Free.Dolphin.Core.Util;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinServer.Entity;
using DolphinServer.Controller;
using Free.Dolphin.Common;
using DolphinServer.Service.Mj.ActionStorage;

namespace DolphinServer.Service.Mj
{
    public  class CsGameRoomThree : CsMjGameRoom
    {

        public CsGameRoomThree(string uid) : base(uid)
        {
            this.CurrentCard = new List<int>();
            this.RoomPeoPleType = 1;
        }

        protected override void SendCard(Boolean isReady) {
            if (this.JuShu == 9)
            {
                GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(this.RoomMagaerUid);


                user.RoomCard--;
                RedisContext.GlobalContext.AddHashEntity(user);

                //房主茶卷不足
                if (user.RoomCard <= 0)
                {
                    A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                    error.ErrorCode = 5;
                    error.ErrorInfo = ErrorInfo.ErrorDic[5];
                    byte[] responseByte = error.Build().ToByteArray();
                    foreach (var row in this.Players)
                    {
                        WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 9999, responseByte);
                    }
                    return;
                }

                foreach (var row in this.Players)
                {
                    row.HuType = 0;
                    row.Score = 1000;
                    row.SubScore = 0;
                    row.AddScore = 0;
                    this.JuShu = 1;
                }
            }

            if (this.Player == null)
            {
                this.Player = this.Players.First;
            }



            //庄为链表结构，设置庄
            var temp1 = this.Player.Value;
            var temp2 = this.Player.NextOrFirst().Value;
            var temp3 = this.Player.NextOrFirst().NextOrFirst().Value;

            this.Players.First.Value = temp1;
            this.Players.First.NextOrFirst().Value = temp2;
            this.Players.First.NextOrFirst().NextOrFirst().Value = temp3;
            this.Player = this.Players.First;



            var downArray = cardArray.Take(14).ToArray();
            var rigthArray = cardArray.Skip(14).Take(13).ToArray();
            var topArray = cardArray.Skip(27).Take(13).ToArray();


            Players.First.Value.InitCard(downArray);
            Players.First.Next.Value.InitCard(rigthArray);
            Players.First.Next.Next.Value.InitCard(topArray);


            var response = A1006Response.CreateBuilder();
            response.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response.JuShu = this.JuShu;
            response.Zhangshu = 55;

            var player1 = ProtoEntity.Player.CreateBuilder();
            player1.AddRangeCard(downArray);
            player1.SetScore(this.Player.Value.Score);
            player1.SetUid(this.Player.Value.PlayerUser.Uid);
            response.AddUsers(player1);

            var player2 = ProtoEntity.Player.CreateBuilder();
            player2.AddRangeCard(rigthArray);
            player2.SetScore(this.Player.NextOrFirst().Value.Score);
            player2.SetUid(this.Player.NextOrFirst().Value.PlayerUser.Uid);
            response.AddUsers(player2);

            var player3 = ProtoEntity.Player.CreateBuilder();
            player3.AddRangeCard(topArray);
            player3.SetScore(this.Player.NextOrFirst().NextOrFirst().Value.Score);
            player3.SetUid(this.Player.NextOrFirst().NextOrFirst().Value.PlayerUser.Uid);
            response.AddUsers(player3);
            

            A1003AndA1006Response.Builder responseBase = !isReady ? this.Create1003And1006Req(this.Players.Count) : A1003AndA1006Response.CreateBuilder();
            responseBase.A1006Req = response.Build();
            foreach (var row in this.Players)
            {
                row.ReLoad();
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 10036, responseBase.Build().ToByteArray());

            }

            this.CardIndex = 40;
            IsFrist = true;
            dianPaoNumber = 0;
            haiGuoNumber = 0;
            this.CurrentCard.Clear();
            this.IsEnd = false;
            this.JuShu++;


            //存储动作
            List<CmdPlayer> lCmdPlayer = new List<CmdPlayer>();

            lCmdPlayer.Add(new CmdPlayer
            {
                Uid = player1.Uid,
                Cards = downArray.ToList()
            });
            lCmdPlayer.Add(new CmdPlayer
            {
                Uid = player2.Uid,
                Cards = rigthArray.ToList()
            });

            lCmdPlayer.Add(new CmdPlayer
            {
                Uid = player3.Uid,
                Cards = topArray.ToList()
            });
            actionStorage = new GameActionStoreage(lCmdPlayer);
        }
    }
}
