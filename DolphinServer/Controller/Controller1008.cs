﻿using DolphinServer.Service.Mj;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{

    [ControllerProtocol((int)ControllerType.Controller1008)]
    [ControllerAuth]
    public class Controller1008 : ControllerBase
    {
        public Controller1008(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            string uid = Context.Session.User.Uid;
            int roomId = int.Parse(Context.HttpQueryString["RoomID"]);
            int huType = int.Parse(Context.HttpQueryString["HuType"]);
            CsMjGameRoom room = CsGameRoomManager.GetRoomById(roomId);
            return null;
        }
    }
}
