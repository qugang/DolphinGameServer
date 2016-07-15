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
    /// 海底过
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1017)]
    [ControllerAuth]
    public class Controller1017 : ControllerBase
    {
        public Controller1017(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.GuoHaidi(this.Context.Session.User.Uid);
            return null;
        }
    }
}
