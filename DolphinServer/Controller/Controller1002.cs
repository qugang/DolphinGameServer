using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service;
using DolphinServer.Service.Mj;
using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 创建房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1002)]
    [ControllerAuth]
    public class Controller1002 : ControllerBase
    {
        public Controller1002(ControllerContext context) : base(context)
        {

        }

        public override byte[] ProcessAction()
        {

            GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.HttpQueryString["Uid"]);

            if(user.RoomCard <=0)
            {
                A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                error.ErrorCode = 4;
                error.ErrorInfo = ErrorInfo.ErrorDic[4];
                WebSocketServerWrappe.SendPackgeWithUser(Context.Session.User.Uid, 9999, error.Build().ToByteArray());
                return null;
            }

            CsMjGameRoom room = CsGameRoomManager.CreateRoom(user, int.Parse(Context.HttpQueryString["RoomPeopleType"]));
            A1002Response.Builder response = A1002Response.CreateBuilder();
            response.RoomID = room.RoomId;
            response.RoomType = int.Parse(Context.HttpQueryString["RoomType"]);

            return response.Build().ToByteArray();
        }
    }
}
