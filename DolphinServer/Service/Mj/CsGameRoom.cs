using DolphinServer.ProtoEntity;
using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Free.Dolphin.Common.Util;
namespace DolphinServer.Service.Mj
{
    /// <summary>
    /// 长沙麻将房间管理
    /// </summary>
    public class CsMjGameRoom : MjGameRoomBase
    {
        /// <summary>
        /// 是否开局，因为开局需要检查碰碰胡等
        /// </summary>
        private Boolean isFrist = false;
        protected override void SendCard()
        {
            var downArray = cardArray.Take(14).ToArray();
            var rigthArray = cardArray.Skip(14).Take(13).ToArray();
            var topArray = cardArray.Skip(27).Take(13).ToArray();
            var leftArray = cardArray.Skip(40).Take(13).ToArray();

            Player down = ProtoEntity.Player.CreateBuilder().AddRangeCard(downArray).Build();
            Player rigth = ProtoEntity.Player.CreateBuilder().AddRangeCard(rigthArray).Build();
            Player top = ProtoEntity.Player.CreateBuilder().AddRangeCard(topArray).Build();
            Player left = ProtoEntity.Player.CreateBuilder().AddRangeCard(leftArray).Build();

            Players.First.Value.InitCard(downArray);
            Players.First.Next.Value.InitCard(rigthArray);
            Players.First.Next.Next.Value.InitCard(topArray);
            Players.First.Next.Next.Next.Value.InitCard(leftArray);


            var response = A1006Response.CreateBuilder();
            response.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response.Player1 = down;
            response.Player2 = rigth;
            response.Player3 = top;
            response.Player4 = left;


            var response1 = A1006Response.CreateBuilder();
            response1.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response1.Player1 = rigth;
            response1.Player2 = top;
            response1.Player3 = left;
            response1.Player4 = down;

            var response2 = A1006Response.CreateBuilder();
            response2.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response2.Player1 = top;
            response2.Player2 = left;
            response2.Player3 = down;
            response2.Player4 = rigth;

            var response3 = A1006Response.CreateBuilder();
            response3.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response3.Player1 = left;
            response3.Player2 = down;
            response3.Player3 = rigth;
            response3.Player4 = top;

            WebSocketServerWrappe.SendPackgeWithUser(this.Player.Value.PlayerUser.Uid, 1006, response.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().Value.PlayerUser.Uid, 1006, response1.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().NextOrFirst().Value.PlayerUser.Uid, 1006, response2.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value.PlayerUser.Uid, 1006, response3.Build().ToByteArray());
            this.cardIndex += 53;
            isFrist = true;

        }


        /// <summary>
        /// 开局第一次胡
        /// </summary>
        /// <param name="uid"></param>
        public void FistHu(string uid)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            node.Value.ResetEvent.Set();
        }

        public void Hu(string uid, int huType)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.Player = this.Player;
            this.OutCardState = OutCardState.Hu;



            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 吃牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        public void Chi(string uid, int card, int card1, int card2)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if ((row.CheckGang(card) ||
                    row.CheckPeng(card) ||
                    row.CheckHu(card)) && row.PlayerUser.Uid != uid)
                {
                    row.ResetEvent.WaitOne();
                }
            }

            //牌未被彭吃杠
            if (this.OutCardState == OutCardState.Normal)
            {
                node.Value.Chi(card, card1, card2);
                A1010Response.Builder response = A1010Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                response.Card1 = card1;
                response.Card2 = card2;
                var array = response.Build().ToByteArray();
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1010, array);
                }
                this.OutCardState = OutCardState.Chi;
            }
            this.Player = node;
            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 杠牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void Gang(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if ((row.CheckHu(card)) && row.PlayerUser.Uid != uid)
                {
                    row.ResetEvent.WaitOne();
                }
            }

            //牌未被胡
            if (this.OutCardState == OutCardState.Normal)
            {
                A1012Response.Builder response = A1012Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                var array = response.Build().ToByteArray();
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1012, array);
                }
                this.OutCardState = OutCardState.Gang;
                node.Value.Gang(card);
                this.Player = node;
                Mo(node.Value.PlayerUser.Uid);
            }
            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 碰牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void Peng(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != uid && (row.CheckGang(card) || row.CheckHu(card)))
                {
                    row.ResetEvent.WaitOne();
                }
            }

            //牌未被杠或胡
            if (this.OutCardState == OutCardState.Normal)
            {
                A1011Response.Builder response = A1011Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                var array = response.Build().ToByteArray();
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1011, array);
                }
                this.OutCardState = OutCardState.Peng;
                this.Player = node;
                node.Value.Peng(card);
            }
            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="uid"></param>
        public void Mo(string uid)
        {
            LogManager.Log.Debug("开始摸牌");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            int card = this.ReadCard();

            LogManager.Log.Debug("摸到牌" + card.GetItemValue() + "牌类型" + card.GetItemType());

            node.Value.AddCard(card);


            A1008Response.Builder response = A1008Response.CreateBuilder();
            response.Card = card;
            response.Uid = uid;
            response.ModCard = 108 - cardIndex;
            byte[] responseArray = response.Build().ToByteArray();

            foreach (var row in Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1008, responseArray);
            }
            LogManager.Log.Debug("摸牌结束");
            
        }

        /// <summary>
        /// 打牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void Da(string uid, int card)
        {
            if (uid != this.Player.Value.PlayerUser.Uid)
            {
                LogManager.Log.Debug("没有轮到"+uid +"打牌");
                return;
            }

            if (this.isFrist)
            {
                foreach (var row in Players)
                {
                    if ((row.CheckKaiJuHu()) && row.PlayerUser.Uid != uid)
                    {
                        LogManager.Log.Debug("开局胡等待" + row.PlayerUser.Uid + "wait");
                        row.ResetEvent.WaitOne();
                        LogManager.Log.Debug("开局胡等待处理完毕" + row.PlayerUser.Uid + "wait");
                    }
                }
            }

            LogManager.Log.Debug("通过开局胡检查");
            this.isFrist = false;
            this.Player.Value.DaCard(card);
            this.OutCardState = OutCardState.Normal;

            A1007Response.Builder response = A1007Response.CreateBuilder();
            response.Card = card;
            response.Uid = uid;
            byte[] responseArray = response.Build().ToByteArray();

            foreach (var row in Players)
            {
                row.ResetEvent.Reset();
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1007, responseArray);
            }

            LogManager.Log.Debug("当前玩家" + this.Player.Value.PlayerUser.Uid + "后续玩家" + this.Player.NextOrFirst().Value.PlayerUser.Uid);
            var prePlayer = this.Player.NextOrFirst();
            LogManager.Log.Debug("检查吃" + prePlayer.Value.PlayerUser.Uid + "是否可以吃" + card.GetItemValue() + "类型" + card.GetItemType());
            
            if (prePlayer.Value.CheckChi(card))
            {
                LogManager.Log.Debug("吃" + prePlayer.Value.PlayerUser.Uid);
                prePlayer.Value.ResetEvent.WaitOne();
                if (this.OutCardState != OutCardState.Normal)
                {
                    return;
                }
            }

            foreach (var row in Players)
            {
                LogManager.Log.Debug(row.PlayerUser.Uid + "手上的牌" + row.PrintCards());

                if (row.PlayerUser.Uid != this.Player.Value.PlayerUser.Uid)
                {
                    if (row.CheckGang(card) || row.CheckPeng(card) || row.CheckHu(card))
                    {
                        LogManager.Log.Debug("等待玩家操作" + row.PlayerUser.Uid + "牌：" + card.GetItemValue());
                        row.ResetEvent.WaitOne();
                        LogManager.Log.Debug("等待玩家操作完毕" + row.PlayerUser.Uid + "牌：" + card.GetItemValue());
                        if (this.OutCardState != OutCardState.Normal)
                        {
                            return;
                        }
                        continue;
                    }
                }
            }

            this.Player = this.Player.NextOrFirst();
            Mo(this.Player.Value.PlayerUser.Uid);
        }

        /// <summary>
        /// 过
        /// </summary>
        /// <param name="uid"></param>
        public void Guo(string uid)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            LogManager.Log.Debug(node.Value.PlayerUser.Uid + "过");
            node.Value.ResetEvent.Set();
        }




    }



}
