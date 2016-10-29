using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Common
{
    public class RedisColumnAttribute : Attribute
    {
        public RedisColumnType ColumnType { get; private set; }
        public RedisColumnAttribute(RedisColumnType type)
        {
            this.ColumnType = type;
        }
    }
}
