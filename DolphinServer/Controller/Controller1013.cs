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
    /// 自摸
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1013)]
    [ControllerAuth]
    public class Controller1013 : ControllerBase
    {
        public Controller1013(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            int huType = int.Parse(Context.HttpQueryString["HuType"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.Zimo(Context.Session.User.Uid, huType);
            return null;
        }
    }
}
