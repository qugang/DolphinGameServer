using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Controller
{
    public enum ControllerType
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        Controller1001 = 1001,
        /// <summary>
        /// 创建房间
        /// </summary>
        Controller1002 = 1002,
        /// <summary>
        /// 加入房间
        /// </summary>
        Controller1003 = 1003
    }
}
