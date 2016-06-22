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
            Player down = Player.CreateBuilder().AddRangeCard(cardArray.Take(14)).Build();
            Player rigth = Player.CreateBuilder().AddRangeCard(cardArray.Skip(14).Take(13)).Build();
            Player top = Player.CreateBuilder().AddRangeCard(cardArray.Skip(27).Take(13)).Build();
            Player left = Player.CreateBuilder().AddRangeCard(cardArray.Skip(40).Take(13)).Build();

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

            WebSocketServer.SendPackgeWithUser(this.player.Value.PlayerUser.Uid, 1006, response.Build().ToByteArray());
            //TODO： 测试先发一个人

            //WebSocketServer.SendPackgeWithUser(this.player.Next.Value.PlayerUser.Uid, 1006, response1.Build().ToByteArray());
            //WebSocketServer.SendPackgeWithUser(this.player.Next.Next.Value.PlayerUser.Uid, 1006, response2.Build().ToByteArray());
            //WebSocketServer.SendPackgeWithUser(this.player.Next.Next.Value.PlayerUser.Uid, 1006, response3.Build().ToByteArray());

        }
        
        /// <summary>
        /// 出牌接收
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="card">牌</param>
        public void OutCard(string uid,int card) {

            foreach (var row in this.players)
            {
                if(row.u)
            }

            var player = this.players.Where(p => p.PlayerUser.Uid == uid).FirstOrDefault();
            player.OutCard(card);
            player.
            
        }
    }
}
