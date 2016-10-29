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
    public class CsMjGameRoomTests
    {
        /// <summary>
        /// 测试算分规则小胡
        /// </summary>
        [TestMethod()]
        public void CalculationResultXiaohuTest()
        {
            LinkedListNode<CsGamePlayer> linker = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "1" }));
            linker.Value.HuType = 1;
            LinkedListNode<CsGamePlayer> linker1 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "2" }));
            linker1.Value.HuType = 1;
            LinkedListNode<CsGamePlayer> linker2 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "3" }));
            LinkedListNode<CsGamePlayer> linker3 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "4" }));

            CsMjGameRoom room = new CsMjGameRoom("1");
            room.Players = new LinkedList<CsGamePlayer>();
            room.Players.AddLast(linker);
            room.Players.AddLast(linker1);
            room.Players.AddLast(linker2);
            room.Players.AddLast(linker3);

            CalculationScore.Calculation(room.Players, "1", "1");

            Assert.AreEqual(linker.Value.AddScore, 18);
            Assert.AreEqual(linker.Value.SubScore, 6);

            Assert.AreEqual(linker1.Value.AddScore, 8);
            Assert.AreEqual(linker1.Value.SubScore, 6);

            Assert.AreEqual(linker2.Value.AddScore, 0);
            Assert.AreEqual(linker2.Value.SubScore, 7);

            Assert.AreEqual(linker3.Value.AddScore, 0);
            Assert.AreEqual(linker3.Value.SubScore, 7);
        }



        /// <summary>
        /// 测试算分规则大胡
        /// </summary>
        [TestMethod()]
        public void CalculationResultDahuTest()
        {
            LinkedListNode<CsGamePlayer> linker = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "1" }));
            linker.Value.HuType = 4 | 128;
            LinkedListNode<CsGamePlayer> linker1 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "2" }));
            LinkedListNode<CsGamePlayer> linker2 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "3" }));
            LinkedListNode<CsGamePlayer> linker3 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "4" }));

            CsMjGameRoom room = new CsMjGameRoom("1");
            room.Players = new LinkedList<CsGamePlayer>();
            room.Players.AddLast(linker);
            room.Players.AddLast(linker1);
            room.Players.AddLast(linker2);
            room.Players.AddLast(linker3);

            CalculationScore.Calculation(room.Players, "1", "1");

            Assert.AreEqual(linker.Value.AddScore, 81);
            Assert.AreEqual(linker.Value.SubScore, 0);

            Assert.AreEqual(linker1.Value.AddScore, 0);
            Assert.AreEqual(linker1.Value.SubScore, 27);

            Assert.AreEqual(linker2.Value.AddScore, 0);
            Assert.AreEqual(linker2.Value.SubScore, 27);

            Assert.AreEqual(linker3.Value.AddScore, 0);
            Assert.AreEqual(linker3.Value.SubScore, 27);
        }




        /// <summary>
        /// 测试算分规则点炮
        /// </summary>
        [TestMethod()]
        public void CalculationResultDianPaoTest()
        {
            LinkedListNode<CsGamePlayer> linker = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "1" }));
            linker.Value.PaoHuType = 4 | 128;
            LinkedListNode<CsGamePlayer> linker1 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "2" }));
            LinkedListNode<CsGamePlayer> linker2 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "3" }));
            LinkedListNode<CsGamePlayer> linker3 = new LinkedListNode<CsGamePlayer>(new CsGamePlayer(new Entity.GameUser() { Uid = "4" }));
            linker.Value.DianPaoPlayer = linker2;
            CsMjGameRoom room = new CsMjGameRoom("1");
            room.Players = new LinkedList<CsGamePlayer>();
            room.Players.AddLast(linker);
            room.Players.AddLast(linker1);
            room.Players.AddLast(linker2);
            room.Players.AddLast(linker3);

            CalculationScore.Calculation(room.Players, "1", "1");

            Assert.AreEqual(linker.Value.AddScore, 27);
            Assert.AreEqual(linker.Value.SubScore, 0);

            Assert.AreEqual(linker2.Value.SubScore, 27);
            
        }





    }
}