
using Free.Dolphin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{
    [RedisTableAttribute(), Serializable]
    public class GameManagerUser
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Pwd { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int MaxCardRoom { get; set; }

        
    }
}
