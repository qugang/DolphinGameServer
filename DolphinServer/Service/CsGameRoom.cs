using Free.Dolphin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinServer.Service
{
    /// <summary>
    /// 长沙麻将房间管理
    /// </summary>
    public class CsMjGameRoom
    {

        /// <summary>
        /// 房间号
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 玩家集合
        /// </summary>
        public Queue<CsGamePlayer> players { get; set; }

        int cardIndex = 0;

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

        public void Begin()
        {
            cardIndex = 0;
            RandCard();
            SendCard();
        }

        public void ReLoad()
        {
            cardIndex = 0;
            players.Enqueue(players.Dequeue());
            RandCard();
            SendCard();
        }

        private void RandCard()
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

        private void SendCard()
        {

          //  ControllerFactory.SendController(players.ToList().ConvertAll(p => p.PlayerSession), 1006, cardArray.Take(53).ToArray());

        }
    }
}
