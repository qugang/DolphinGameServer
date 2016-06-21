using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core.Cache
{
    //TODO: 是否需要内存实体，经简单测试redis 每秒插入与读取能力在2万左右，按现在网络运行环境每秒并发最多，2，3百，以某银行系统为列最高并发也才1000左右，个人觉得瓶颈不在redis，现阶段已满足读取需求
    public class MemoryCache
    {

    }
}
