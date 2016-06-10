using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinDB.Redis;
using DolphinNetWork;

namespace DolphinServer.Entity
{

    [RedisTableAttribute(), Serializable]
    public class GameUser : IGameUser
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Pwd { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public DateTime OnlimeDate { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public Guid Sid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public GameUserState UserState { get; set; }
    }
}
