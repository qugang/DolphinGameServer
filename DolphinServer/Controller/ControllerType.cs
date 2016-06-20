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
        Controller1003 = 1003,
        /// <summary>
        /// 解散房间
        /// </summary>
        Controller1004 = 1004,
        /// <summary>
        /// 游戏准备
        /// </summary>
        Controller1005 = 1005,
        /// <summary>
        /// 发牌
        /// </summary>
        Controller1006 = 1006,
        /// <summary>
        /// 摸牌
        /// </summary>
        Controller1007 = 1007,

        /// <summary>
        /// 胡牌
        /// </summary>
        Controller1008 = 1008,

        /// <summary>
        /// 查询日排行榜
        /// </summary>
        Controller1009 = 1009,

        /// <summary>
        /// 查询周排行榜
        /// </summary>
        Controller1010 = 1010,


        /// <summary>
        /// 查询月排行榜
        /// </summary>
        Controller1011 = 1011,
    }
}
