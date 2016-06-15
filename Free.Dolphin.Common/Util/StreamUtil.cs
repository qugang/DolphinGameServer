using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Common.Util
{
    public static class StreamUtil
    {
        public static string ReadStringToEnd(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
