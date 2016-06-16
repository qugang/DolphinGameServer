using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 摸牌
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
            throw new NotImplementedException();
        }
    }
}
