using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 查询周排行榜
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1011)]
    [ControllerAuth]
    public class Controller1011 : ControllerBase
    {
        public Controller1011(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            A1009Response.Builder response = A1009Response.CreateBuilder();
            //TODO:添加头像 与查询效率问题
            foreach (var row in RedisContext.GlobalContext.FindSoredEntity<GameRankMonth>(10))
            {
                GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(row.Uid);
                var rank = Rank.CreateBuilder();
                rank.Uid = user.Uid;
                rank.Score = row.Score;
                response.AddRanks(rank);
            }
            return response.Build().ToByteArray();
        }
    }
}
