using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using DolphinServer.Service;
using DolphinServer.Service.Mj;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    /// <summary>
    /// 加入房间
    /// </summary>
    [ControllerProtocol((int)ControllerType.Controller1003)]
    [ControllerAuth]
    public class Controller1003 : ControllerBase
    {
        public Controller1003(ControllerContext context) : base(context)
        {
        }

        public override byte[] ProcessAction()
        {
            CsMjGameRoom room = CsGameRoomManager.JoinRoom(Context.Session.User as GameUser, int.Parse(Context.HttpQueryString["RoomID"]));

            if (room == null)
            {
                A9999DataErrorResponse.Builder error = A9999DataErrorResponse.CreateBuilder();
                error.ErrorCode = 2;
                error.ErrorInfo = ErrorInfo.ErrorDic[2];
                return error.Build().ToByteArray();
            }
            return null;
        }

    }
}
