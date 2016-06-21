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
    ///  发牌
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1006)]
    [ControllerAuth]
    public class Controller1006 : ControllerBase
    {
        public Controller1006(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            //var cards = (this.Context.ObjectContext as int[]);

            //A1006Response.Builder response = A1006Response.CreateBuilder();
            //response.Player1 = Player.CreateBuilder().AddRangeCard(cards.Take(14)).Build();
            //response.Player2 = Player.CreateBuilder().AddRangeCard(cards.Skip(14).Take(13)).Build();
            //response.Player3 = Player.CreateBuilder().AddRangeCard(cards.Skip(27).Take(13)).Build();
            //response.Player4 = Player.CreateBuilder().AddRangeCard(cards.Skip(40).Take(13)).Build();

            // return response.Build().ToByteArray();
            return null;
        }
    }
}
