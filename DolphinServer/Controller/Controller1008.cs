using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 胡牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1008)]
    [ControllerAuth]
    public class Controller1008 : ControllerBase
    {
        public Controller1008(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            throw new NotImplementedException();
        }
    }
}
