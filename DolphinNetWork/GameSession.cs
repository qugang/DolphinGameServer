using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public class GameSession
    {
        public Guid SessionId { get; set; }

        public GameSessionState SessionState { get; set; }
        public static GameSession NewSession()
        {
            return new GameSession();
        }
        public static GameSession Parse(Guid session)
        {
            return new GameSession { SessionId = session, SessionState = GameSessionState.OnLine };
        }
    }
}
