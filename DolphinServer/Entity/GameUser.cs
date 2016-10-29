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
    public class GameUser : IGameUser
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Pwd { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string OpenId { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string NickName { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int Sex { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Province { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string City { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string Country { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public string HeadImgurl { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int FriendNumber { get; set; }


        [RedisColumn(RedisColumnType.RedisColumn)]
        public int RoomCard { get; set; }


        [RedisColumn(RedisColumnType.RedisColumn)]
        public int IsLingQu { get; set; }
        


        [RedisColumn(RedisColumnType.RedisColumn)]
        public List<string> YaoUid { get; set; }


        public Guid Sid
        {
            get;set;
        }

        public DateTime OnlimeDate
        {
            get;set;
        }

        public GameUserState UserState
        {
            get;set;
        }

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
