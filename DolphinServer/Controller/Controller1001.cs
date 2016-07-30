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


                    A9997ClientConnectionResponse.Builder a9997Response =
                        A9997ClientConnectionResponse.CreateBuilder();

                    a9997Response.Uid = user.Uid;

                    byte[] a9997Array = a9997Response.Build().ToByteArray();

                    foreach (var row in room.Players)
                    {

                        A1001User.Builder roomUser = A1001User.CreateBuilder();
                        roomUser.Uid = row.PlayerUser.Uid;

                        if (row.ChiCards != null && row.ChiCards.Count > 0)
                        {
                            roomUser.AddRangeChiArray(row.ChiCards);
                        }

                        if (row.PengCards != null && row.PengCards.Count > 0)
                        {
                            roomUser.AddRangePengArray(row.PengCards);
                        }
                        if (row.GangCards != null && row.GangCards.Count > 0)
                        {
                            roomUser.AddRangeGangArray(row.GangCards);
                        }
                        if (row.wCards != null && row.wCards.Count > 0)
                        {
                            roomUser.AddRangeWArray(row.wCards);
                            roomUser.WNumber = row.wNumber;
                            roomUser.WTotalNumber = row.twNumber;
                        }

                        if (row.tCards != null && row.tCards.Count > 0)
                        {
                            roomUser.AddRangeTArray(row.tCards);
                            roomUser.TNumber = row.tNumber;
                            roomUser.TTotalNumber = row.ttNumber;
                        }


                        if (row.sCards != null && row.sCards.Count > 0)
                        {
                            roomUser.AddRangeSArray(row.sCards);
                            roomUser.SNumber = row.sNumber;
                            roomUser.STotalNumber = row.tsNumber;
                        }

                        if (row.zhuoCards != null && row.zhuoCards.Count > 0)
                        {
                            roomUser.AddRangeOutCard(row.sCards);
                        }

                        roomUser.HuType = row.HuType;
                        roomUser.Score = row.Score;
                        roomUser.Sore = row.Score;
                        response.AddUsers(roomUser);

                        WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 9997, a9997Array);

                    }

                    if (room.JuShu != 0)
                    {
                        response.JuShu = room.JuShu;
                    }
                    if (room.CardIndex != 0)
                    {
                        response.ZhangShu = room.CardIndex + 2;
                    }
                }
            }
            Context.Session.User = user;
            GameUserManager.AddOrUpdateUser(user.Uid, Context.Session);
            response.Uid = user.Uid;

           

            return response.Build().ToByteArray();
        }
    }
}
