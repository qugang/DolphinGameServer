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
    /// 用户登录
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1001)]
    public class Controller1001 : ControllerBase
    {
        public Controller1001(ControllerContext context) : base(context)
        {

        }

        public override byte[] ProcessAction()
        {
            GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.HttpQueryString["Uid"].ToString());
            if (user == null)
            {
                user = new GameUser();
                user.Uid = Context.HttpQueryString["Uid"].ToString();
                user.OnlimeDate = DateTime.Now;
                RedisContext.GlobalContext.AddHashEntity(user);
            }
            Context.Session.User = user;
            A1001Response.Builder response = A1001Response.CreateBuilder();
            response.Uid = user.Uid;
            return response.Build().ToByteArray();
        }
    }
}
