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
    /// 补张
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1020)]
    [ControllerAuth]
    public class Controller1020 : ControllerBase
    {
        public Controller1020(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int card = int.Parse(Context.HttpQueryString["Card"]);
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            string desUid = Context.HttpQueryString.ContainsKey("DesUid") ? Context.HttpQueryString["DesUid"] : null;
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            if (desUid != null)
            {
                room.MingBuzhang(Context.Session.User.Uid,card, desUid);
            }
            else
            {
                room.AnBuZhang(Context.Session.User.Uid, card);
            }
            return null;
        }
    }
}
