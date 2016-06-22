using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{


    /// <summary>
    /// 使用结构体描述牌
    /// 第八第九个字节用于描述类型 00 万 01 桶 10 索 11 字
    /// 第五第六第7个字节用于描述张数 01 一张 10 二张 11 三张 100 四张
    /// </summary>

    public static class MjUtil
    {
        public static int GetItemType(this int value)
        {
            return (value >> 7) & 0x03;
        }

        public static int GetItemNumber(this int value)
        {
            return value >> 4 & 0x07;
        }

        public static int AddItemNumber(this int value, int number)
        {
            return value + 16 * number;
        }

        public static int SubItemNumber(this int value ,int number)
        {
            return value - 16 * number;
        }


        public static int ClearItemNumber(this int value)
        {
            return value & 0x38F;
        }

        public static int GetItemValue(this int value)
        {
            return (value & 0xF) + 1;
        }

        public static void RemoveCardItem(this List<int> array,int card)
        {
            int index = array.FindIndex(p => p.GetItemValue() == card.GetItemValue());

            if (index == -1)
            {
                throw new Exception("所打的牌元素未在玩家手中 card :" + card.GetItemValue());
            }

            if (array[index].GetItemNumber() > 1)
            {
                array[index] = array[index].SubItemNumber(1);
            }
            else
            {
                array.RemoveAt(index);
            }
        }

        public static void AddCardItem(this List<int> array, int card)
        {
            int index = array.FindIndex(p => p.GetItemValue() == card.GetItemValue());

            if (index == -1)
            {
                array.Add(card);
            }
            else
            {
                array[index].AddItemNumber(1);
            }
        }
        
    }
}
