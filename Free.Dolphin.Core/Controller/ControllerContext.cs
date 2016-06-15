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
        private static Func<object> _gameUserFunc = null;
        public static void InitGameUserType<T>()
        {
            _gameUserFunc = CreateInstanceDelegate<T>();
        }

        private static Func<object> CreateInstanceDelegate<T>()
        {
            var construtor = typeof(T).GetConstructor(new Type[] { });
            NewExpression newExp = Expression.New(construtor, null);
            Expression<Func<object>> lambdaExp =
                Expression.Lambda<Func<object>>(newExp, null);
            Func<object> func = lambdaExp.Compile();
            return func;
        }

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
