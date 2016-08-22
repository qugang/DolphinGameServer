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
        /// 解散房间
        /// </summary>
        public Boolean Cancel { get; set; }

        /// <summary>
        /// 解散房间投票状态
        /// </summary>
        public Boolean CancelState { get; set; }
        /// <summary>
        /// 玩家分数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 吃的牌
        /// </summary>
        public List<int> ChiCards { get; set; }
        /// <summary>
        /// 杠的牌
        /// </summary>
        public List<int> GangCards { get; set; }

        /// <summary>
        /// 补张
        /// </summary>
        public List<int> BuZhangCards { get; set; }

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

        /// <summary>
        /// 用于描述手上的牌
        /// </summary>
        public int tNumber { get; set; }

        /// <summary>
        /// 用于描述手上加吃加碰的牌的张数，判断清一色时用
        /// </summary>
        public int ttNumber { get; set; }

        /// <summary>
        /// 索
        /// </summary>
        public List<int> sCards { get; set; }

        public int sNumber { get; set; }

        /// <summary>
        /// 用于描述手上加吃加碰的牌的张数，判断清一色时用
        /// </summary>
        public int tsNumber { get; set; }

        /// <summary>
        /// 万
        /// </summary>
        public List<int> wCards { get; set; }

        public int wNumber { get; set; }

        /// <summary>
        /// 用于描述手上加吃加碰的牌的张数，判断清一色时用
        /// </summary>
        public int twNumber { get; set; }
        

        public GameUser PlayerUser { get; set; }

        public MjGamePlayerBase(GameUser user)
        {
            this.PlayerUser = user;
            this.IsReady = true;
            this.Score = 1000;
        }

        public void InitCard(int[] card)
        {
            this.wCards = new List<int>();
            this.tCards = new List<int>();
            this.sCards = new List<int>();
            this.ChiCards = new List<int>();
            this.PengCards = new List<int>();
            this.GangCards = new List<int>();
            this.BuZhangCards = new List<int>();
            this.zhuoCards = new List<int>();
            foreach (var row in card)
            {
                int itemType = row.GetItemType();

                if (itemType == 0)
                {
                    this.wCards.AddCardItem(row);
                    this.wNumber++;
                    this.twNumber++;
                }
                else if (itemType == 1)
                {
                    this.tCards.AddCardItem(row);
                    this.tNumber++;
                    this.ttNumber++;
                }
                else
                {
                    this.sCards.AddCardItem(row);
                    this.sNumber++;
                    this.tsNumber++;
                }
                SortCards();
            }
        }

        protected void SortCards()
        {
            this.wCards.Sort((a, b) =>
            {
                return a.GetItemValue() - b.GetItemValue();
            });
            this.tCards.Sort((a, b) =>
            {
                return a.GetItemValue() - b.GetItemValue();
            });

            this.sCards.Sort((a, b) =>
            {
                return a.GetItemValue() - b.GetItemValue();
            });
        }

        public Boolean IsReady { get; set; }

        public void OutCard(int card)
        {
            int type = card.GetItemType();
            if (type == 0)
            {
                this.wCards.RemoveCardItem(card);
                this.wNumber--;
            }
            else if (type == 1)
            {
                this.tCards.RemoveCardItem(card);
                this.tNumber--;
            }
            else
            {
                this.sCards.RemoveCardItem(card);
                this.sNumber--;
            }
            SortCards();
        }

        public void DaCard(int card)
        {

            int type = card.GetItemType();
            if (type == 0)
            {
                this.wCards.RemoveCardItem(card);
                this.wNumber--;
                this.twNumber--;
            }
            else if (type == 1)
            {
                this.tCards.RemoveCardItem(card);
                this.tNumber--;
                this.ttNumber--;
            }
            else
            {
                this.sCards.RemoveCardItem(card);
                this.sNumber--;
                this.tsNumber--;
            }
            SortCards();
            this.zhuoCards.Add(card);
        }

        public void AddCard(int card)
        {

            int type = card.GetItemType();
            if (type == 0)
            {
                this.wCards.AddCardItem(card);
                this.wNumber++;
            }
            else if (type == 1)
            {
                this.tCards.AddCardItem(card);
                this.tNumber++;
            }
            else
            {
                this.sCards.AddCardItem(card);
                this.sNumber++;
            }
            SortCards();
        }


        private Boolean CheckChi(int card, List<int> array)
        {
            List<int> tempArray = array.ToList();

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
        /// 检测吃
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckChi(int card)
        {

            int type = card.GetItemType();
            if (type == 0)
            {
                return CheckChi(card, this.wCards);
            }
            else if (type == 1)
            {
                return CheckChi(card, this.tCards);
            }
            else
            {
                return CheckChi(card, this.sCards);
            }

        }


        /// <summary>
        /// 检测杠
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckGang(int card)
        {
            int type = card.GetItemType();
            if (type == 0)
            {
                return this.wCards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() == 3);
            }
            else if (type == 1)
            {
                return this.tCards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() == 3);
            }
            else
            {
                return this.sCards.Exists(p => p.GetItemValue() == card.GetItemValue() && card.GetItemNumber() == 3);
            }
        }

        /// <summary>
        /// 检测碰
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Boolean CheckPeng(int card)
        {
            int type = card.GetItemType();
            if (type == 0)
            {
                return this.wCards.Exists(p => p.GetItemValue() == card.GetItemValue() && p.GetItemNumber() >= 2);
            }
            else if (type == 1)
            {
                return this.tCards.Exists(p => p.GetItemValue() == card.GetItemValue() && p.GetItemNumber() >= 2);
            }
            else
            {
                return this.sCards.Exists(p => p.GetItemValue() == card.GetItemValue() && p.GetItemNumber() >= 2);
            }
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
        /// 检查胡,没有检查清一色
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public virtual Boolean CheckHu()
        {


            if (this.wNumber == 1 || this.tNumber == 1 || this.sNumber == 1)
            {
                return false;
            }

            if ((this.twNumber == 0 && this.ttNumber == 0) ||
                (this.tsNumber == 0 && this.twNumber == 0) ||
                (this.tsNumber == 0 && this.ttNumber == 0))
            {
                var result = CheckQingYiSe();
                return result;
            }

            if (this.CheckXiaoQiDui())
            {
                return true;
            }


            int wJiangPos = this.wCards.FindJiang(this.wNumber);
            int tJiangPos = this.tCards.FindJiang(this.tNumber);
            int sJiangPos = this.sCards.FindJiang(this.sNumber);

            if (wJiangPos == -1 &&
                tJiangPos == -1 &&
                sJiangPos == -1)
            {
                return false;
            }

            if (!CheckHuParam(this.wCards, wJiangPos))
            {
                return false;
            }
            if (!CheckHuParam(this.tCards, tJiangPos))
            {
                return false;
            }
            if (!CheckHuParam(this.sCards, sJiangPos))
            {
                return false;
            }
            return true;
        }

        public Boolean CheckQingYiSe()
        {

            List<int> tempArray = new List<int>();


            if (this.tsNumber == 0 && this.ttNumber == 0)
            {
                tempArray = this.wCards;
            }
            else if (this.tsNumber == 0 && this.twNumber == 0)
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

        public void Chi(int card, int card1, int card2)
        {
            this.OutCard(card1);
            this.OutCard(card2);
            this.ChiCards.AddRange(new int[] { card, card1, card2 });
        }

        public void Peng(int card)
        {
            this.OutCard(card);
            this.OutCard(card);
            this.PengCards.Add(card);
        }

        public void Gang(int card)
        {
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.GangCards.Add(card);
        }

        public void AnGang(int card)
        {
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.GangCards.Add(card);
        }

        public void BuZhang(int card)
        {
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.BuZhangCards.Add(card);
        }

        public void AnBuZhang(int card)
        {
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.OutCard(card);
            this.BuZhangCards.Add(card);
        }

        protected void PushCard(int card)
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

        protected void PopCard(int card)
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

                if (jiangNumber.GetItemNumber() > 2)
                {
                    array[jiangPos] = array[jiangPos].SubItemNumber(2);
                }
                else
                {
                    array.RemoveAt(jiangPos);
                }
                var result = this.Analyze(array);

                if (jiangNumber.GetItemNumber() > 2)
                {
                    array[jiangPos] = jiangNumber;
                }
                else
                {
                    array.Insert(jiangPos, jiangNumber);
                }

                return result;
            }
            else
            {
                return this.Analyze(array);
            }
        }



        private Boolean Analyze(List<int> array)
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
                result = this.Analyze(array);
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
                    result = this.Analyze(array);
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
                && this.sCards.All(p => p.GetItemNumber() == 2 || p.GetItemNumber() == 4)
                && this.PengCards.Count == 0 && this.GangCards.Count == 0 && this.ChiCards.Count == 0)
            {
                return true;
            }
            return false;
        }

        public string PrintCards()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("万");
            foreach (var row in this.wCards)
            {
                for (int i = 0; i < row.GetItemNumber(); i++)
                {

                    sb.Append(row.GetItemValue());
                }
            }

            sb.Append("筒");
            foreach (var row in this.tCards)
            {
                for (int i = 0; i < row.GetItemNumber(); i++)
                {

                    sb.Append(row.GetItemValue());
                }
            }

            sb.Append("索");
            foreach (var row in this.sCards)
            {
                for (int i = 0; i < row.GetItemNumber(); i++)
                {

                    sb.Append(row.GetItemValue());
                }
            }
            sb.Append("总张数: " + (this.wNumber + this.tNumber + this.sNumber));
            return sb.ToString();
        }

        private string GetCardTypeForString(int type)
        {
            if (type == 0)
            {
                return "万";
            }
            else if (type == 1)
            {
                return "筒";
            }
            else
            {
                return "索";
            }
        }



    }
}
