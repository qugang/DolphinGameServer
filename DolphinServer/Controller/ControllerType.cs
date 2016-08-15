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
        /// 打牌
        /// </summary>
        Controller1007 = 1007,
        /// <summary>
        /// 未用
        /// </summary>
        Controller1008 = 1008,
        /// <summary>
        /// 过牌
        /// </summary>
        Controller1009 = 1009,
        /// <summary>
        /// 吃牌
        /// </summary>
        Controller1010 = 1010,
        /// <summary>
        /// 碰牌
        /// </summary>
        Controller1011 = 1011,
        /// <summary>
        /// 杠牌
        /// </summary>
        Controller1012 = 1012,
        /// <summary>
        /// 自摸
        /// </summary>
        Controller1013 = 1013,
        /// <summary>
        /// 捉炮
        /// </summary>
        Controller1014 = 1014,
        /// <summary>
        /// 开局胡
        /// </summary>
        Controller1015 = 1015,
        /// <summary>
        /// 海底摸牌
        /// </summary>
        Controller1016 = 1016,

        /// <summary>
        /// 海底过
        /// </summary>
        Controller1017 = 1017,

        /// <summary>
        /// 聊天接收
        /// </summary>
        Controller1100 = 1100,

        /// <summary>
        /// 杠打牌
        /// </summary>
        Controller1018 = 1018,

        /// <summary>
        /// 抓炮结果
        /// </summary>
        Controller1019 = 1019,

        /// <summary>
        /// 
        /// </summary>
        Controller1020 = 1020,

    }
}
