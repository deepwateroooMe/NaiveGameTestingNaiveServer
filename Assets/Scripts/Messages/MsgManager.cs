using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts {

    public class MsgManager { // MsgLister Management

        // 消息事件委托类型(与状态事件相似)
        public delegate void MsgLisener(MsgBase msgBase);

        // 消息监听列表 字典
        public static Dictionary<string, MsgLisener> msgListeners = new Dictionary<string, MsgLisener>();

        // 增加消息监听
        public static void AddMsgListener(string msgName, MsgLisener lisener) {
            // 添加
            if (msgListeners.ContainsKey(msgName)) {
                msgListeners[msgName] = lisener;
            } else { // 新增                     
                msgListeners[msgName] = lisener;
            }
        }

        // 删除消息监听
        public static void RemoveMsgListener(string msgName, MsgLisener lisener) {
            if (msgListeners.ContainsKey(msgName)) {
                msgListeners[msgName] -= lisener;
                if (msgListeners[msgName] == null)
                    msgListeners.Remove(msgName);
            }
        }

        // 分发事件
        public static void FireMsg(string msgName, MsgBase msgBase) {
            if (msgListeners.ContainsKey(msgName)) {
                // 执行方法
                msgListeners[msgName](msgBase );
            }
        }
	}
}
