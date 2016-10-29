using Free.Dolphin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{
    [RedisTableAttribute(), Serializable]
    public class GameResult
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }
        
        [RedisColumn(RedisColumnType.RedisColumn)]
        public int AddScore { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public Guid ActionId { get; set; }

        [RedisColumn(RedisColumnType.RedisScore)]
        public double DateTime { get; set; }
    }
    
}
