using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public class ControllerContext
    {

        internal ControllerContext(Dictionary<string, string> keyValue)
        {
            this.HttpQueryString = keyValue;
            this.ProtocolId = int.Parse(HttpQueryString["ProtocolId"]);
            this.Sign = HttpQueryString["Sign"].ToString();
        }
        public int ProtocolId { get; set; }


        public string Sign { get; set; }

        public GameSession Session { get; set; }

        public Dictionary<string, string> HttpQueryString { get; set; }
    }
}
