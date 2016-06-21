using DolphinServer.ProtoEntity;
using DolphinServer.Service;
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
            Boolean isSucess = CsGameRoomManager.JoinRoom(Context.Session.User, int.Parse(Context.HttpQueryString["RoomID"]));

            A1002Response.Builder response = A1002Response.CreateBuilder();
            if (isSucess)
            {
                CsMjGameRoom room = CsGameRoomManager.CreateRoom(Context.Session.User);
                return response.Build().ToByteArray();
            }
            else
            {
                response.ErrorCode = 2;
                response.ErrorInfo = ErrorInfo.ErrorDic[2];
                return response.Build().ToByteArray();
            }
        }
    }
}
