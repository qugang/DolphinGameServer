using Microsoft.VisualStudio.TestTools.UnitTesting;
using Free.Dolphin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Free.Dolphin.Common.Tests
{
    [TestClass()]
    public class RedisContextTests
    {
        [RedisTableAttribute(), Serializable]
        public class GameUserMapping
        {
            [RedisColumn(RedisColumnType.RedisKey)]
            public long UserId { get; set; }

            [RedisColumn(RedisColumnType.RedisColumn)]
            public string Uid { get; set; }
        }
        [TestMethod()]
        public void IncrementTest()
        {
            RedisContext.InitRedisContext("localhost,allowAdmin=true", Assembly.GetAssembly(typeof(RedisContextTests)));

            RedisContext.GlobalContext.DeleteAllHashKey("GameUser");
        }

        [TestMethod()]
        public void FindSoredEntityTest()
        {
            RedisContext.InitRedisContext("localhost,allowAdmin=true", Assembly.GetAssembly(typeof(RedisContextTests)));

            // RedisContext.RedisDb.SortedSetAdd("123456", new StackExchange.Redis.SortedSetEntry[] { new StackExchange.Redis.SortedSetEntry("Fuck",1.1) });
            GameResult result = new GameResult();
            result.Uid = "1";
            result.ActionId = Guid.NewGuid();
            result.AddScore = -1;
            result.DateTime = double.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

            RedisContext.GlobalContext.AddSortedSetEntity(result);
            
        }
    }


    [RedisTableAttribute(), Serializable]
    public class GameResult
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public string Uid { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int AddScore { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public Guid ActionId { get; set; }

        [RedisColumn(RedisColumnType.RedisScore)]
        public double DateTime { get; set; }
    }
}