using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 打牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1009)]
    [ControllerAuth]
    public class Controller1009 : ControllerBase
    {
        public Controller1009(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {

        }
    }
}
