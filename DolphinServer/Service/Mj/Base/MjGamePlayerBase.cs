using DolphinServer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    public abstract class MjGamePlayerBase
    {

        /// <summary>
        /// 手上的牌
        /// </summary>
        public List<int> Cards { get; set; }

        /// <summary>
        /// 吃的牌
        /// </summary>
        public List<int> ChiCards { get; set; }
        /// <summary>
        /// 杠的牌
        /// </summary>
        public List<int> GangCards { get; set; }

        /// <summary>
        /// 碰的牌
        /// </summary>
        public List<int> PengCards { get; set; }

        /// <summary>
        /// 桌上的牌
        /// </summary>
        public List<int> zhuoCards { get; set; }


        public GameUser PlayerUser { get; set; }
        public MjGamePlayerBase(GameUser user)
        {
            this.PlayerUser = user;
            this.IsReady = true;
        }

        public Boolean IsReady { get; set; }

        public void OutCard(int card)
        {
            this.Cards.RemoveCardItem(card);
        }

        /// <summary>
        /// 检测吃
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        protected List<dynamic> CheckChi(int card)
        {
            List<int> tempArray = this.Cards.ToList();
            List<dynamic> chiArray = new List<dynamic>();

            int index = tempArray.FindIndex(p => p.GetItemNumber() == card);

            if (index == -1)
            {
                tempArray.Add(card);
                tempArray.Sort((a, b) =>
                {
                    return a.GetItemValue() - b.GetItemValue();
                });
                index = tempArray.FindIndex(p => p.GetItemNumber() == card);
            }

            if (index + 2 < tempArray.Count)
            {
                if (this.IsShun(tempArray[index], tempArray[index + 1], tempArray[index + 2]))
                {
                    chiArray.Add(new { oneItem = tempArray[index + 2],twoItem = tempArray[index + 1]});
                }
            }
            if (index - 2 >= 0)
            {
                if (this.IsShun(tempArray[index - 2], tempArray[index - 1], tempArray[index]))
                {
                    chiArray.Add( new { oneItem = tempArray[index - 1], twoItem = tempArray[index - 2]});
                }
            }
            if (index - 1 >= 0 && index + 1 < tempArray.Count)
            {
                if (this.IsShun(tempArray[index - 1], tempArray[index], tempArray[index + 1]))
                {
                    chiArray.Add(new { oneItem = tempArray[index + 1], twoItem = tempArray[index - 1]});
                }
            }
            return chiArray;
        }


        /// <summary>
        /// 检测杠
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        protected int CheckGang(int card)
        {
            if (this.Cards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() == 3))
            {
                return card;
            }
            return -1;
        }

        /// <summary>
        /// 检测碰
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        protected int CheckPeng(int card)
        {
            if (this.Cards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() >= 2))
            {
                return card;
            }
            return -1;
        }

        /// <summary>
        /// 检测是否是顺
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        protected Boolean IsShun(int a, int b, int c)
        {
            if (c.GetItemValue() - b.GetItemValue() == 1)
            {
                if (b.GetItemValue() - a.GetItemValue() == 1)
                {
                    return true;
                }
            }
            return false;
        }



    }
}
