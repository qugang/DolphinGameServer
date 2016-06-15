using DolphinServer.Entity;
using DolphinServer.Service;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 创建房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1002)]
    public class Controller1002 : ControllerBase
    {
        public Controller1002(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            GameRoom room = GameRoomManager.CreateRoom(Context.Session);
            

            throw new NotImplementedException();
        }
    }
}
