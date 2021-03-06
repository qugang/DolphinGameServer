﻿using DolphinServer.Entity;
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
    /// 游戏准备
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1005)]
    [ControllerAuth]
    public class Controller1005 : ControllerBase
    {
        public Controller1005(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            CsGameRoomManager.Ready(int.Parse(Context.HttpQueryString["RoomID"]), Context.Session.User as GameUser);
            return null;
        }
    }
}
