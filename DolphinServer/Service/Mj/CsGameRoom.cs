using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    /// <summary>
    /// 长沙麻将房间管理
    /// </summary>
    public class CsMjGameRoom : MjGameRoomBase
    {
        protected override void SendCard()
        {
            Player down = ProtoEntity.Player.CreateBuilder().AddRangeCard(cardArray.Take(14)).Build();
            Player rigth = ProtoEntity.Player.CreateBuilder().AddRangeCard(cardArray.Skip(14).Take(13)).Build();
            Player top = ProtoEntity.Player.CreateBuilder().AddRangeCard(cardArray.Skip(27).Take(13)).Build();
            Player left = ProtoEntity.Player.CreateBuilder().AddRangeCard(cardArray.Skip(40).Take(13)).Build();

            var response = A1006Response.CreateBuilder();
            response.Player1 = down;
            response.Player2 = rigth;
            response.Player3 = top;
            response.Player4 = left;


            var response1 = A1006Response.CreateBuilder();
            response1.Player1 = rigth;
            response1.Player2 = top;
            response1.Player3 = left;
            response1.Player4 = down;

            var response2 = A1006Response.CreateBuilder();
            response1.Player1 = top;
            response1.Player2 = left;
            response1.Player3 = down;
            response1.Player4 = rigth;

            var response3 = A1006Response.CreateBuilder();
            response3.Player1 = left;
            response3.Player2 = down;
            response3.Player3 = rigth;
            response3.Player4 = top;

            WebSocketServer.SendPackgeWithUser(this.Player.Value.PlayerUser.Uid, 1006, response.Build().ToByteArray());
            //TODO： 测试先发一个人

            //WebSocketServer.SendPackgeWithUser(this.player.Next.Value.PlayerUser.Uid, 1006, response1.Build().ToByteArray());
            //WebSocketServer.SendPackgeWithUser(this.player.Next.Next.Value.PlayerUser.Uid, 1006, response2.Build().ToByteArray());
            //WebSocketServer.SendPackgeWithUser(this.player.Next.Next.Value.PlayerUser.Uid, 1006, response3.Build().ToByteArray());

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

        public void Hu(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);
            node.Value.ResetEvent.Set();

            this.Player = this.Player;
        }

        public void Chi(string uid, int card)
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
        }

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
        }

        public void Peng(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);

            foreach (var row in Players)
            {
                if ((row.CheckGang(card) || row.CheckHu(card)) && row.PlayerUser.Uid != uid)
                {
                    row.ResetEvent.WaitOne();
                }
            }
        }

        public void Mo(string uid, int card)
        {
        }

        public void Da(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);


            this.Player = this.Player.Next;


        }

        public void FristDa(string uid, int card)
        {
            LinkedListNode<CsGamePlayer> node = FindPlayer(uid);

            foreach (var row in Players)
            {
                if ((row.CheckKaiJuHu()) && row.PlayerUser.Uid != uid)
                {
                    row.ResetEvent.WaitOne();
                }
            }
        }


        /// <summary>
        /// 出牌接收
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="card">牌</param>
        public void OutCard(string uid, int card, List<int> huArray)
        {
            // var player = this.Player.Where(p => p.PlayerUser.Uid == uid).FirstOrDefault();
            //player.OutCard(card);
        }
    }



}
