using Free.Dolphin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{
    [RedisTableAttribute(), Serializable]
    public class GameUserMapping
    {

        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public long UserId { get; set; }
        
    }
}
