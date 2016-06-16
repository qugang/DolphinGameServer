using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Entity
{

    [RedisTableAttribute(), Serializable]
    public class GameUser : IGameUser
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string WeChatID { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Pwd { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public DateTime OnlimeDate { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public Guid Sid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public GameUserState UserState { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public String Name { get; set; }
        
        IGameUser IGameUser.Login(string uid, string pwd)
        {
            if (uid != null)
            {
                GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(uid);
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
