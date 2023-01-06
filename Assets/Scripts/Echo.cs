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

    public InputField iptTxt;
    public Text txtRev;
    //socket
    Socket socket;
    IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
    //发送队列
    Queue<ByteArray> writeQueue = new Queue<ByteArray>();
    //接收缓冲区
    ByteArray readBuff = new ByteArray();
    string recvStr = "";
    // Use this for initialization
    void Start() {
        IPEndPoint iPEP = new IPEndPoint(ipAdr, 5000);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //socket.Connect(iPEP);
        socket.BeginConnect(iPEP,ConnectCallback ,socket);
    }
    public void ConnectCallback(IAsyncResult ar) {
        try {
            Socket socketCb = ar.AsyncState as Socket;
            socketCb.EndConnect(ar);
            Debug.Log("连接服务器完成!");
            //连接上服务器之后就可以开始接收服务器的消息了 
            socketCb.BeginReceive(readBuff.bytes, readBuff .writeIdx,
                                  readBuff.remain, , ReceiveCallBack, socket);
        }
        catch (SocketException e) {
            Debug.Log("连接服务器失败!" + e);
        }
    }
    public void ReceiveCallBack(IAsyncResult ar) {
        try {
            Socket socketCb = ar.AsyncState as Socket;
            //接收数据的长度
            int count = socketCb.EndReceive(ar); 
            readBuff .writeIdx += count;
            //处理粘包消息
            OnReceiveData();
            //recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            if (readBuff.remain < ) {
                readBuff.MoveBytes();
                readBuff.ReSize(readBuff.length * );
            }
            socketCb.BeginReceive(readBuff .bytes , readBuff .writeIdx , readBuff.remain , SocketFlags.None,
                                  ReceiveCallBack, socket); 
        }
        catch (SocketException e) {
            Debug.Log("接收数据失败!" + e.Message);
        }
    }
    //发送消息功能
    public void btnSend() {
        ...
            }
    public void SendCallBack(IAsyncResult ar) {
        ...
            }
    //粘包分包的处理
    public void OnReceiveData() {
        Debug.Log("[Recv 1] length = " + readBuff.length);
        Debug.Log("[Recv 2] readBuff = " +BitConverter .ToString (readBuff .bytes ,readBuff.readIdx,readBuff.length ));
        //缓存中长度小于等于2,代表消息极其不完整
        if (readBuff.length <= ) { 
            return;
        }
        int readIdx = readBuff.readIdx;
        byte[] bytes = readBuff.bytes; 
        //  << 后面是几就代表乘以2的几次方 
        Int16 bodyLength = (Int16)((bytes[readIdx + ] << ) | bytes[readIdx]);
        //缓存中的消息长度大于2,但是还不能组成一条消息 
        if (readBuff.length < bodyLength) {
            return;
        }
        readBuff.readIdx += ;
        Debug.Log("[Recv 3] bodyLength=" + bodyLength);
        //消息完整取出一条消息
        byte[] stringByte = new byte[bodyLength];
        readBuff.Read(stringByte, , bodyLength); 
        string s = System.Text.Encoding.UTF8.GetString(stringByte);
        Debug.Log("Recv 4 s=" + s);
        Debug.Log("Recv 5 readbuff=" + BitConverter.ToString(readBuff.bytes, readBuff.readIdx, readBuff.length)); 
        recvStr = s;
        if (readBuff.length > ) {
            OnReceiveData();
        } 
    }

    private void Update() {
        txtRev.text = recvStr;
    }
}
