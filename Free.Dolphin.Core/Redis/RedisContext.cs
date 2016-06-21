using Free.Dolphin.Common;
using Free.Dolphin.Common.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public class RedisContext
    {

        private RedisContext()
        {

        }

        static Dictionary<Type, ReflectionUtil> _keyCache = new Dictionary<Type, ReflectionUtil>();
        static Dictionary<Type, ReflectionUtil> _scoreCache = new Dictionary<Type, ReflectionUtil>();
        static Dictionary<Type, Func<object>> _objectCache = new Dictionary<Type, Func<object>>();
        public static RedisContext GlobalContext { get; private set; }

        protected static ConnectionMultiplexer RedisConnection { get; private set; }

        protected static IDatabase RedisDb { get; private set; }
        public static void InitRedisContext(string redisConnectionString, Assembly assembly)
        {
            InitEntity(assembly);

            RedisConnection = ConnectionMultiplexer.Connect(redisConnectionString);

            RedisDb = RedisConnection.GetDatabase();
            GlobalContext = new RedisContext();
        }

        private static void InitEntity(Assembly assembly)
        {
            foreach (var row in assembly.GetTypes())
            {
                RedisTableAttribute rt = row.GetCustomAttribute<RedisTableAttribute>();
                if (rt != null)
                {

                    if (!_objectCache.ContainsKey(row))
                    {
                        _objectCache.Add(row, ReflectionUtil.CreateInstanceDelegate(row));
                    }

                    if (!_keyCache.ContainsKey(row))
                    {
                        foreach (var propertie in row.GetProperties())
                        {
                            RedisColumnAttribute redisColumn = propertie.GetCustomAttribute<RedisColumnAttribute>();
                            if (redisColumn != null && redisColumn.ColumnType == RedisColumnType.RedisKey)
                            {
                                _keyCache.Add(row,
                                new ReflectionUtil(
                                     ReflectionUtil.CreatePropertyGetter(propertie),
                                     ReflectionUtil.CreatePropertySetter(propertie)
                                     ));
                            }
                            if (redisColumn != null && redisColumn.ColumnType == RedisColumnType.RedisScore)
                            {
                                _scoreCache.Add(row, new ReflectionUtil(
                                     ReflectionUtil.CreatePropertyGetter(propertie),
                                     ReflectionUtil.CreatePropertySetter(propertie)
                                     ));
                            }
                        }
                    }
                }
            }
        }


        //TODO: 修改序列化的方式
        public async void AddHashEntityAsync(object entity)
        {
            await Task.Run(() =>
            {
                Type t = entity.GetType();
                var key = _keyCache[t].GetValue(entity).ToString();

                RedisDb.HashSet(t.Name, new HashEntry[] {
                new HashEntry(key, SerializerUtil.BinarySerialize(entity))
            });
            });
        }
        public void AddHashEntity(object entity)
        {
            Type t = entity.GetType();
            var key = _keyCache[t].GetValue(entity).ToString();

            RedisDb.HashSet(t.Name, new HashEntry[] {
                new HashEntry(key, SerializerUtil.BinarySerialize(entity))
            });
        }

        public long IncrHashEntity(object entity)
        {
            Type t = entity.GetType();
            var key = _keyCache[t].GetValue(entity).ToString();
            return RedisDb.HashIncrement(t.Name, key);
        }

        public T FindHashEntityByKey<T>(string key)
        {
            var entity = RedisDb.HashGet(typeof(T).Name, key);
            if (!entity.IsNull)
            {
                return (T)SerializerUtil.BinaryDeserialize(RedisDb.HashGet(typeof(T).Name, key));
            }
            return default(T);
        }

        public IEnumerable<T> FindEntityAll<T>()
        {
            Task<HashEntry[]> task = RedisDb.HashGetAllAsync(typeof(T).Name);
            Task.WaitAll(task);
            foreach (var row in task.Result)
            {
                yield return (T)SerializerUtil.BinaryDeserialize(row.Value);
            }
        }

        public IEnumerable<T> FindSoredEntity<T>(int take) {
            Type type = typeof(T);
            foreach (var row in RedisDb.SortedSetRangeByRankWithScores(type.Name,0,take))
            {
                object o = _objectCache[type]();
                _scoreCache[type].SetValue(o, row.Score);
                _keyCache[type].SetValue(o, row.Element.ToString());
                yield return (T)o;
            }
        }


        public void AddSortedSetEntity(object entity)
        {
            Type t = entity.GetType();
            var element = _keyCache[t].GetValue(entity).ToString();
            var score = (int)_scoreCache[t].GetValue(entity);
            RedisDb.SortedSetAdd(t.Name,new SortedSetEntry[] {
                new SortedSetEntry(element,score)
            });
        }

        public void DeleteHashEntity(object entity)
        {
            Type t = entity.GetType();
            var key = _keyCache[t].GetValue(entity).ToString();
            RedisDb.HashDelete(t.Name, key);
        }

        public void DeleteSortedSetEntity(object entity)
        {
            Type t = entity.GetType();
            var key = _keyCache[t].GetValue(entity).ToString();
            RedisDb.SortedSetRemove(t.Name, key);
        }
    }
}
