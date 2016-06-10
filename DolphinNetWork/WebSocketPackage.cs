using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    internal class WebSocketPackage
    {
        internal static Dictionary<string, string> UnPackage(Stream data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(data,Encoding.UTF8);

            string str = sr.ReadToEnd();

            foreach (var row in str.Split('&'))
            {
                string[] keyValue = row.Split('=');
                dic.Add(keyValue[0], keyValue[1]);
            }
            return dic;
        }


    }
}
