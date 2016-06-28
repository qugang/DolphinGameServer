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
    /// 过牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1009)]
    [ControllerAuth]
    public class Controller1009 : ControllerBase
    {
        public Controller1009(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            string uid = Context.Session.User.Uid;
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.Guo(uid);

            return null;
        }
    }
}
