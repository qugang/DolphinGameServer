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
    /// 第一次打牌
    /// 因为开局打牌需要判断是否胡缺一色等,所以走不同的协议
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1007)]
    [ControllerAuth]
    public class Controller1007 : ControllerBase
    {
        public Controller1007(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            int card = int.Parse(Context.HttpQueryString["Card"]);
            string uid = Context.Session.User.Uid;
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);

            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            room.FristDa(uid, card);
            

            return null;
        }
    }
}
