using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service;
using DolphinServer.Service.Mj;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 加入房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1003)]
    [ControllerAuth]
    public class Controller1003 : ControllerBase
    {
        public Controller1003(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            CsMjGameRoom room = CsGameRoomManager.JoinRoom(Context.Session.User as GameUser, int.Parse(Context.HttpQueryString["RoomID"]));

            A1003Response.Builder response = A1003Response.CreateBuilder();

            if (room != null)
            {
                foreach (var row in room.Players)
                {
                    var builder = A1003User.CreateBuilder();
                    builder.Uid = row.PlayerUser.Uid;
                    builder.Sore = 1000;
                    builder.HatImage = "";
                    builder.Sex = 0;
                    response.AddUsers(builder.Build());
                }

                foreach (var row in room.Players)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(row.PlayerUser.Uid, 1003, response.Build().ToByteArray());
                }
                return null;
            }
            else
            {
                A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                error.ErrorCode = 2;
                error.ErrorInfo = ErrorInfo.ErrorDic[2];
                return error.Build().ToByteArray();
            }
        }
        
    }
}
