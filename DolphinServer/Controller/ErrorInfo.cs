using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    public class ErrorInfo
    {
        static ErrorInfo()
        {
            ErrorDic = new Dictionary<int, string>();

            ErrorDic.Add(1, "服务器繁忙");
            ErrorDic.Add(2, "房间号不存在或房间已满");
            ErrorDic.Add(3, "没有可摸的牌，骰子:");
        }

        public static Dictionary<int, string> ErrorDic { get; set; }


    }

}
