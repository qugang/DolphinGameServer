using DolphinServer.Entity;
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
            GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.Uid);
            if (user == null)
            {
                user = new GameUser();
                user.Uid = Context.Uid;
                user.Pwd = Context.Pwd;
                user.Sid = Context.Sid;
                user.OnlimeDate = DateTime.Now;
                RedisContext.GlobalContext.AddHashEntity(user);
            }
            return null;
        }
    }
}
