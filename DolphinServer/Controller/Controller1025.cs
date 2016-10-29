using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service.Mj.ActionStorage;
using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    [ControllerProtocol((int)ControllerType.Controller1025)]
    [ControllerAuth]
    public class Controller1025 : ControllerBase
    {
        public Controller1025(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {

            GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.HttpQueryString["YaoUid"].ToString());
            GameUser myUser = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.Session.User.Uid);
            if (user != null)
            {

                if (myUser.YaoUid == null)
                {
                    user.YaoUid.Add(Context.HttpQueryString["YaoUid"].ToString());
                    user.FriendNumber++;

                    RedisContext.GlobalContext.AddHashEntity(user);
                }
                else if (myUser.YaoUid != null && !myUser.YaoUid.Contains(Context.HttpQueryString["YaoUid"].ToString()))
                {
                    user.YaoUid.Add(Context.HttpQueryString["YaoUid"].ToString());
                    user.FriendNumber++;
                    RedisContext.GlobalContext.AddHashEntity(user);
                }
                else
                {
                    A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                    error.ErrorCode = 6;
                    error.ErrorInfo = "该邀请码已输入过";
                    WebSocketServerWrappe.SendPackgeWithUser(Context.Session.User.Uid, 9999, error.Build().ToByteArray());
                    return null;
                }

            }
            else
            {
                A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                error.ErrorCode = 5;
                error.ErrorInfo = "邀请码不存在";
                WebSocketServerWrappe.SendPackgeWithUser(Context.Session.User.Uid, 9999, error.Build().ToByteArray());
                return null;
            }
            return null;
        }
    }
}
