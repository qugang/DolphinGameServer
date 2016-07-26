﻿using DolphinServer.ProtoEntity;
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
        public CsMjGameRoom(int jushu) : base(jushu)
        {

        }
        /// <summary>
        /// 是否开局，因为开局需要检查碰碰胡等
        /// </summary>
        private Boolean isFrist = false;
        protected override void SendCard()
        {

            if (this.JuShu == 0)
            {
                A9999DataErrorResponse.Builder errorResponse = A9999DataErrorResponse.CreateBuilder();
                errorResponse.ErrorCode = 3;
                errorResponse.ErrorInfo = "茶卷已用完";
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 9999, errorResponse.Build().ToByteArray());
                }
            }

            var downArray = cardArray.Take(14).ToArray();
            var rigthArray = cardArray.Skip(14).Take(13).ToArray();
            var topArray = cardArray.Skip(27).Take(13).ToArray();
            var leftArray = cardArray.Skip(40).Take(13).ToArray();

            Player down = ProtoEntity.Player.CreateBuilder().AddRangeCard(downArray).SetScore(this.Player.Value.Score).Build();
            Player rigth = ProtoEntity.Player.CreateBuilder().AddRangeCard(rigthArray).SetScore(this.Player.NextOrFirst().Value.Score).Build();
            Player top = ProtoEntity.Player.CreateBuilder().AddRangeCard(topArray).SetScore(this.Player.NextOrFirst().NextOrFirst().Value.Score).Build();
            Player left = ProtoEntity.Player.CreateBuilder().AddRangeCard(leftArray).SetScore(this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value.Score).Build();

            Players.First.Value.InitCard(downArray);
            Players.First.Next.Value.InitCard(rigthArray);
            Players.First.Next.Next.Value.InitCard(topArray);
            Players.First.Next.Next.Next.Value.InitCard(leftArray);


            var response = A1006Response.CreateBuilder();
            response.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response.JuShu = this.JuShu;
            response.Zhangshu = 55;
            response.Player1 = down;
            response.Player2 = rigth;
            response.Player3 = top;
            response.Player4 = left;


            var response1 = A1006Response.CreateBuilder();
            response1.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response1.JuShu = this.JuShu;
            response1.Zhangshu = 55;
            response1.Player1 = rigth;
            response1.Player2 = top;
            response1.Player3 = left;
            response1.Player4 = down;

            var response2 = A1006Response.CreateBuilder();
            response2.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response2.JuShu = this.JuShu;
            response2.Zhangshu = 55;
            response2.Player1 = top;
            response2.Player2 = left;
            response2.Player3 = down;
            response2.Player4 = rigth;

            var response3 = A1006Response.CreateBuilder();
            response3.ZhuangUid = this.Player.Value.PlayerUser.Uid;
            response3.JuShu = this.JuShu;
            response3.Zhangshu = 55;
            response3.Player1 = left;
            response3.Player2 = down;
            response3.Player3 = rigth;
            response3.Player4 = top;

            WebSocketServerWrappe.SendPackgeWithUser(this.Player.Value.PlayerUser.Uid, 1006, response.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().Value.PlayerUser.Uid, 1006, response1.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().NextOrFirst().Value.PlayerUser.Uid, 1006, response2.Build().ToByteArray());
            WebSocketServerWrappe.SendPackgeWithUser(this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value.PlayerUser.Uid, 1006, response3.Build().ToByteArray());
            this.cardIndex = 53;
            isFrist = true;

            foreach (var row in this.Players)
            {
                row.IsReady = false;
            }

            this.JuShu--;

        }


        /// <summary>
        /// 开局胡
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="huType"></param>
        public void FistHu(string uid, int huType)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            int score = this.CalculationScore(huType);

            foreach (var row in Players)
            {
                if ((row.CheckKaiJuHu()) && row.PlayerUser.Uid != uid)
                {
                    LogManager.Log.Debug("开局胡等待" + row.PlayerUser.Uid + "wait");
                    row.ResetEvent.WaitOne();
                    LogManager.Log.Debug("开局胡等待处理完毕" + row.PlayerUser.Uid + "wait");
                }
            }

            if (node.Value.PlayerUser.Uid == this.Players.First.Value.PlayerUser.Uid)
            {
                score++;
            }

            var rigthNode = node.NextOrFirst();
            int subRigthScore = rigthNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;
            var topNode = node.NextOrFirst().NextOrFirst();
            int subTopScore = topNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;
            var leftNode = node.NextOrFirst().NextOrFirst().NextOrFirst();
            int subLeftScore = leftNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;


            rigthNode.Value.Score -= subRigthScore;
            topNode.Value.Score -= subTopScore;
            leftNode.Value.Score -= subLeftScore;

            this.Player.Value.Score += subRigthScore + subTopScore + subLeftScore;

            A1015Response.Builder response = A1015Response.CreateBuilder();
            response.HuType = huType;
            response.Uid = node.Value.PlayerUser.Uid;
            response.Score = subRigthScore + subTopScore + subLeftScore;
            var builder = A1015User.CreateBuilder();
            builder.Uid = rigthNode.Value.PlayerUser.Uid;
            builder.Score = subRigthScore;
            response.AddUsers(builder);
            builder.Uid = topNode.Value.PlayerUser.Uid;
            builder.Score = subTopScore;
            response.AddUsers(builder);
            builder.Uid = leftNode.Value.PlayerUser.Uid;
            builder.Score = subLeftScore;
            response.AddUsers(builder);

            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1015, sendByte);
            }
            node.Value.ResetEvent.Set();

        }

        /// <summary>
        /// 自摸
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="huType"></param>
        public void Zimo(string uid, int huType)
        {
            LogManager.Log.Debug("自摸开始");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.Player = this.Player;
            int score = this.CalculationScore(huType);

            if (node.Value.PlayerUser.Uid == this.Players.First.Value.PlayerUser.Uid)
            {
                score++;
            }

            var rigthNode = node.NextOrFirst();
            int subRigthScore = rigthNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;
            var topNode = node.NextOrFirst().NextOrFirst();
            int subTopScore = topNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;
            var leftNode = node.NextOrFirst().NextOrFirst().NextOrFirst();
            int subLeftScore = leftNode.Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid ? score + 1 : score;


            rigthNode.Value.Score -= subRigthScore;
            topNode.Value.Score -= subTopScore;
            leftNode.Value.Score -= subLeftScore;

            this.Player.Value.Score += subRigthScore + subTopScore + subLeftScore;

            A1013Response.Builder response = A1013Response.CreateBuilder();
            response.HuType = huType;
            response.Uid = node.Value.PlayerUser.Uid;
            response.Score = subRigthScore + subTopScore + subLeftScore;
            var builder = A1013User.CreateBuilder();
            builder.Uid = rigthNode.Value.PlayerUser.Uid;
            builder.Score = subRigthScore;
            response.AddUsers(builder);
            builder.Uid = topNode.Value.PlayerUser.Uid;
            builder.Score = subTopScore;
            response.AddUsers(builder);
            builder.Uid = leftNode.Value.PlayerUser.Uid;
            builder.Score = subLeftScore;
            response.AddUsers(builder);

            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1013, sendByte);
            }
            this.OutCardState = OutCardState.Hu;
            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 捉跑
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="desUid"></param>
        /// <param name="huType"></param>
        public void ZhuoPao(string uid, string desUid, int huType, int card)
        {
            foreach (var row in Players)
            {
                if ((row.CheckHu(card)) && row.PlayerUser.Uid != uid && row.PlayerUser.Uid != desUid)
                {
                    LogManager.Log.Debug("捉炮等待" + row.PlayerUser.Uid + "wait");
                    row.ResetEvent.WaitOne();
                    LogManager.Log.Debug("捉炮等待处理完毕" + row.PlayerUser.Uid + "wait");
                }
            }
            LogManager.Log.Debug("捉炮开始");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.Player = this.Player;
            int score = this.CalculationScore(huType);

            if (node.Value.PlayerUser.Uid == this.Players.First.Value.PlayerUser.Uid)
            {
                score++;
            }
            var desNode = FindPlayer(desUid);
            var desSubScore = desNode.Value.PlayerUser.Uid == this.Players.First.Value.PlayerUser.Uid ? score++ : score;
            desNode.Value.Score -= desSubScore;
            this.Player.Value.Score += desSubScore;

            A1014Response.Builder response = A1014Response.CreateBuilder();
            response.Uid = node.Value.PlayerUser.Uid;
            response.Score = desSubScore;
            response.HuType = huType;
            response.DesUid = desUid;
            response.DesScore = desSubScore;
            response.Card = card;
            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1014, sendByte);
            }
            this.OutCardState = OutCardState.Hu;
            node.Value.ResetEvent.Set();
            LogManager.Log.Debug("捉炮结束");
        }

        /// <summary>
        /// 吃牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        public void Chi(string uid, string desUid, int card, int card1, int card2)
        {
            LogManager.Log.Debug("吃牌开始");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if ((row.CheckGang(card) ||
                    row.CheckPeng(card) ||
                    row.CheckHu(card)) && row.PlayerUser.Uid != uid && row.PlayerUser.Uid != desUid)
                {
                    row.ResetEvent.WaitOne();
                }
            }

            //牌未被彭吃杠
            if (this.OutCardState == OutCardState.Normal)
            {
                this.OutCardState = OutCardState.Chi;
                LogManager.Log.Debug("牌可以吃");
                node.Value.Chi(card, card1, card2);
                A1010Response.Builder response = A1010Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                response.Card1 = card1;
                response.Card2 = card2;
                var array = response.Build().ToByteArray();
                this.Player = node;
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1010, array);
                }
            }
            node.Value.ResetEvent.Set();
            LogManager.Log.Debug("吃牌结束");
        }

        /// <summary>
        /// 杠牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void Gang(string uid, string desUid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if ((row.CheckHu(card)) && row.PlayerUser.Uid != uid && row.PlayerUser.Uid != desUid)
                {
                    row.ResetEvent.WaitOne();
                }
            }



            //牌未被胡
            if (this.OutCardState == OutCardState.Normal)
            {

                this.OutCardState = OutCardState.Gang;
                int modZhang = 108 - (this.cardIndex + 1);

                int dunshu = modZhang / 2;


                int saizi = new Random().Next(2, 12);

                //TODO : 筛子随出大于当前墩数
                if (saizi > dunshu)
                {
                    this.OutCardState = OutCardState.Gang;

                    A1012Response.Builder response1 = A1012Response.CreateBuilder();
                    response1.Uid = node.Value.PlayerUser.Uid;
                    response1.Card = card;
                    response1.CardCode = 1;
                    var array1 = response1.Build().ToByteArray();
                    foreach (var row in this.Players)
                    {
                        WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1012, array1);
                    }
                    return;
                }

                int card1 = this.ReadCard();
                int card2 = this.ReadCard();
                A1012Response.Builder response = A1012Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                response.Card1 = card1;
                response.Card2 = card2;
                var array = response.Build().ToByteArray();
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1012, array);
                }
                node.Value.Gang(card);
                this.Player = node;
                this.OutCardState = OutCardState.Normal;


                if (node.Value.CheckHu(card1) || node.Value.CheckHu(card2))
                {
                    node.Value.ResetEvent.WaitOne();
                    return;
                }


                foreach (var row in Players)
                {
                    LogManager.Log.Debug(row.PlayerUser.Uid + "手上的牌" + row.PrintCards());

                    if (row.PlayerUser.Uid != this.Player.Value.PlayerUser.Uid)
                    {
                        if (row.CheckGang(card2) || row.CheckPeng(card2) || row.CheckHu(card1) || row.CheckHu(card2))
                        {
                            row.ResetEvent.WaitOne();
                            if (this.OutCardState != OutCardState.Normal)
                            {
                                return;
                            }
                            continue;
                        }
                    }
                }

                this.Player = this.Player.NextOrFirst();
                //是最后一张牌发送海底命令
                if (this.cardIndex == cardArray.Length - 2)
                {
                    A1016Response.Builder rep1016 = A1016Response.CreateBuilder();
                    rep1016.Uid = this.Player.Value.PlayerUser.Uid;
                    var rep1016Array = rep1016.Build().ToByteArray();
                    foreach (var row in Players)
                    {
                        WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1016, rep1016Array);
                    }

                }
                else
                {
                    Mo(this.Player.Value.PlayerUser.Uid);
                }


            }
            node.Value.ResetEvent.Set();
        }



        /// <summary>
        /// 碰牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void Peng(string uid, string desUid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != desUid && row.PlayerUser.Uid != uid && (row.CheckGang(card) || row.CheckHu(card)))
                {
                    row.ResetEvent.WaitOne();
                }
            }

            //牌未被杠或胡
            if (this.OutCardState == OutCardState.Normal)
            {
                this.OutCardState = OutCardState.Peng;
                A1011Response.Builder response = A1011Response.CreateBuilder();
                response.Uid = node.Value.PlayerUser.Uid;
                response.Card = card;
                var array = response.Build().ToByteArray();

                node.Value.Peng(card);
                this.Player = node;
                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1011, array);
                }
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


            if (cardIndex == cardArray.Length)
            {
                //TODO: 牌已摸完
                return;
            }
            int card = this.ReadCard();

            LogManager.Log.Debug("摸到牌" + card.GetItemValue() + "牌类型" + card.GetItemType());

            node.Value.AddCard(card);


            A1008Response.Builder response = A1008Response.CreateBuilder();
            response.Card = card;
            response.Uid = uid;
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
                LogManager.Log.Debug("没有轮到" + uid + "打牌");
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
                LogManager.Log.Debug(prePlayer.Value.PlayerUser.Uid + "手上的牌" + prePlayer.Value.PrintCards());
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
            //是最后一张牌发送海底命令
            if (this.cardIndex == cardArray.Length - 2)
            {
                A1016Response.Builder rep1016 = A1016Response.CreateBuilder();
                rep1016.Uid = this.Player.Value.PlayerUser.Uid;
                var rep1016Array = rep1016.Build().ToByteArray();
                foreach (var row in Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1016, rep1016Array);
                }

            }
            else
            {
                Mo(this.Player.Value.PlayerUser.Uid);

            }
        }

        public void MoHaidi(string uid)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            int card = this.cardArray[this.cardIndex];

            A1017Response.Builder rep1017 = A1017Response.CreateBuilder();
            rep1017.Uid = this.Player.Value.PlayerUser.Uid;
            rep1017.Card = card;
            var rep1017Array = rep1017.Build().ToByteArray();
            foreach (var row in Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1017, rep1017Array);
            }
        }

        public void GuoHaidi(string uid)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            LogManager.Log.Debug(node.Value.PlayerUser.Uid + "海底过");
            node.Value.ResetEvent.Set();

            //都没人要留局
            if (node.NextOrFirst().Value.PlayerUser.Uid == this.Player.Value.PlayerUser.Uid)
            {
                A1018Response.Builder rep1018 = A1018Response.CreateBuilder();
                rep1018.Card = this.cardArray[this.cardIndex]; ;
                var rep1018Array = rep1018.Build().ToByteArray();
                foreach (var row in Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1018, rep1018Array);
                }
            }
            else
            {
                A1016Response.Builder rep1016 = A1016Response.CreateBuilder();
                rep1016.Uid = node.NextOrFirst().Value.PlayerUser.Uid;
                var rep1016Array = rep1016.Build().ToByteArray();
                foreach (var row in Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1016, rep1016Array);
                }
            }

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


        //1 小胡抓炮
        //10 小胡自摸
        //100 四喜
        //1000 板板胡
        //10000 缺一色
        //100000 六六顺
        //1000000 碰碰胡
        //10000000 清一色
        //100000000 海底捞月
        //1000000000 海底炮
        //10000000000 七小对
        //100000000000 豪华七小对
        //1000000000000 杠上开花
        //10000000000000 抢杠胡
        //100000000000000 杠上炮
        //1000000000000000 全求人
        //10000000000000000 将将胡
        //100000000000000000 杠上炮
        //1000000000000000000 杠翻倍
        public int CalculationScore(int huType)
        {
            int score = 0;
            if ((huType & 1) == 1)
            {
                score += 1;
            }

            int i = 2;
            while (i <= 32)
            {
                //小胡
                if ((huType & i) == i)
                {
                    score += 2;
                }
                i = i * 2;
            }

            i = 32;

            while (i <= 0x40000)
            {
                if ((huType & i) == i)
                {
                    score += 6;
                }
                i = i * 2;
            }
            return score;
        }

    }
}
