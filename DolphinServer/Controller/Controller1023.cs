using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service.Mj;
using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1023)]
    [ControllerAuth]
    public class Controller1023 : ControllerBase
    {
        public Controller1023(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            A1026Response.Builder response = A1026Response.CreateBuilder();
            foreach (var row in RedisContext.GlobalContext.FindSoredEntityByKey<GameResult>(this.Context.Session.User.Uid,50))
            {
                A1026GameResult.Builder gr = A1026GameResult.CreateBuilder();
                gr.ResultActionId = row.ActionId.ToString();
                gr.Score = row.AddScore;
                gr.DateTime = row.DateTime.ToString();

                response.AddGResult(gr);
            }


            WebSocketServerWrappe.SendPackgeWithUser(this.Context.Session.User.Uid, 1026, response.Build().ToByteArray());
            

            return null;
        }
    }
}
