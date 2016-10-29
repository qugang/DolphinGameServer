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
    public class CsGamePlayerTests
    {
        [TestMethod()]
        public void CheckHuTest()
        {
            CsGamePlayer csGamePlayer = new CsGamePlayer(new Entity.GameUser());
            csGamePlayer.InitCard(new int[] { 0x00 });
            Boolean result = csGamePlayer.CheckHu(0x01);

            Assert.AreEqual(result, true);
        }
    }
}