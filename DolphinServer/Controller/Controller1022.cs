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
    /// 重新开始游戏
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1022)]
    [ControllerAuth]
    public class Controller1022 : ControllerBase
    {
        public Controller1022(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            return null;
        }
    }
}
