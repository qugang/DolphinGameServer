using DolphinServer.Entity;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{

    public class CsGamePlayer : MjGamePlayerBase
    { 
        public CsGamePlayer(GameUser gameSession) : base(gameSession)
        {

        }



        /// <summary>
        /// 检查四喜
        /// </summary>
        /// <returns></returns>
        public Boolean CheckSiXi()
        {
            if (this.Cards.Exists(p => p.GetItemNumber() == 4))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查板板胡
        /// </summary>
        /// <returns></returns>
        public Boolean CheckBanBanHu()
        {
            if (this.Cards.All(p => p.GetItemValue() != 2 && p.GetItemValue() != 5 && p.GetItemValue() != 8))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 检查六六顺
        /// </summary>
        /// <returns></returns>
        public Boolean CheckLiuLiuShun()
        {
            int count = 0;
            this.Cards.ForEach(p => {
                if (p.GetItemNumber() >= 3)
                {
                    count++;
                }
            });
            if (count >= 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查缺一色
        /// </summary>
        /// <returns></returns>
        public Boolean CheckQueYiSe()
        {
            if (this.wNumber == 0 || this.tNumber == 0 || this.sNumber == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 此胡是为了检测开局胡
        /// </summary>
        public Boolean CheckKaiJuHu()
        {
            if (this.CheckBanBanHu() ||
                this.CheckLiuLiuShun() ||
                this.CheckQueYiSe() ||
                this.CheckSiXi())
            {
                return true;
            }
            return false;
        }

    }
}
