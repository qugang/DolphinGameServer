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
    /// 杠打牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1019)]
    [ControllerAuth]
    public class Controller1019 : ControllerBase
    {
        public Controller1019(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            string desUid = Context.HttpQueryString["DesUid"];
            int card1 = int.Parse(Context.HttpQueryString["Card1"]);
            int card2 = int.Parse(Context.HttpQueryString["Card2"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.ZhuoPaoResult(this.Context.Session.User.Uid, desUid,card1,card2);
            return null;
        }
    }
}
