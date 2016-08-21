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
    /// 海底打
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1021)]
    [ControllerAuth]
    public class Controller1021 : ControllerBase
    {
        public Controller1021(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int card = int.Parse(Context.HttpQueryString["Card"]);
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.DaHaidi(Context.Session.User.Uid, card);
            return null;
        }
    }
}
