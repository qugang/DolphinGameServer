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
            CsMjGameRoom room = CsGameRoomManager.CreateRoom(Context.Session.User as GameUser);
            A1002Response.Builder response = A1002Response.CreateBuilder();
            response.RoomID = room.RoomId;
            response.RoomType = int.Parse(Context.HttpQueryString["RoomType"]);
            

            return response.Build().ToByteArray();
        }
    }
}
