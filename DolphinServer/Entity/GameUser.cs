using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinDB.Redis;

namespace DolphinServer.Entity
{

    [RedisTableAttribute(), Serializable]
    public class GameUser
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string UserID { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string UserName { get; set; }
    }
}
