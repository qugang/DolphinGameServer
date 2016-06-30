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
    /// 吃
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1010)]
    [ControllerAuth]
    public class Controller1010 : ControllerBase
    {
        public Controller1010(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int card = int.Parse(Context.HttpQueryString["Card"]);
            int card1 = int.Parse(Context.HttpQueryString["Card1"]);
            int card2 = int.Parse(Context.HttpQueryString["Card2"]);
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.Chi(Context.Session.User.Uid, card, card1, card2);
            return null;
        }
    }
}
