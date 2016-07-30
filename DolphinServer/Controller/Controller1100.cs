using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
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
    /// 聊天消息接收
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1100)]
    [ControllerAuth]
    public class Controller1100 : ControllerBase
    {
        public Controller1100(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
          
            string message = Context.HttpQueryString["Message"];
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.SendMessage(Context.HttpQueryString["Uid"], message);
            return null;
        }
    }
}
