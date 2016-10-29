using DolphinServer.Controller;
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

[ControllerProtocol((int)ControllerType.Controller1026)]
[ControllerAuth]
public class Controller1026 : ControllerBase
{
    public Controller1026(ControllerContext context) : base(context)
    {
    }

    public override byte[] ProcessAction()
    {

        GameUser user = RedisContext.GlobalContext.FindHashEntityByKey<GameUser>(Context.Session.User.Uid);

        int prizeType = int.Parse(Context.HttpQueryString["prizeType"].ToString());

        if (prizeType == 0)
        {
            if ((user.IsLingQu & 1) != 0 && user.FriendNumber >= 1)
            {
                user.RoomCard += 2;
                user.IsLingQu |= 1;
            }
        }
        if (prizeType == 1)
        {
            if ((user.IsLingQu & 2) != 0 && user.FriendNumber >= 2)
            {
                user.RoomCard += 4;
                user.IsLingQu |= 2;
            }
        }
        if (prizeType == 2)
        {
            if ((user.IsLingQu & 4) != 0 && user.FriendNumber >= 3)
            {
                user.RoomCard += 6;
                user.IsLingQu |= 4;
            }
        }
        if (prizeType == 3)
        {
            if ((user.IsLingQu & 8) != 0 && user.FriendNumber >= 4)
            {
                user.RoomCard += 8;
                user.IsLingQu |= 8;
            }
        }
        if (prizeType == 4)
        {
            if ((user.IsLingQu & 16) != 0 && user.FriendNumber >= 5)
            {
                user.RoomCard += 12;
                user.IsLingQu |= 16;
            }
        }
        RedisContext.GlobalContext.AddHashEntity(user);

        A1025Response.Builder response = A1025Response.CreateBuilder();
        response.RoomCard = user.RoomCard;
        response.FriendNumber = user.FriendNumber;

        WebSocketServerWrappe.SendPackgeWithUser(Context.Session.User.Uid, 1025, response.Build().ToByteArray());


        return null;
    }
}