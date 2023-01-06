using Assets.Scripts.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {

    public class Test : MonoBehaviour {

        private void Start() {
            NetManager.Connect("127.0.0.1", ); 
            NetManager.AddMsgListener("MsgMove", OnMsgMove);
        }

        public void Send() {
            MsgMove msgMove = new MsgMove {
                x = ,
                y = ,
                z = -110
            };
            NetManager.Send(msgMove);
        }

        public void OnMsgMove(MsgBase msgBase) {
            MsgMove msg = msgBase as MsgMove;
            Debug.Log(msg.protoName);
            Debug.Log(msg.x);
            Debug.Log(msg.y);
            Debug.Log(msg.z);
        }

        private void Update() {
            NetManager.Update();
        }

        // private void Start() {
        //     NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc,OnConnectSucc);
        //     NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        //     NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        // }
        // public void ConnectClick() {
        //     NetManager.Connect("127.0.0.1", 8888);
        // }
        // void OnConnectSucc(string s) {
        //     Debug.Log("连接成功!");
        // }
        // void OnConnectFail(string s) {
        //     Debug.Log("连接失败");
        // }
        // void OnConnectClose(string s) {
        //     Debug.Log("已关闭");
        // }
    }
}
