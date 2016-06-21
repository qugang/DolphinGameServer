using Free.Dolphin.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public abstract class ControllerBase
    {
        private static Func<object> _gameUserFunc = null;
        public static void InitGameUserType<T>()
        {
            _gameUserFunc = ReflectionUtil.CreateInstanceDelegate(typeof(T));
        }
        

        protected ControllerContext Context { get; set; }


        public ControllerBase(ControllerContext context)
        {
            this.Context = context;
        }

        public abstract byte[] ProcessAction();

        public Boolean IsLogin()
        {
            return Context.Session.User == null ? false : true;
        }

        public Boolean IsAuth()
        {
            return this.GetType().GetCustomAttribute<ControllerAuthAttribute>() == null ? false : true;
        }

        public Boolean Login()
        {
            string uid = null;
            string pwd = null;
            if (Context.HttpQueryString.ContainsKey("Uid"))
            {
                uid = Context.HttpQueryString["Uid"];
            }
            if (Context.HttpQueryString.ContainsKey("Pwd"))
            {
                pwd = Context.HttpQueryString["Pwd"];
            }
            var user = (_gameUserFunc() as IGameUser).Login(uid, pwd);
            if (user == null)
            {
                return false;
            }
            else
            {
                Context.Session.User = user;
                GameSessionManager.AddSessionWithUser(Context.Session, user.Uid);
                return true;
            }
        }
    }
}
