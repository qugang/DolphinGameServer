using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service
{

    public class CsGamePlayer
    {
        public int[] Cards { get; set; }
        public IGameUser PlayerSession { get; set; }
        public CsGamePlayer(IGameUser gameSession)
        {
            this.PlayerSession = gameSession;
            this.IsReady = true;
        }

        public Boolean IsReady { get; set; }

        public Boolean CkeckPeng()
        {
            return false;
        }

        public Boolean CheckChi()
        {
            return false;
        }

        public Boolean CheckGang()
        {
            return false;
        }
    }
}
