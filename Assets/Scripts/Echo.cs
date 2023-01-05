using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Echo : MonoBehaviour {
    private const string TAG = "Echo";

    public GameObject revGo;
    public GameObject sndGo;

    private TMP_Text revTxt;
    private TMP_InputField sndTxt;
    Socket socket;
    string recvStr;
    byte[] buffer = new byte[1024];
    
    void Start() {
        revTxt = revGo.GetComponent<TMP_Text>();
        sndTxt = sndGo.GetComponent<TMP_InputField>();

        IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        IPEndPoint iPEP = new IPEndPoint(ipAdr, 5000);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(iPEP); // 改写成异步 BeginConnect + EndConnect
        // socket.BeginConnect(iPEP, ConnectCallback, socket);
    }
    private void Update() {
        // revTxt.text = recvStr;
        if (socket == null)
            return;
        // 有可读数据
        if (socket.Poll(-1, SelectMode.SelectRead)) {
            byte[] readBuff = new byte[1024];
            int count = socket.Receive(readBuff);
            string recStr = System.Text.Encoding.Default.GetString(readBuff, 0,count);
            revTxt.text = recvStr;
        }
    }
// 我觉得这里我什么地方写得不对，消息并没能真正发送成功，服务器接收不到    
    public void sendBtn() {
        //send
        if (socket == null)
            return;
        if (socket.Poll(-1, SelectMode.SelectWrite))
        {
            string sendStr = sndTxt.text;
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            socket.Send(sendBytes);
        }
//         //socket.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, SendCallBack, socket); // 这里是他写的异步调用方法，终于有参考了！！！
//         string sendStr = sndTxt.text;
//         byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
// 		socket.Send(sendBytes); // <<<<<<<<<<<<<<<<<<<< BeginSend + EndSend
// // 		byte[] readBuff = new byte[1024];
// // // TODO: BUG        
// // 		socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket); // 这里有点儿写不到，不知道怎么设置回调等参数：原
//         来我写得都是对的！！！
// // 2. 异步读取，从服务器
//         int count = socket.Receive(readBuff); // <<<<<<<<<<<<<<<<<<<< BeginReceive + EndReceive
//         string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, 1024);
//         revTxt.text = recvStr;
// // // 3.release resource 释放资源
//         socket.Close();
    }
// 下面是异步回调方法    
    public void ConnectCallback(IAsyncResult ar) {
        try {
            Socket socketCb = ar.AsyncState as Socket; // 这里是拿到索引
            socketCb.BeginReceive(buffer, 0, 1024, 0, ReceiveCallBack, socket); // 开始接收数据
            socketCb.EndConnect(ar );
            Debug.Log("连接服务器完成!");
        }
        catch (SocketException e) {
            Debug.Log("连接服务器失败!" + e);
        }
    }
    public void ReceiveCallBack(IAsyncResult ar) {
        try {
            Socket socketCb = ar.AsyncState as Socket;
            int count = socketCb.EndReceive(ar);
// 测试中还发现,客户端接收到新消息之后会覆盖掉以前的消息,那么客户端也要改一下.            
            // recvStr = System.Text.Encoding.Default.GetString(buffer, 0, count);
            string s = System.Text.Encoding.Default.GetString(buffer, 0, count); // 收到的新消息
            recvStr += s + "\n"; // 把新消息新添加一行，显示出来
            socketCb.BeginReceive(buffer, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException e) {
            Debug.Log("接收数据失败!" + e.Message);
        }
    }
// 这里，我什么也没有实现
	public void SendCallback(IAsyncResult ar) { }
    // public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socket_flags, AsyncCallback callback, object state);
    // Send这个函数不是把数据发送到了服务器那里,而是发送到了系统分配给socket的缓冲区中,
    // 是的,这里走到send的回调函数那里并不代表已经发送到服务器了,而是在系统分配的缓冲区中
    // public int EndSend(IAsyncResult result);
}
