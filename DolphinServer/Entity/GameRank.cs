using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{
    [RedisTableAttribute(), Serializable]
    public class GameRankWeek
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisScore)]
        public int Score { get; set; }
    }

    [RedisTableAttribute(), Serializable]
    public class GameRankDay
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisScore)]
        public int Score { get; set; }
    }


    [RedisTableAttribute(), Serializable]
    public class GameRankMonth
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisScore)]
        public int Score { get; set; }
    }
}
