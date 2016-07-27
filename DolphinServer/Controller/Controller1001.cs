using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service.Mj;
using Free.Dolphin.Core;
using Free.Dolphin.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1001)]
    public class Controller1001 : ControllerBase
    {
        public Controller1001(ControllerContext context) : base(context)
        {

        }

        public override byte[] ProcessAction()
        {
            GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.HttpQueryString["Uid"].ToString());
            A1001Response.Builder response = A1001Response.CreateBuilder();
            if (user == null)
            {
                user = new GameUser();
                user.Uid = Context.HttpQueryString["Uid"].ToString();
                user.OnlimeDate = DateTime.Now;
                RedisContext.GlobalContext.AddHashEntity(user);
            }
            else
            {
                CsMjGameRoom room =  CsGameRoomManager.GetRoomByUserId(user.Uid);

                if (room != null)
                {
                    response.RoomId = room.RoomId;
                    response.RoomType = room.RoomType;

                    foreach (var row in room.Players)
                    {

                        A1001User.Builder roomUser = A1001User.CreateBuilder();
                        roomUser.Uid = row.PlayerUser.Uid;
                        roomUser.AddRangeChiCard(row.ChiCards);
                        roomUser.AddRangePengCard(row.PengCards);
                        roomUser.AddRangeGangCard(row.GangCards);

                        roomUser.AddRangeWArray(row.wCards);
                        roomUser.WNumber = row.wNumber;
                        roomUser.WTotalNumber = row.twNumber;


                        roomUser.AddRangeTArray(row.tCards);
                        roomUser.TNumber = row.tNumber;
                        roomUser.TTotalNumber = row.ttNumber;


                        roomUser.AddRangeSArray(row.sCards);
                        roomUser.SNumber = row.sNumber;
                        roomUser.STotalNumber = row.tsNumber;

                        roomUser.HuType = row.HuType;
                        roomUser.Score = row.Score;
                        response.AddUser(roomUser);
                    }
                }

                response.JuShu = room.JuShu;
                response.ZhangShu = room.CardIndex + 2;

            }
            Context.Session.User = user;
            GameUserManager.AddOrUpdateUser(user.Uid, Context.Session);
            response.Uid = user.Uid;

           

            return response.Build().ToByteArray();
        }
    }
}
