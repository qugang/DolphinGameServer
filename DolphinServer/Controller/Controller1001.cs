using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service.Mj;
using Free.Dolphin.Common;
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
            A1001Response.Builder response = A1001Response.CreateBuilder();
            GameUserMapping mapping = RedisContext.GlobalContext.FindHashEntityByKey<GameUserMapping>(Context.HttpQueryString["Uid"].ToString());

            GameUser user = null;

            if (mapping == null)
            {
                user = new GameUser();
                user.RoomCard = 8;
                user.OnlimeDate = DateTime.Now;

                mapping = new GameUserMapping();

                mapping.Uid = Context.HttpQueryString["Uid"].ToString();
                mapping.UserId = RedisContext.GlobalContext.HashLength(mapping);
                RedisContext.GlobalContext.AddHashEntity(mapping);

            }
            else
            {
                user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(mapping.UserId.ToString());
                if (int.Parse(user.OnlimeDate.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.ToString("yyyyMMdd")))
                {
                    user.RoomCard += 2;
                }
            }

            user.OnlimeDate = DateTime.Now;
            user.Uid = mapping.UserId.ToString();
            user.NickName = Context.HttpQueryString["Nickname"].ToString();
            user.Sex = int.Parse(Context.HttpQueryString["Sex"].ToString());
            user.Province = Context.HttpQueryString["Province"].ToString();
            user.City = Context.HttpQueryString["City"].ToString();
            user.Country = Context.HttpQueryString["Country"].ToString();
            user.HeadImgurl = Context.HttpQueryString["Headimgurl"].ToString();

            RedisContext.GlobalContext.AddHashEntity(user);


            CsMjGameRoom room = CsGameRoomManager.GetRoomByUserId(user.Uid);

            if (room != null)
            {
                var currentPlayer = room.FindPlayer(user.Uid);
                response.RoomId = room.RoomId;
                response.RoomType = room.RoomType;

                if (room.Players.Count == 4)
                {
                    response.IsEnd = room.IsEnd ? 1 : 0;
                    if (!room.IsEnd)
                    {
                        response.IsFrist = room.IsFrist ? 1 : 0;
                        response.CurrentUid = room.Player.Value.PlayerUser.Uid;
                        response.AddRangeNeedGangDaPai(currentPlayer.Value.NeedGangDaPai);
                        response.AddRangeCurrentCard(room.CurrentCard);
                        response.HaidiPai = currentPlayer.Value.HaidiPai;
                    }
                }

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
                        roomUser.AddRangeOutCard(row.zhuoCards);
                    }

                    if (row.AnGangCards != null && row.AnGangCards.Count > 0)
                    {
                        roomUser.AddRangeAnGangArray(row.AnGangCards);
                    }
                    if (row.BuZhangCards != null && row.BuZhangCards.Count > 0)
                    {
                        roomUser.AddRangeBuZhangArray(row.BuZhangCards);
                    }


                    roomUser.Name = row.PlayerUser.NickName;
                    roomUser.HatImage = row.PlayerUser.HeadImgurl;
                    roomUser.Sex = row.PlayerUser.Sex;
                    roomUser.HuType = row.HuType;
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
                    response.ZhangShu = room.CardIndex + 1;
                }
            }


            response.Uid = user.Uid;
            response.ImageUrl = user.HeadImgurl;
            response.NickName = user.NickName;
            response.RoomCard = user.RoomCard;
            response.FriendNumber = user.FriendNumber;
            response.IsLingQu = user.IsLingQu;

            Context.Session.User = user;
            GameUserManager.AddOrUpdateUser(user.Uid, Context.Session);
            return response.Build().ToByteArray();
        }
    }
}
