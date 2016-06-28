using DolphinServer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    public abstract class MjGamePlayerBase
    {

        public ManualResetEvent ResetEvent = new ManualResetEvent(false);


        /// <summary>
        /// 玩家分数
        /// </summary>
        public int Score { get; set; }

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

        /// <summary>
        /// 筒
        /// </summary>
        public List<int> tCards { get; set; }

        public int tNumber { get; set; }

        /// <summary>
        /// 索
        /// </summary>
        public List<int> sCards { get; set; }

        public int sNumber { get; set; }

        /// <summary>
        /// 万
        /// </summary>
        public List<int> wCards { get; set; }

        public int wNumber { get; set; }

        public GameUser PlayerUser { get; set; }

        public MjGamePlayerBase(GameUser user)
        {
            this.PlayerUser = user;
            this.IsReady = true;
        }

        public void InitCard(int[] card)
        {
            this.Cards = new List<int>();
            this.wCards = new List<int>();
            this.tCards = new List<int>();
            this.sCards = new List<int>();
            foreach (var row in card)
            {
                int itemType = row.GetItemType();

                if (itemType == 0)
                {
                    this.wCards.AddCardItem(row);
                    this.wNumber++;
                }
                if (itemType == 1)
                {
                    this.tCards.AddCardItem(row);
                    this.tNumber++;
                }
                if (itemType == 2)
                {
                    this.sCards.AddCardItem(row);
                    this.sNumber++;
                }

                this.Cards.AddCardItem(row);

            }
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
        public Boolean CheckChi(int card)
        {
            List<int> tempArray = this.Cards.ToList();

            int index = tempArray.FindIndex(p => p.GetItemValue() == card.GetItemValue());

            if (index == -1)
            {
                tempArray.Add(card);
                tempArray.Sort((a, b) =>
                {
                    return a.GetItemValue() - b.GetItemValue();
                });
                index = tempArray.FindIndex(p => p.GetItemValue() == card.GetItemValue());
            }

            if (index + 2 < tempArray.Count)
            {
                if (this.IsShun(tempArray[index], tempArray[index + 1], tempArray[index + 2]))
                {
                    return true;
                }
            }
            if (index - 2 >= 0)
            {
                if (this.IsShun(tempArray[index - 2], tempArray[index - 1], tempArray[index]))
                {
                    return true;
                }
            }
            if (index - 1 >= 0 && index + 1 < tempArray.Count)
            {
                if (this.IsShun(tempArray[index - 1], tempArray[index], tempArray[index + 1]))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 检测杠
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckGang(int card)
        {
            if (this.Cards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() == 3))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测碰
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckPeng(int card)
        {
            if (this.Cards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() >= 2))
            {
                return true;
            }
            return false;
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


        /// <summary>
        /// 检查胡,没有检查清一色，考虑效率问题，具体需要测试
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckHu(int card)
        {

            PushCard(card);

            if (this.wNumber == 1 || this.tNumber == 1 || this.sNumber == 1)
            {
                PopCard(card);
                return false;
            }

            if (this.wNumber == 14 || this.tNumber == 14 || this.sNumber == 14)
            {
                var result = CheckQingYiSe();
                PopCard(card);
                return result;
            }

            if (this.CheckXiaoQiDui())
            {
                PopCard(card);
                return true;
            }
            
            int wJiangPos = this.wCards.FindJiang(this.wNumber);
            int tJiangPos = this.tCards.FindJiang(this.tNumber);
            int sJiangPos = this.sCards.FindJiang(this.sNumber);

            if (wJiangPos == -1 &&
                tJiangPos == -1 &&
                sJiangPos == -1)
            {
                PopCard(card);
                return false;
            }

            if (!CheckHuParam(this.wCards, wJiangPos))
            {
                PopCard(card);
                return false;
            }
            if (!CheckHuParam(this.tCards, wJiangPos))
            {
                PopCard(card);
                return false;
            }
            if (!CheckHuParam(this.sCards, sJiangPos))
            {
                PopCard(card);
                return false;
            }

            PopCard(card);

            return true;
        }

        public Boolean CheckQingYiSe() {

            List<int> tempArray = new List<int>();
            if (this.wNumber == 14)
            {
                tempArray = this.wCards;
            }
            else if (this.tNumber == 14)
            {
                tempArray = this.tCards;
            }
            else
            {
                tempArray = this.sCards;
            }

            for (int i = 0; i < tempArray.Count; i++)
            {
                if (tempArray[i].GetItemNumber() >= 2)
                {
                    var result = CheckHuParam(tempArray, i);

                    if (result)
                    {
                        return result;
                    }
                }
            }
            return false;
        }

        private void PushCard(int card)
        {

            var tempType = card.GetItemType();

            if (tempType == 0)
            {
                this.wCards.AddCardItem(card);
                this.wNumber++;
            }
            else if (tempType == 1)
            {
                this.tCards.AddCardItem(card);
                this.tNumber++;
            }
            else
            {
                this.sCards.AddCardItem(card);
                this.sNumber++;
            }
        }

        private void PopCard(int card)
        {
            var tempType = card.GetItemType();

            if (tempType == 0)
            {
                this.wCards.RemoveCardItem(card);
                this.wNumber--;
            }
            else if (tempType == 1)
            {
                this.tCards.RemoveCardItem(card);
                this.tNumber--;
            }
            else
            {
                this.sCards.RemoveCardItem(card);
                this.sNumber--;
            }
        }


         private Boolean CheckHuParam(List<int> array, int jiangPos)
        {
            if (jiangPos != -1)
            {
                var jiangNumber = array[jiangPos];
                array[jiangPos] = array[jiangPos].SubItemNumber(2);
                var result = this.analyze(array);
                array[jiangPos] = jiangNumber;
                return result;
            }
            else
            {
                return this.analyze(this.wCards);
            }
        }
        


        private Boolean analyze(List<int> array)
        {
            var result = false;
            var pos = array.FindIndex(p =>
            {
                if (p.GetItemNumber() > 0)
                {
                    return true;
                }
                return false;
            });

            if (pos == -1)
            {
                return true;
            }


            if (array[pos].GetItemNumber() >= 3)
            {
                array[pos] = array[pos].SubItemNumber(3);
                result = this.analyze(array);
                array[pos] = array[pos].AddItemNumber(3);
                return result;
            }
            if (pos <= array.Count - 3 &&
                array[pos].GetItemNumber() >= 1 &&
                array[pos].GetItemNumber() >= 1 &&
                array[pos].GetItemNumber() >= 1)
            {
                if (this.IsShun(array[pos], array[pos + 1], array[pos + 2]))
                {
                    array[pos] = array[pos].SubItemNumber(1);
                    array[pos + 1] = array[pos + 1].SubItemNumber(1);
                    array[pos + 2] = array[pos + 2].SubItemNumber(1);
                    result = this.analyze(array);
                    array[pos] = array[pos].AddItemNumber(1);
                    array[pos + 1] = array[pos + 1].AddItemNumber(1);
                    array[pos + 2] = array[pos + 2].AddItemNumber(1);
                    return result;
                }
            }
            return false;
        }


        public Boolean CheckXiaoQiDui()
        {
            if (this.wCards.All(p => p.GetItemNumber() == 2 || p.GetItemNumber() == 4)
                && this.tCards.All(p => p.GetItemNumber() == 2 || p.GetItemNumber() == 4)
                && this.sCards.All(p => p.GetItemNumber() == 2 || p.GetItemNumber() == 4))
            {
                return true;
            }
            return false;
        }
        


    }
}
