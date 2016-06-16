using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service;
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
            GameRoom room = GameRoomManager.CreateRoom(Context.Session);
            A1002Response.Builder response = A1002Response.CreateBuilder();
            response.RoomID = room.RoomId;
            return response.Build().ToByteArray();
        }
    }
}
