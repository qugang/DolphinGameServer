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
    /// 解散房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1004)]
    [ControllerAuth]
    public class Controller1004 : ControllerBase
    {
        public Controller1004(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            string uid = Context.Session.User.Uid;
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            int cancelType = int.Parse(Context.HttpQueryString["CancelType"]);
           
            CsGameRoomManager.Cancel(uid, roomId,cancelType);
            return null;
        }
    }
}
