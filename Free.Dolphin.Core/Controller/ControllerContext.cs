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
            var construtor = typeof(T).GetConstructor(null);
            NewExpression newExp = Expression.New(construtor, null);
            Expression<Func<object>> lambdaExp =
                Expression.Lambda<Func<object>>(newExp, null);
            Func<object> func = lambdaExp.Compile();
            return func;
        }

        internal ControllerContext(Dictionary<string, string> keyValue)
        {
            this.HttpQueryString = keyValue;
            this.ProtocolNumber = int.Parse(HttpQueryString["ProtocolId"]);
            this.Sid = Guid.Parse(HttpQueryString["Sid"].ToString());
            this.Uid = HttpQueryString["Uid"].ToString();
            this.Sign = HttpQueryString["Sign"].ToString();
            this.Pwd = HttpQueryString["Pwd"].ToString();
            
            if (!String.IsNullOrEmpty(this.Uid))
            {
                IGameUser user = (IGameUser)_gameUserFunc();
                user.Login(this.Uid);
            }


        }
        public int ProtocolNumber { get; set; }

        public Guid Sid { get; set; }

        public string Uid { get; set; }

        public string Pwd { get; set; }

        public string Sign { get; set; }

        public GameSession Session { get; set; }

        public Dictionary<string, string> HttpQueryString { get; set; }
    }
}
