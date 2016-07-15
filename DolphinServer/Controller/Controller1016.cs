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
    /// 海底摸牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1016)]
    [ControllerAuth]
    public class Controller1016 : ControllerBase
    {
        public Controller1016(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.MoHaidi(Context.Session.User.Uid);
            return null;
        }
    }
}
