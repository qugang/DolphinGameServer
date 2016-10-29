using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Free.Dolphin.Core.Util;

namespace DolphinServer.Service.Mj
{
    public static class CalculationScore
    {

        private static Boolean IsZhongNiao(this CsGamePlayer player, string niaoUid)
        {
            if (player.PlayerUser.Uid == niaoUid)
            {
                return true;
            }
            return false;
        }

        private static Boolean IsZhuang(this CsGamePlayer player, string zhuangUid)
        {
            if (player.PlayerUser.Uid == zhuangUid)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 计算得分
        /// </summary>
        /// <param name="player"></param>
        /// <param name="niaoUid1"></param>
        /// <param name="niaoUid2"></param>
        /// <param name="zhuangUid"></param>
        public static void Calculation(LinkedList<CsGamePlayer> player, string niaoUid1, string niaoUid2)
        {
            string zhuangUid = player.First.Value.PlayerUser.Uid;
            foreach (var row in player.ToList())
            {
                if ((row.HuType | row.FirstHuType) == 0)
                {
                    continue;
                }

                int score = row.IsZhuang(zhuangUid) ? Calculation(row.GetAllHuType()) +1 : Calculation(row.GetAllHuType());
                
                int addScore = 0;

                foreach (var xRow in player.ToList())
                {
                    if (xRow.PlayerUser.Uid != row.PlayerUser.Uid)
                    {
                        int tempScore = xRow.IsZhuang(zhuangUid) ? score + 1 : score;
                        int niaoScore1 = row.IsZhongNiao(niaoUid1) || xRow.IsZhongNiao(niaoUid1) ? tempScore : 0;
                        int niaoScore2 = row.IsZhongNiao(niaoUid2) || xRow.IsZhongNiao(niaoUid2) ? tempScore : 0;
                        xRow.SubScore += tempScore + niaoScore1 + niaoScore2;
                        addScore += tempScore + niaoScore1 + niaoScore2;
                    }
                }
                row.AddScore += addScore;
            }

            foreach (var row in player.ToList())
            {
                if (row.PaoHuType == 0)
                {
                    continue;
                }

                int score = row.IsZhuang(zhuangUid) ? Calculation(row.GetAllHuType()) + 1 : Calculation(row.GetAllHuType());

                score = row.DianPaoPlayer.Value.IsZhuang(zhuangUid) ? score + 1 : score;

                int niaoScore1 = row.IsZhongNiao(niaoUid1) || row.DianPaoPlayer.Value.IsZhongNiao(niaoUid1) ? score : 0;
                int niaoScore2 = row.IsZhongNiao(niaoUid2) || row.DianPaoPlayer.Value.IsZhongNiao(niaoUid2) ? score : 0;

                row.AddScore = score + niaoScore1 + niaoScore2;
                row.DianPaoPlayer.Value.SubScore = score + niaoScore1 + niaoScore2;
            }

            foreach (var row in player.ToList())
            {
                row.Score += row.AddScore - row.SubScore;
            }


        }



        //1 小胡抓炮
        //10 小胡自摸
        //100 四喜
        //1000 板板胡
        //10000 缺一色
        //100000 六六顺
        //1000000 碰碰胡
        //10000000 清一色
        //100000000 海底捞月
        //1000000000 海底炮
        //10000000000 七小对
        //100000000000 豪华七小对
        //1000000000000 杠上开花
        //10000000000000 抢杠胡
        //100000000000000 杠上炮
        //1000000000000000 全求人
        //10000000000000000 将将胡
        //100000000000000000 杠上炮
        //1000000000000000000 杠翻倍
        public static int Calculation(int huType)
        {
            int score = 0;
            if ((huType & 1) == 1)
            {
                score += 1;
            }

            int i = 2;
            while (i <= 32)
            {
                //小胡
                if ((huType & i) == i)
                {
                    score += 2;
                }
                i = i * 2;
            }

            i = 64;

            while (i <= 0x40000)
            {
                if ((huType & i) == i)
                {
                    score += 6;
                }
                i = i * 2;
            }
            return score;
        }
    }
}
