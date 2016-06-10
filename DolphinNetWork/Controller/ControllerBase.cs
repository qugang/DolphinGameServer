using System;
using System.Collections.Generic;
using System.Linq;
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

        public abstract void ProcessAction();

    }
}
