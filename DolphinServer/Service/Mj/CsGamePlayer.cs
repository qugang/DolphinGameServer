using DolphinServer.ProtoEntity;
using Free.Dolphin.Core;
using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Free.Dolphin.Core.Util;
using DolphinServer.Controller;
using DolphinServer.Entity;

namespace DolphinServer.Service.Mj
{

    public class CsGamePlayer : MjGamePlayerBase
    {
        public int FirstHuType { get; set; }

        public int HuType { get; set; }

        public int PaoHuType { get; set; }

        public int GetAllHuType()
        {
            return FirstHuType | HuType | PaoHuType;
        }

        public int SubScore { get; set; }

        public int AddScore { get; set; }

        public LinkedListNode<CsGamePlayer> DianPaoPlayer { get; set; }

        public CsGamePlayer(GameUser gameSession) : base(gameSession)
        {
            NeedGangDaPai = new List<int>();
        }

        /// <summary>
        /// 断线重连重新发送打牌命令
        /// </summary>
        public List<int> NeedGangDaPai { get; set; }

        /// <summary>
        /// 检查四喜
        /// </summary>
        /// <returns></returns>
        public Boolean CheckSiXi()
        {
            if (this.wCards.Exists(p => p.GetItemNumber() == 4))
            {
                return true;
            }
            if (this.tCards.Exists(p => p.GetItemNumber() == 4))
            {
                return true;
            }
            if (this.sCards.Exists(p => p.GetItemNumber() == 4))
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
            if (this.wCards.All(p => p.GetItemValue() != 2 && p.GetItemValue() != 5 && p.GetItemValue() != 8)
                && this.tCards.All(p => p.GetItemValue() != 2 && p.GetItemValue() != 5 && p.GetItemValue() != 8)
                && this.sCards.All(p => p.GetItemValue() != 2 && p.GetItemValue() != 5 && p.GetItemValue() != 8))
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
            this.wCards.ForEach(p =>
            {
                if (p.GetItemNumber() >= 3)
                {
                    count++;
                }
            });
            this.tCards.ForEach(p =>
            {
                if (p.GetItemNumber() >= 3)
                {
                    count++;
                }
            });
            this.sCards.ForEach(p =>
            {
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
        public Boolean CheckJiangJiangHu()
        {
            if (this.wCards.All(p => p.GetItemValue() == 2 || p.GetItemValue() == 5 || p.GetItemValue() == 8) &&
               this.tCards.All(p => p.GetItemValue() == 2 || p.GetItemValue() == 5 || p.GetItemValue() == 8) &&
               this.sCards.All(p => p.GetItemValue() == 2 || p.GetItemValue() == 5 || p.GetItemValue() == 8))
            {
                return true;
            }
            return false;
        }

        public Boolean CheckQuanQiuRen()
        {
            if (this.tCards.Count == 1 && this.sCards.Count == 0 && this.wCards.Count == 0)
                return true;
            if (this.sCards.Count == 1 && this.tCards.Count == 0 && this.wCards.Count == 0)
                return true;
            if (this.wCards.Count == 1 && this.tCards.Count == 0 && this.sCards.Count == 0)
                return true;
            return false;
        }

        public Boolean CheckHu(int card)
        {
            card = card & 0x18F | 0x10;
            PushCard(card);
            this.SortCards();
            if (this.CheckQuanQiuRen())
            {
                PopCard(card);
                return true;
            }
            if (this.CheckJiangJiangHu())
            {

                PopCard(card);
                return true;
            }
            if (base.CheckHu())
            {

                PopCard(card);
                return true;
            }
            this.PopCard(card);
            return false;
        }

        public void ReLoad()
        {
            this.IsReady = false;
            this.HuType = 0;
            this.PaoHuType = 0;
            this.AddScore = 0;
            this.SubScore = 0;
            this.DianPaoPlayer = null;
            this.ResetEvent.Reset();
        }
    }
}