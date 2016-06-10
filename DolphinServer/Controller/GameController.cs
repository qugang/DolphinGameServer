using DolphinNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    [ControllerProtocol(1001)]
    public class GameController : ControllerBase
    {
        public GameController(ControllerContext context) : base(context)
        {

        }

        public override byte[] ProcessAction()
        {
            throw new NotImplementedException();
        }
    }
}
