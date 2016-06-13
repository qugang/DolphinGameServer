using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{

    [RedisTableAttribute(), Serializable]
    public class GameRoom
    {

        /// <summary>
        /// 房间号
        /// </summary>
        [RedisColumn(RedisColumnType.RedisKey)]
        public string RoomId { get; set; }

        /// <summary>
        /// 玩家集合
        /// </summary>
        [RedisColumn(RedisColumnType.RedisColumn)]
        public List<String> UserID { get; set; }

        /// <summary>
        /// 管理者
        /// </summary>
        [RedisColumn(RedisColumnType.RedisColumn)]
        public string ManagerUser { get; set; }

    }
}
