using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    internal class WebSocketPackage
    {
        internal static Dictionary<string, string> UnPackage(string context)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string str = System.Web.HttpUtility.UrlDecode(context.Replace("?d=", ""));

            foreach (var row in str.Split('&'))
            {
                string[] keyValue = row.Split('=');
                dic.Add(keyValue[0], keyValue[1]);
            }
            return dic;
        }
    }
}
