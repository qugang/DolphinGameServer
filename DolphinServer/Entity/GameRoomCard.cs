using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{
    [RedisTableAttribute(), Serializable]
    public class GameRoomCardLs
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string LsID { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string ManagerUser { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string DesUser { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int RoomCard { get; set; }
    }
}
