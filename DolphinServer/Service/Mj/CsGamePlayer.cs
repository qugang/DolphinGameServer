using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{

    public class CsGamePlayer : MjGamePlayerBase
    {
        public CsGamePlayer(GameUser gameSession) : base(gameSession)
        {
        }
    }
}
