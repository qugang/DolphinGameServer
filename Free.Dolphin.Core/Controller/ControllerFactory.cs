using Free.Dolphin.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public class ControllerFactory
    {

        private static Dictionary<int, Func<ControllerContext, object>> _controllerInitCache = new Dictionary<int, Func<ControllerContext, object>>();

        public static void InitController(Assembly assembly)
        {
            foreach (var row in assembly.GetTypes())
            {
                if (row.BaseType == typeof(ControllerBase))
                {
                    ControllerProtocolAttribute ca = row.GetCustomAttribute<ControllerProtocolAttribute>();

                    if (ca != null)
                    {
                        _controllerInitCache.Add(ca.ProtocolNumber, ReflectionUtil.CreateInstanceDelegate<ControllerContext>(row));
                    }
                }
            }
        }

        internal static ControllerBase CreateController(ControllerContext context)
        {
            return _controllerInitCache[context.ProtocolId](context) as ControllerBase;
        }

        public static async void SendController(List<GameSession> session, int protocol, Dictionary<string, string> keyValue)
        {
            foreach (var row in session)
            {
                await Task.Factory.StartNew(() =>
                {
                    ControllerContext context = new ControllerContext(keyValue);
                    ControllerBase controller = CreateController(context);
                    byte[] sendByte = controller.ProcessAction();
                    List<byte> list = new List<byte>();
                    list.Add((byte)(protocol >> 8));
                    list.Add((byte)(protocol & 0xFF));
                    list.AddRange(sendByte);
                    row.SocketClient.Send(list.ToArray());
                }
                );
            }
        }
    }
}
