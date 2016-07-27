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
                3|0x10,4|0x10,5|0x10,
                //初始化筒,0x10表示1张,0x80表示筒
                7|0x10|0x80,7|0x10|0x80,7|0x10|0x80,
                //初始化条,0x10表示1张,0x100表示条
                1|0x10|0x100,2|0x10|0x100,3|0x10|0x100,4|0x10|0x100,4|0x10|0x100,7|0x10|0x100,8|0x10|0x100

            });
            Boolean result = player.CheckHu(6 | 0x10 | 0x100);
            Assert.AreEqual(result, true);
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

            //152207手上的牌万13457筒15688索467总张数: 13
            CsGamePlayer player = new CsGamePlayer(null);
            player.InitCard(new int[] {
            0|0x10, 2|0x10,3|0x10,4|0x10,6|0x10
            });
            
            player.CheckChi(0x25);
            

        }

        [TestMethod()]
        public void CheckPengTest()
        {
            CsGamePlayer player = new CsGamePlayer(null);
            player.InitCard(new int[] {
            0|0x10,0|0x10,0|0x10,1|0x10,2|0x10,3|0x10,4|0x10,5|0x10,6|0x10,7|0x10,7|0x10,7|0x10,8|0x10
            });

            Boolean result = player.CheckPeng(0 | 0x10);

            Assert.AreEqual(result, true);
        }
    }
}