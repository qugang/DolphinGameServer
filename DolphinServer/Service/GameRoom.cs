using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service
{
    public class GameRoom
    {

        /// <summary>
        /// 房间号
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 玩家集合
        /// </summary>
        public List<GameSession> UserID { get; set; }
        
    }
}
