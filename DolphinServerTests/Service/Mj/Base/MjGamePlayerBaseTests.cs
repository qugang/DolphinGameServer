using Microsoft.VisualStudio.TestTools.UnitTesting;
using DolphinServer.Service.Mj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj.Tests
{
    [TestClass()]
    public class MjGamePlayerBaseTests
    {
        /// <summary>
        /// 测试小胡
        /// </summary>
        [TestMethod()]
        public void CheckHuTest()
        {
            CsGamePlayer player = new CsGamePlayer(null);
            player.InitCard(new int[] {
                   //初始化万,0x10 表示1张
                0|0x10,0|0x10,
                //初始化筒,0x10表示1张,0x80表示筒
                3|0x10|0x80,4|0x10|0x80,5|0x10|0x80,6|0x10|0x80,7|0x10|0x80,8|0x10|0x80,
                //初始化条,0x10表示1张,0x100表示条
                1|0x10|0x100,2|0x10|0x100,3|0x10|0x100,7|0x10|0x100,7|0x10|0x100,

            });

            for (int i = 0; i < 10000; i++)
            {
                Boolean result = player.CheckHu(0 | 0x10);
            }

        }


        /// <summary>
        /// 测试清一色
        /// </summary>
        [TestMethod()]
        public void CheckQingYiSeTest()
        {
            CsGamePlayer player = new CsGamePlayer(null);
            player.InitCard(new int[] {
            0|0x10,0|0x10,0|0x10,1|0x10,2|0x10,3|0x10,4|0x10,5|0x10,6|0x10,7|0x10,7|0x10,7|0x10,8|0x10
            });

            Boolean result = player.CheckHu(0 | 0x10);

            Assert.AreEqual(result, true);

        }

        [TestMethod()]
        public void CheckChiTest()
        {
            CsGamePlayer player = new CsGamePlayer(null);
            player.InitCard(new int[] {
            0|0x10,0|0x10,0|0x10,1|0x10,2|0x10,3|0x10,4|0x10,5|0x10,6|0x10,7|0x10,7|0x10,7|0x10,8|0x10
            });

            Boolean result = player.CheckHu(1 | 0x10);

            Assert.AreEqual(result, true);
        }
    }
}