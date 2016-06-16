using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    ///  发牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1006)]
    [ControllerAuth]
    public class Controller1006 : ControllerBase
    {
        public Controller1006(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            throw new NotImplementedException();
        }
    }
}
