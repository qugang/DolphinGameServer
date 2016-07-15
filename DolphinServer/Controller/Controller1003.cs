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
            response.RoomID = int.Parse(Context.HttpQueryString["RoomID"]);
            response.RoomType = int.Parse(Context.HttpQueryString["RoomType"]);

            if (room != null)
            {
                var player = room.Players.First;
                int playerLen = room.Players.Count;
                for (int i = 0; i < playerLen; i++)
                {

                    var builder = A1003User.CreateBuilder();
                    builder.Uid = player.Value.PlayerUser.Uid;
                    builder.Sore = 1000;
                    builder.HatImage = "";
                    builder.Sex = 0;
                    response.AddUsers(builder.Build());
                    player = player.Next;
                }

                player = room.Players.First;

                for (int i = 0; i < playerLen; i++)
                {
                    WebSocketServerWrappe.SendPackgeWithUser(player.Value.PlayerUser.Uid, 1003, response.Build().ToByteArray());
                    player = player.Next;
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
