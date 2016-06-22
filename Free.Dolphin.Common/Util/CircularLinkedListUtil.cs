using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Common.Util
{
    public static class CircularLinkedListUtil
    {
        public static LinkedListNode<object> NextOrFirst(this LinkedListNode<object> current)
        {
            if (current.Next == null)
                return current.List.First;
            return current.Next;
        }

        public static LinkedListNode<object> PreviousOrLast(this LinkedListNode<object> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            return current.Previous;
        }
    }
}
