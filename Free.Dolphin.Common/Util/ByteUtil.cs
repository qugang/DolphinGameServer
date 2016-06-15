using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Common.Util
{
    public static class ByteUtil
    {
        public static string ByteToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in bytes)
            {
                sb.Append(Convert.ToString(row, 16));
            }
            return sb.ToString();
        }
    }
}
