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
    [ControllerProtocol((int)ControllerType.Controller1024)]
    [ControllerAuth]
    public class Controller1024 : ControllerBase
    {
        public Controller1024(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            GameActionStoreage ga = RedisContext.GlobalContext.FindHashEntityByKey<GameActionStoreage>(Context.HttpQueryString["ResultActionId"]);

            A1027Response.Builder a1027Respose = A1027Response.CreateBuilder();

            foreach (var row in ga.LPlayer)
            {
                A1027GameUser.Builder gameuser = A1027GameUser.CreateBuilder();

                GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(row.Uid);
                gameuser.Uid = row.Uid;
                gameuser.HatImage = user.HeadImgurl;
                gameuser.Sex = user.Sex;
                gameuser.Name = user.NickName;
                gameuser.AddRangeCards(row.Cards);
                a1027Respose.AddGameUser(gameuser);
            }

            foreach (var row in ga.CmdList)
            {
                A1027CmdList.Builder cmdList = A1027CmdList.CreateBuilder();
                cmdList.Uid = row.Uid;
                cmdList.AType = (int)row.AType;
                cmdList.Card = row.Card;
                cmdList.Card1 = row.Card1;
                cmdList.Card2 = row.Card2;
                cmdList.HuType = row.HuType;
                a1027Respose.AddCmdList(cmdList);
            }

            WebSocketServerWrappe.SendPackgeWithUser(this.Context.Session.User.Uid, 1027, a1027Respose.Build().ToByteArray());



            return null;


        }
    }
}
