using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinDB.Redis
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
