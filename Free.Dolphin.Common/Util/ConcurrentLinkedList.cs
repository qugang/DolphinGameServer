using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Free.Dolphin.Common.Util
{
    public class ConcurrentLinkedList<T> : LinkedList<T>
    {
        SpinLock sl = new SpinLock();

        /// <summary>
        /// 线程安全的AddLast
        /// </summary>
        /// <param name="node"></param>
        public new void AddLast(LinkedListNode<T> node) {
            bool llNodeListLocked = false;
            try
            {
                sl.Enter(ref llNodeListLocked);
                base.AddLast(node);
            
             }
            finally
            {
                if (llNodeListLocked)
                    sl.Exit();
            }
        }

        /// <summary>
        /// 线程安全的AddLast
        /// </summary>
        /// <param name="node"></param>
        public new void AddLast(T node)
        {
            bool llNodeListLocked = false;
            try
            {
                sl.Enter(ref llNodeListLocked);
                base.AddLast(node);

            }
            finally
            {
                if (llNodeListLocked)
                    sl.Exit();
            }
        }
    }
}
