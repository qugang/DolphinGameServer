using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core.MQ
{
    public class MQServer
    {

        public static void RunMQ(Action<object> OnMqRevice) {
            MessageQueue mq = new MessageQueue(@".\private$\MsgQueue");

            if (!MessageQueue.Exists(".\\private$\\MsgQueue"))
            {
                MessageQueue.Create(".\\private$\\MsgQueue");
            }

            Task.Factory.StartNew(() => {

                while (true)
                {
                    Message message = mq.Receive();

                    Task.Factory.StartNew(() =>
                    {
                        message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(string) });
                        OnMqRevice(message.Body);
                    });
                }
            });
        }
    }
}
