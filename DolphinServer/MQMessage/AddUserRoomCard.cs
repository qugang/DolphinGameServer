using DolphinServer.Entity;
using DolphinServer.ProtoEntity;
using Free.Dolphin.Common;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.MQMessage
{
    [Serializable]
    public class AddUserRoomCard : IMQMessage
    {
        public string ManagerUid { get; set; }
        public string Uid { get; set; }
        public int RoomCard { get; set; }

        public void Process()
        {
            A1025Response.Builder response = A1025Response.CreateBuilder();

            response.Uid = this.Uid;
            response.RoomCard = this.RoomCard;

            WebSocketServerWrappe.SendPackgeWithUser(this.Uid, 1025, response.Build().ToByteArray());
        }
    }
}
