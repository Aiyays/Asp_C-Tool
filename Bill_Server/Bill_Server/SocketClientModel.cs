using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bill_Server
{
    public class SocketClientModel
    {
        /// <summary>
        /// 客户端Socket对象
        /// </summary>
        Socket sokMsg;
        /// <summary>
        /// 负责 向主窗体文本框显示消息的方法委托
        /// </summary>
        Action<string> dgShowMsg;
        /// <summary>
        ///  负责 从主窗体 中移除 当前连接
        /// </summary>
        Action<string> dgRemoveConnection;
        /// <summary>
        /// 消息监听的线程
        /// </summary>
        Thread threadMsg;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sokMsg"></param>
        /// <param name="dgShowMsg"></param>
        /// <param name="dgRemoveConnection"></param>
        public SocketClientModel(Socket sokMsg, Action<string> dgShowMsg, Action<string> dgRemoveConnection)
        {
            this.sokMsg = sokMsg;
            this.dgShowMsg = dgShowMsg;
            this.dgRemoveConnection = dgRemoveConnection;
            this.threadMsg = new Thread(RecMsg)
            {
                IsBackground = true
            };
            this.threadMsg.Start();
        }
        
        /// <summary>
        /// 循环监听
        /// </summary>
        bool isRec = true;
        /// <summary>
        /// 监听客户端发送来的消息
        /// </summary>
        void RecMsg()
        {
            while (isRec)
            {
                try
                {
                    byte[] arrMsg = new byte[1024*1024];
                    //接收 对应 客户端发来的消息
                    int length = sokMsg.Receive(arrMsg);
                    if (length > 0)
                    {
                        //将接收到的消息数组转成字符串
                        string strMsg = System.Text.Encoding.UTF8.GetString(arrMsg, 0, length);

                        //通过委托 显示消息到 窗体的文本框
                        dgShowMsg(sokMsg.RemoteEndPoint.ToString() + "发来消息：" + strMsg);
                    }
                    else
                    {
                        isRec = false;
                        dgRemoveConnection(sokMsg.RemoteEndPoint.ToString());
                        threadMsg.Abort();
                        threadMsg = null;
                    }
                }
                catch
                {
                    isRec = false;
                    dgRemoveConnection(sokMsg.RemoteEndPoint.ToString());
                    threadMsg.Abort();
                    threadMsg = null;
                }
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="strMsg"></param>
        public void Send(string strMsg)
        {
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
            sokMsg.Send(arrMsg);
        }
    }
}
