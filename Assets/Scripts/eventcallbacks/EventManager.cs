using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    // 网络连接状态
    public enum NetEvent {
        SUCC = 1, // ConnectSucc
        FAIL = 2, // ConnectFail
        CLOSE = 3 // Close
    }

	public class EventManager {

        static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();

        public delegate void EventListener(string str);

        // 增加监听
        public static void AddEventListener(NetEvent netEvent, EventListener lisener) {
            if (eventListeners.ContainsKey(netEvent)) {
                eventListeners[netEvent] += lisener;
            } else {
                // 直接加key和value
                eventListeners[netEvent] = lisener;
            }
        }

        // 移除监听
        public static void RemoveEventListener(NetEvent netEvent, EventListener lisener) {
            eventListeners[netEvent] = null;
            if (eventListeners[netEvent]==null) {
                // 移除key 就可以
                eventListeners.Remove(netEvent);
            }
        }

        public static void FireEvent(NetEvent netEvent, string err) {
            if (eventListeners.ContainsKey(netEvent)) {
                // 两种写法都可以
                // eventListeners[netEvent](err);
                eventListeners[netEvent].Invoke(err);
            }
        }

// 下面这点儿简单测试用例，不用管        
        // Dictionary<string, EventListener> _test = new Dictionary<string, EventListener>();
        // EventListener lisener1;
        // void Start () {
        //     lisener1 += test01;
        //     if (_test.ContainsKey("1")) {
        //         // 已经代表value了
        //         _test["1"] += lisener1;
        //     } else
        //         _test ["1"] = lisener1;
        //     foreach (var item in _test) {
        //         item.Value.Invoke("01");
        //     }
        // }
        // void test01(string s) {
        //     Debug.Log(s);
        // }
	}
}
