using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public abstract class ControllerBase
    {
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
    }
}
