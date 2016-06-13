using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 加入房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1003)]
    public class Controller1003 : ControllerBase
    {
        public Controller1003(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            throw new NotImplementedException();
        }
    }
}
