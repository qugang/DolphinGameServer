using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public class ControllerContext
    {
        internal ControllerContext(Dictionary<string, string> keyValue)
        {
            this.HttpQueryString = keyValue;
            this.ProtocolNumber = int.Parse(HttpQueryString["ProtocolId"]);
            this.Sid = Guid.Parse(HttpQueryString["Sid"].ToString());
            this.Uid = HttpQueryString["Uid"].ToString();
            this.Sign = HttpQueryString["Sign"].ToString();
            this.Pwd = HttpQueryString["Pwd"].ToString();

            //TODO IGameUser如何初始化

        }
        public int ProtocolNumber { get; set; }

        public Guid Sid { get; set; }

        public string Uid { get; set; }

        public string Pwd { get; set; }

        public string Sign { get; set; }

        public GameSession Session { get; set; }

        public Dictionary<string, string> HttpQueryString { get; set; }
    }
}
