using Fleck;
using Free.Dolphin.Common.Util;
using Free.Dolphin.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Free.Dolphin.Core
{
    public class WebSocketServerWrap
    {

        public static Func<string, Exception, byte[]> OnErrorMessage { get; set; }

        public static Action<string> OnRevice { get; set; }

        public static Action<Byte[]> OnSend { get; set; }

        public static Action<string> OnOpen { get; set; }

        public static Action<string> OnClose { get; set; }
        public static void Init(string ip, int port)
        {
            var server = new WebSocketServer("ws://192.168.0.103:9001");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    GameSessionManager.AddSession(GameSession.Parse(socket));
                    WebSocketServerWrap.OnOpen(socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort);
                };
                socket.OnClose = () =>
                {
                    GameSession session = GameSessionManager.RemoveSession(socket);

                    WebSocketServerWrap.OnClose(socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort);

                };
                socket.OnMessage = message =>
                {
                    try
                    {
                        WebSocketServerWrap.OnRevice(message);
                        Dictionary<string, string> keyValue = WebSocketPackage.UnPackage(message);
                        ControllerContext context = new ControllerContext(keyValue);
                        context.Session = GameSessionManager.GetSession(socket);
                        ControllerBase controller = ControllerFactory.CreateController(context);
                        if (controller.IsAuth() && !controller.IsLogin())
                        {
                            if (controller.Login())
                            {
                                byte[] sendByte = controller.ProcessAction();
                                List<byte> list = new List<byte>();
                                list.Add((byte)(context.ProtocolId >> 8));
                                list.Add((byte)(context.ProtocolId & 0xFF));
                                list.AddRange(sendByte);
                                WebSocketServerWrap.OnSend(list.ToArray());
                                socket.Send(list.ToArray());
                            }
                            else
                            {
                                socket.OnError(new Exception("断线重登处理失败"));
                            }
                        }
                        else
                        {
                            byte[] sendByte = controller.ProcessAction();
                            if (sendByte != null)
                            {
                                List<byte> list = new List<byte>();
                                list.Add((byte)(context.ProtocolId >> 8));
                                list.Add((byte)(context.ProtocolId & 0xFF));
                                list.AddRange(sendByte);
                                WebSocketServerWrap.OnSend(list.ToArray());
                                socket.Send(list.ToArray());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        socket.OnError(ex);
                    }
                };

                socket.OnError = error =>
                {
                    if (!socket.IsAvailable)
                    {
                        socket.OnClose();
                    }
                    else
                    {
                        byte[] array = WebSocketServerWrap.OnErrorMessage(error.Message, error);
                        List<byte> list = new List<byte>();
                        list.Add((byte)(9999 >> 8));
                        list.Add((byte)(9999 & 0xFF));
                        list.AddRange(array);
                        socket.Send(list.ToArray());
                    }
                };
            });
        }

        public async static void SendPackgeWithUser(string uid, int protocol, byte[] sendByte)
        {
            await Task.Factory.StartNew(() =>
            {

                List<byte> list = new List<byte>();
                list.Add((byte)(protocol >> 8));
                list.Add((byte)(protocol & 0xFF));
                list.AddRange(sendByte);

                GameUserManager.SendPackgeWithUser(uid, list.ToArray());
            });
        }
    }
}
