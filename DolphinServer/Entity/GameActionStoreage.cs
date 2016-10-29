using Free.Dolphin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj.ActionStorage
{
    [RedisTableAttribute(), Serializable]
    public class GameActionStoreage
    {
        [RedisColumn(RedisColumnType.RedisKey)]
        public Guid ActionId { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public int AddScore { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public List<CmdEntity> CmdList { get; set; }

        [RedisColumn(RedisColumnType.RedisColumn)]
        public List<CmdPlayer> LPlayer { get; set; }

        public GameActionStoreage()
        {
        }

        public GameActionStoreage(List<CmdPlayer> player)
        {
            this.LPlayer = player;
            this.CmdList = new List<CmdEntity>();
        }

        public void PushMo(string uid, int card)
        {
            CmdList.Add(new CmdEntity()
            {
                AType = ActionType.Mo,
                Card = card,
                Uid = uid,
            });
        }

        public void PushDa(string uid, int card)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.Da,
                Card = card
            });
        }

        public void PushChi(string uid, int card1, int card2, int card3)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.Chi,
                Card = card1,
                Card1 = card2,
                Card2 = card3
            });
        }

        public void PushGang(string uid, int card1, int card2, int card3)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.Gang,
                Card = card1,
                Card1 = card2,
                Card2 = card3
            });

            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.GangDa,
                Card = card2
            });

            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.GangDa,
                Card = card3
            });
        }

        public void PushFristHu(string uid, int huType)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.FristHu,
                HuType = huType
            });
        }

        public void PushHu(string uid, int huType)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.Hu,
                HuType = huType
            });
        }

        public void PushZhuoPao(string uid, int card, int huType)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.ZhuoPao,
                Card = card
            });
        }

        public void PushPeng(string uid, int card)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.Peng,
                Card = card
            });
        }

        public void PushBuZhang(string uid, int card, int mCard)
        {
            CmdList.Add(new CmdEntity()
            {
                Uid = uid,
                AType = ActionType.BuZhang,
                Card = card,
                Card1 = mCard
            });
        }

    }

    [Serializable]
    public enum ActionType
    {
        Mo = 0,
        Da,
        Chi,
        Peng,
        Gang,
        FristHu,
        Hu,
        ZhuoPao,
        BuZhang,
        GangDa
    }

    [Serializable]
    public class CmdPlayer
    {
        public string Uid { get; set; }

        public List<int> Cards { get; set; }

    }

    [Serializable]
    public class CmdEntity
    {
        public CmdEntity()
        {
            this.Card = -1;
            this.Card1 = -1;
            this.Card2 = -1;
            this.HuType = -1;
        }

        public string Uid { get; set; }

        public ActionType AType { get; set; }

        public int Card { get; set; }

        public int Card1 { get; set; }

        public int Card2 { get; set; }

        public int HuType { get; set; }
    }
}
