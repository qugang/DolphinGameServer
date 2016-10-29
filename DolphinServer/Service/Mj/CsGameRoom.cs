using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Free.Dolphin.Core.Util;
using DolphinServer.Controller;
using DolphinServer.Entity;
using Free.Dolphin.Common;
using DolphinServer.Service.Mj.ActionStorage;

namespace DolphinServer.Service.Mj
{
    /// <summary>
    /// 长沙麻将房间管理
    /// </summary>
    public class CsMjGameRoom : MjGameRoomBase
    {
        public CsMjGameRoom(string uid) : base()
        {
            this.CurrentCard = new List<int>();
            this.RoomMagaerUid = uid;
            this.RoomPeoPleType = 0;
        }
        /// <summary>
        /// 是否开局，因为开局需要检查碰碰胡等
        /// </summary>
        public bool IsFrist { get; set; }

        /// <summary>
        /// 点炮次数，用于返回结果时线程同步问题
        /// </summary>
        protected int dianPaoNumber = 0;

        protected GameActionStoreage actionStorage = null;

        /// <summary>
        /// 海底过，用于判断是否留局
        /// </summary>
        protected int haiGuoNumber = 0;
        protected override void SendCard(Boolean isReady)
        {
            if (this.JuShu == 1)
            {
                GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(this.RoomMagaerUid);
                user.RoomCard--;
                RedisContext.GlobalContext.AddHashEntity(user);
            }

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
            var temp4 = this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value;

            this.Players.First.Value = temp1;
            this.Players.First.NextOrFirst().Value = temp2;
            this.Players.First.NextOrFirst().NextOrFirst().Value = temp3;
            this.Players.First.NextOrFirst().NextOrFirst().NextOrFirst().Value = temp4;
            this.Player = this.Players.First;



            var downArray = cardArray.Take(14).ToArray();
            var rigthArray = cardArray.Skip(14).Take(13).ToArray();
            var topArray = cardArray.Skip(27).Take(13).ToArray();
            var leftArray = cardArray.Skip(40).Take(13).ToArray();


            Players.First.Value.InitCard(downArray);
            Players.First.Next.Value.InitCard(rigthArray);
            Players.First.Next.Next.Value.InitCard(topArray);
            Players.First.Next.Next.Next.Value.InitCard(leftArray);


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

            var player4 = ProtoEntity.Player.CreateBuilder();
            player4.AddRangeCard(leftArray);
            player4.SetScore(this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value.Score);
            player4.SetUid(this.Player.NextOrFirst().NextOrFirst().NextOrFirst().Value.PlayerUser.Uid);
            response.AddUsers(player4);

            A1003AndA1006Response.Builder responseBase = !isReady ? this.Create1003And1006Req(this.Players.Count) : A1003AndA1006Response.CreateBuilder();
            responseBase.A1006Req = response.Build();
            foreach (var row in this.Players)
            {
                row.ReLoad();
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 10036, responseBase.Build().ToByteArray());

            }

            this.CardIndex = 53;
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
            lCmdPlayer.Add(new CmdPlayer
            {
                Uid = player4.Uid,
                Cards = leftArray.ToList()
            });
            actionStorage = new GameActionStoreage(lCmdPlayer);


        }


        /// <summary>
        /// 开局胡
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="huType"></param>
        public void FistHu(string uid, int huType)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);

            if (node.Value.ResetEvent.WaitOne(0))
            {
                return;
            }


            A1015Response.Builder response = A1015Response.CreateBuilder();
            response.HuType = huType;
            response.Uid = node.Value.PlayerUser.Uid;
            node.Value.FirstHuType |= huType;

            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1015, sendByte);
            }
            node.Value.ResetEvent.Set();
            actionStorage.PushFristHu(uid, huType);
        }

        /// <summary>
        /// 自摸
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="huType"></param>
        public void Zimo(string uid, int huType)
        {
            LogManager.Log.Debug("自摸开始");

            if (this.IsFrist)
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



            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.Player = node;
            this.Player.Value.HuType |= huType;
            Tuple<int, int> niao = this.ZhuaNiao();
            string niaoUid1 = this.GetUidWithNiao(niao.Item1);
            string niaoUid2 = this.GetUidWithNiao(niao.Item2);
            CalculationScore.Calculation(this.Players, niaoUid1, niaoUid2);

            SendA1013Response(node.Value.PlayerUser.Uid, niao, niaoUid1, niaoUid2);
            this.OutCardState = OutCardState.Hu;
            this.SetAllResetEvent();
            this.IsEnd = true;

            actionStorage.PushHu(uid, huType);
            this.EndGame();
        }

        /// <summary>
        /// 捉跑
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="desUid"></param>
        /// <param name="huType"></param>
        public void ZhuoPao(string uid, string desUid, int huType, int card, int card1)
        {
            LogManager.Log.Debug("捉炮开始");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            LinkedListNode<CsGamePlayer> desNode = FindPlayer(desUid);

            A1014Response.Builder response = A1014Response.CreateBuilder();
            response.Uid = node.Value.PlayerUser.Uid;
            response.HuType |= huType;
            response.DesUid = desUid;
            response.Card1 = card;
            response.Card2 = card1;

            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1014, sendByte);
            }
            node.Value.PaoHuType |= huType;
            node.Value.DianPaoPlayer = desNode;
            this.OutCardState = OutCardState.Hu;
            this.Player = node;
            node.Value.ResetEvent.Set();
            LogManager.Log.Debug("捉炮结束");

            actionStorage.PushZhuoPao(uid, card, huType);

        }

        /// <summary>
        /// 为了处理一炮多响的情况
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="desUid"></param>
        /// <param name="huType"></param>
        /// <param name="card"></param>
        /// <param name="card1"></param>
        public void ZhuoPaoResult(string uid, string desUid, int card1, int card2)
        {
            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != desUid && (row.CheckHu(card1)))
                {
                    row.ResetEvent.WaitOne();
                }
                if (card2 != -1 && row.PlayerUser.Uid != desUid && (row.CheckHu(card2)))
                {
                    row.ResetEvent.WaitOne();
                }
            }


            LinkedListNode<CsGamePlayer> desNode = FindPlayer(desUid);

            bool llNodeListLocked = false;
            try
            {
                roomLock.Enter(ref llNodeListLocked);
                dianPaoNumber++;
                //一炮多响，放炮人为庄
                if (dianPaoNumber > 1)
                {
                    this.Player = desNode;
                }
                else
                {
                    this.Player = FindPlayer(uid);
                }
                if (dianPaoNumber != 1)
                {
                    return;
                }

            }
            finally
            {
                if (llNodeListLocked)
                    roomLock.Exit();
            }


            Tuple<int, int> niao = this.ZhuaNiao();
            string niaoUid1 = this.GetUidWithNiao(niao.Item1);
            string niaoUid2 = this.GetUidWithNiao(niao.Item2);

            CalculationScore.Calculation(this.Players, niaoUid1, niaoUid2);

            SendA1021Response(niao, niaoUid1, niaoUid2, desUid);


            this.EndGame();

        }


        public Tuple<int, int> ZhuaNiao()
        {
            if (this.CardIndex == cardArray.Length - 1)
            {
                return new Tuple<int, int>(this.cardArray[this.CardIndex], this.cardArray[this.CardIndex]);
            }
            else
            {
                return new Tuple<int, int>(this.cardArray[this.CardIndex], this.cardArray[this.CardIndex + 1]);
            }

        }

        public string GetUidWithNiao(int card)
        {
            var tempPlayer = this.Players.First;
            for (int i = 1; i <= card.GetItemValue(); i++)
            {
                if (i == card.GetItemValue())
                {
                    return tempPlayer.Value.PlayerUser.Uid;
                }
                tempPlayer = tempPlayer.NextOrFirst();
            }
            throw new Exception("GetUidWithNiao Error!");
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

            if (node.Value.ResetEvent.WaitOne(0))
            {
                return;
            }

            //牌未被彭吃杠并且打牌的人为当前吃的玩家的上手
            if (this.OutCardState == OutCardState.Normal && this.Player.Value.PlayerUser.Uid == node.PreviousOrLast().Value.PlayerUser.Uid)
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
                LogManager.Log.Debug(node.Value.PlayerUser.Uid + "吃完手上的牌" + node.Value.PrintCards());

                actionStorage.PushChi(uid, card, card1, card2);
            }
            this.SetAllResetEvent();
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

            if (node.Value.ResetEvent.WaitOne(0))
            {
                return;
            }


            node.Value.Gang(card);

            //牌未被胡
            if (this.OutCardState == OutCardState.Normal)
            {
                node.Value.IsGang = true;
                this.OutCardState = OutCardState.Gang;

                int modZhang = 108 - (this.CardIndex + 1);

                int dunshu = modZhang / 2;


                int saizi = new Random().Next(2, 12);
                int card1 = -1;
                int card2 = -1;
                //TODO : 筛子随出大于当前墩数
                if (saizi <= dunshu)
                {
                    card1 = this.ReadCard();
                    card2 = this.ReadCard();
                }

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

                actionStorage.PushGang(uid, card, card1, card2);
            }
            this.Player = node;
            node.Value.ResetEvent.Set();
        }

        /// <summary>
        /// 暗杠
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void AnGang(string uid, int card)
        {
            if (this.IsFrist)
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

            this.IsFrist = false;

            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);

            node.Value.IsGang = true;

            this.Player = node;

            int modZhang = 108 - (this.CardIndex + 1);

            int dunshu = modZhang / 2;


            int saizi = new Random().Next(2, 12);

            int card1 = -1;
            int card2 = -1;

            //TODO : 筛子随出大于当前墩数
            if (saizi <= dunshu)
            {
                card1 = this.ReadCard();
                card2 = this.ReadCard();
            }

            A1012Response.Builder response = A1012Response.CreateBuilder();
            response.Uid = node.Value.PlayerUser.Uid;
            response.Card = card;
            response.Card1 = card1;
            response.Card2 = card2;
            response.GangType = 0;
            var array = response.Build().ToByteArray();
            node.Value.NeedGangDaPai.Add(card1);
            node.Value.NeedGangDaPai.Add(card2);
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1012, array);
            }
            node.Value.AnGang(card);

            actionStorage.PushGang(uid, card, card1, card2);
        }

        /// <summary>
        /// 杠打牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        public void GangDaPai(string uid, int card1, int card2)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.Player = node;
            this.OutCardState = OutCardState.Normal;

            if (card1 != -1 && card2 != -1)
            {
                A1020Response.Builder response = A1020Response.CreateBuilder();
                response.Card1 = card1;
                response.Card2 = card2;
                response.Uid = uid;
                byte[] responseArray = response.Build().ToByteArray();

                node.Value.zhuoCards.Add(card1);
                node.Value.zhuoCards.Add(card2);

                node.Value.NeedGangDaPai.Clear();

                this.CurrentCard.Clear();
                this.CurrentCard.Add(card1);
                this.CurrentCard.Add(card2);


                foreach (var row in Players)
                {
                    row.ResetEvent.Reset();
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1020, responseArray);
                }

                var prePlayer = this.Player.NextOrFirst();

                if (prePlayer.Value.CheckChi(card1) || prePlayer.Value.CheckChi(card2))
                {
                    prePlayer.Value.ResetEvent.WaitOne();
                    if (this.OutCardState != OutCardState.Normal)
                    {
                        return;
                    }
                }

                foreach (var row in Players)
                {
                    if (row.PlayerUser.Uid != this.Player.Value.PlayerUser.Uid)
                    {
                        if (row.CheckGang(card1) || row.CheckPeng(card1) || row.CheckHu(card1) ||
                            row.CheckGang(card2) || row.CheckPeng(card2) || row.CheckHu(card2))
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
            }

            this.Player = this.Player.NextOrFirst();
            //是最后一张牌发送海底命令
            if (this.CardIndex == cardArray.Length - 1)
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

            if (node.Value.ResetEvent.WaitOne(0))
                return;

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
                actionStorage.PushPeng(uid, card);
            }
            this.SetAllResetEvent();
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="uid"></param>
        public void Mo(string uid)
        {
            LogManager.Log.Debug("开始摸牌");
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);


            if (CardIndex == cardArray.Length)
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

            actionStorage.PushMo(uid, card);
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

            if (this.IsFrist)
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
            this.IsFrist = false;
            this.Player.Value.DaCard(card);

            actionStorage.PushDa(uid, card);

            //为了记录打出的最后牌，为了断线重连状态保存
            this.CurrentCard.Clear();
            this.CurrentCard.Add(card);

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
            if (this.CardIndex == cardArray.Length - 1)
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

        /// <summary>
        /// 暗杠补涨
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="card"></param>
        public void AnBuZhang(string uid, int card)
        {
            if (this.IsFrist)
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

            this.IsFrist = false;

            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            node.Value.AnBuZhang(card);

            int mCard = this.ReadCard();

            node.Value.AddCard(mCard);
            A1022Response.Builder response = new A1022Response.Builder();
            response.Uid = uid;
            response.Card = mCard;
            response.BCard = card;
            response.BuZhangType = 0;

            byte[] responseArray = response.Build().ToByteArray();

            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1022, responseArray);
            }

            actionStorage.PushBuZhang(uid, card, mCard);

        }

        public void MingBuzhang(string uid, int card, string desUid)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            foreach (var row in Players)
            {
                if ((row.CheckHu(card)) && row.PlayerUser.Uid != uid && row.PlayerUser.Uid != desUid)
                {
                    row.ResetEvent.WaitOne();
                }
            }

            if (node.Value.ResetEvent.WaitOne(0))
                return;

            //牌未被胡
            if (this.OutCardState == OutCardState.Normal)
            {
                this.OutCardState = OutCardState.Gang;

                node.Value.BuZhang(card);

                int mCard = this.ReadCard();
                node.Value.AddCard(mCard);
                A1022Response.Builder response = new A1022Response.Builder();
                response.Uid = uid;
                response.Card = mCard;
                response.BCard = card;

                byte[] responseArray = response.Build().ToByteArray();

                foreach (var row in this.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1022, responseArray);
                }
                actionStorage.PushBuZhang(uid, card, mCard);
            }
        }


        public void MoHaidi(string uid)
        {
            this.OutCardState = OutCardState.haidi;
            this.SetAllResetEvent();
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            int card = this.cardArray[this.CardIndex];

            node.Value.HaidiPai = card;
            A1017Response.Builder rep1017 = A1017Response.CreateBuilder();
            rep1017.Uid = node.Value.PlayerUser.Uid;
            rep1017.Card = card;
            var rep1017Array = rep1017.Build().ToByteArray();
            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != uid)
                {
                    row.HaidiPai = -1;
                }

                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1017, rep1017Array);
            }

            actionStorage.PushMo(uid, card);
        }

        public void DaHaidi(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);

            this.OutCardState = OutCardState.Normal;

            A1024Response.Builder a1024Response = A1024Response.CreateBuilder();
            a1024Response.Card = card;
            a1024Response.Uid = uid;

            byte[] responseArray = a1024Response.Build().ToByteArray();

            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != uid)
                {
                    row.HaidiPai = -1;
                }

                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1024, responseArray);

                actionStorage.PushDa(uid, card);
            }

            foreach (var row in Players)
            {
                if (row.PlayerUser.Uid != uid && row.CheckHu(card))
                {
                    row.ResetEvent.WaitOne();
                }
            }

            if (this.OutCardState != OutCardState.Normal)
                return;

            A1018Response.Builder a1018Response = A1018Response.CreateBuilder();

            byte[] response1018Array = a1018Response.Build().ToByteArray();

            foreach (var row in Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1018, response1018Array);
            }
            this.EndGame();

        }

        public void GuoHaidi(string uid)
        {
            this.OutCardState = OutCardState.haidi;
            this.SetAllResetEvent();
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            this.haiGuoNumber++;

            //都没人要留局
            if (this.haiGuoNumber == this.Players.Count)
            {
                A1018Response.Builder rep1018 = A1018Response.CreateBuilder();
                rep1018.Card = this.cardArray[this.CardIndex]; ;
                var rep1018Array = rep1018.Build().ToByteArray();
                foreach (var row in Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1018, rep1018Array);
                }
                this.EndGame();
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


        private void SendA1013Response(string uid, Tuple<int, int> niao, string niaoUid1, string niaoUid2)
        {
            A1013Response.Builder response = A1013Response.CreateBuilder();
            response.Uid = uid;
            response.NiaoCard1 = niao.Item1;
            response.NiaoCard2 = niao.Item2;
            response.NiaoUid1 = niaoUid1;
            response.NiaoUid2 = niaoUid2;

            foreach (var row in this.Players)
            {
                var builder = A1013User.CreateBuilder();
                builder.Uid = row.PlayerUser.Uid;
                builder.Score = row.AddScore - row.SubScore;
                builder.TotalScore = row.Score;
                builder.HuType = row.GetAllHuType();
                response.AddUsers(builder);

            }

            var sendByte = response.Build().ToByteArray();
            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1013, sendByte);
            }
        }


        private void SendA1021Response(Tuple<int, int> niao, string niaoUid1, string niaoUid2, string desUid)
        {
            A1021Response.Builder a1021Response = A1021Response.CreateBuilder();

            a1021Response.NiaoCard1 = niao.Item1;
            a1021Response.NiaoCard2 = niao.Item2;
            a1021Response.DesUid = desUid;
            a1021Response.NiaoUid1 = niaoUid1;
            a1021Response.NiaoUid2 = niaoUid2;

            foreach (var row in this.Players)
            {
                A1021User.Builder user1 = A1021User.CreateBuilder();
                user1.HuType = row.GetAllHuType();
                user1.Uid = row.PlayerUser.Uid;
                user1.Score = row.AddScore - row.SubScore;
                user1.TotalScore = row.Score;
                a1021Response.AddUsers(user1);
            }


            byte[] a1012Array = a1021Response.Build().ToByteArray();

            foreach (var row in this.Players)
            {
                WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1021, a1012Array);
            }
        }


        private void EndGame()
        {
            this.IsEnd = true;

            foreach (var row in this.Players)
            {
                Guid actionId = Guid.NewGuid();
                GameResult result = new GameResult();
                result.ActionId = actionId;
                result.AddScore = row.AddScore;
                result.DateTime = double.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                result.Uid = row.PlayerUser.Uid;
                RedisContext.GlobalContext.AddSortedSetEntity(result);
                actionStorage.ActionId = actionId;
                RedisContext.GlobalContext.AddHashEntity(actionStorage);
            }
        }
    }
}
