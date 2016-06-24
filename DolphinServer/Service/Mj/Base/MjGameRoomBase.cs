using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service.Mj
{
    public abstract class MjGameRoomBase
    {
        public int RoomId { get; set; }
        /// <summary>
        /// 玩家集合
        /// </summary>
        public LinkedList<CsGamePlayer> Players { get; set; }

        /// <summary>
        /// 当前打出的牌
        /// </summary>
        protected int CurrentCard { get; set; }

        protected void SetCurrentCardIsUse()
        {
            this.CurrentCard = this.CurrentCard | 0x200;
        }

        protected void ClaerCurrentCardIsUse()
        {
            this.CurrentCard = this.CurrentCard & 0x1FF;
        }

        protected Boolean CurrentCardIsUse()
        {
            if ((this.CurrentCard & 0x200) > 0)
                return true;
            return false;
        }


        protected LinkedListNode<CsGamePlayer> Player { get; set; }

        public LinkedListNode<CsGamePlayer> FindPlayer(string uid)
        {
            LinkedListNode<CsGamePlayer> next = Players.First;
            for (int i = 0; i < Players.Count; i++)
            {
                if (next.Value.PlayerUser.Uid == uid)
                {
                    return next;
                }
                next = next.Next;
            }
            return null;
        }

        public void SetPlayerReadyFalse()
        {
            foreach (var row in Players)
            {
                row.IsReady = false;
            }
        }



        protected abstract void SendCard();

        public int[] cardArray = {
            0,1,2,3,4,5,6,7,8,
            0,1,2,3,4,5,6,7,8,
            0,1,2,3,4,5,6,7,8,
            0,1,2,3,4,5,6,7,8,
            0 | 0x40,1 | 0x40,2 | 0x40,3 | 0x40,4 | 0x40,5 | 0x40,6 | 0x40,7 | 0x40,8 | 0x40,
            0 | 0x40,1 | 0x40,2 | 0x40,3 | 0x40,4 | 0x40,5 | 0x40,6 | 0x40,7 | 0x40,8 | 0x40,
            0 | 0x40,1 | 0x40,2 | 0x40,3 | 0x40,4 | 0x40,5 | 0x40,6 | 0x40,7 | 0x40,8 | 0x40,
            0 | 0x40,1 | 0x40,2 | 0x40,3 | 0x40,4 | 0x40,5 | 0x40,6 | 0x40,7 | 0x40,8 | 0x40,
            0 | 0x80,1 | 0x80,2 | 0x80,3 | 0x80,4 | 0x80,5 | 0x80,6 | 0x80,7 | 0x80,8 | 0x80,
            0 | 0x80,1 | 0x80,2 | 0x80,3 | 0x80,4 | 0x80,5 | 0x80,6 | 0x80,7 | 0x80,8 | 0x80,
            0 | 0x80,1 | 0x80,2 | 0x80,3 | 0x80,4 | 0x80,5 | 0x80,6 | 0x80,7 | 0x80,8 | 0x80,
            0 | 0x80,1 | 0x80,2 | 0x80,3 | 0x80,4 | 0x80,5 | 0x80,6 | 0x80,7 | 0x80,8 | 0x80
        };

        int cardIndex = 0;

        public virtual void BeginGame()
        {
            cardIndex = 0;
            this.Player = this.Players.First;
            RandCard();
            SendCard();
        }

        public virtual void ReLoadGame()
        {
            cardIndex = 0;
            RandCard();
            SendCard();
        }




        protected void RandCard()
        {
            Random rd = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < cardArray.Length; i++)
            {
                int index = rd.Next(0, cardArray.Length - 1 - i);
                list.Add(cardArray[index]);
                cardArray[index] = cardArray[cardArray.Length - 1 - i];
            }
            cardArray = list.ToArray();
        }
        public int ReadCard()
        {
            if (cardIndex == cardArray.Length)
            {
                throw new Exception("牌已经摸完");
            }

            var tempCard = cardArray[cardIndex];
            cardIndex++;
            return tempCard;
        }






    }
}
