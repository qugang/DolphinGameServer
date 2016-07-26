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
    /// 碰
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1011)]
    [ControllerAuth]
    public class Controller1011 : ControllerBase
    {
        public Controller1011(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int card = int.Parse(Context.HttpQueryString["Card"]);
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            string desUid = Context.HttpQueryString["DesUid"];
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.Peng(Context.Session.User.Uid,desUid,card);
            return null;
        }
    }
}
