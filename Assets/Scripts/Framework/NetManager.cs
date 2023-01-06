using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Scripts.Framework {
    //网络连接状态
    public enum NetEvent {
        ConnectSucc = 1,
        ConnectFail = 2,
        Close = 3
    }
    
    public class NetManager {
        private const string TAG = "Netmanager";
        public delegate void eventLisener(string str);
        Dictionary<string, eventLisener> _test = new Dictionary<string, eventLisener>();
        eventLisener lisener1;
        void Start () {
            lisener1 += test01;
            if (_test.ContainsKey("1")) {
                //已经代表value了
                _test["1"] += lisener1;
            } else
                _test ["1"] = lisener1;
            foreach (var item in _test) {
                item.Value.Invoke("01");
            }
        }
        void test01(string s) {
            Debug.Log(s);
        }
        // 增加监听
        public static void AddEventListener(NetEvent netEvent, EventLisener lisener) {
            if (eventListeners.ContainsKey(netEvent)) {
                eventListeners[netEvent] += lisener;
            } else {
                // 直接加key和value
                eventListeners[netEvent] = lisener;
            }
        }
        // 移除监听
        public static void RemoveEventListener(NetEvent netEvent, EventLisener lisener) {
            eventListeners[netEvent] = null;
            if (eventListeners[netEvent]==null) {
                // 移除key 就可以
                eventListeners.Remove(netEvent);
            }
        }
        private static void FireEvent(NetEvent netEvent, string err) {
            if (eventListeners.ContainsKey(netEvent)) {
                // 两种写法都可以
                // eventListeners[netEvent](err);
                eventListeners[netEvent].Invoke(err);
            }
        }
        static Socket socket;
        // 接收缓冲区
        static ByteArray readBuff;
        // 写入缓冲区(发送缓冲区)
        static Queue<ByteArray> writeQueue;
        public static void Connect(string ip, int port) {
            // 判断状态
            if (socket != null && socket.Connected) {
                Debug.Log("初始化连接失败,已经连接了");
                return;
            }
            if (isConnecting) {
                Debug.Log("连接失败,正在连接中...");
                return;
            }
            // 初始化缓存等信息
            InitState();
            // 参数设置
            socket.NoDelay = true;
            // 连接状态
            isConnecting = true;
            // 开始连接
            socket.BeginConnect(ip, port, ConnectCallBack, socket);
        }

        private static void ConnectCallBack(IAsyncResult ar) {
            try {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                Debug.Log("连接成功!");
                FireEvent(NetEvent.ConnectSucc, "");
                isConnecting = false;
// .在之前章节中所介绍的网络通讯.都是在连接成功之后就开始接收消息.在这里我们并没有这样写.因为我们还没有写好接收数据的方法,现在就把这个方法放在连接服务器成功之后的回调方法中.
                // 开始接收
                socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain,
                                    SocketFlags.None, ReceiveCallback, socket);
            }
            catch (SocketException e) {
                Debug.Log("连接失败:" + e.Message);
                FireEvent(NetEvent.ConnectFail, "");
                isConnecting = false; 
                Debug.Log(e.Message); 
            }
        }
// 对于接收消息的处理和上一节几乎一样,当接收到的数据长度为0的时候,就会断开链接,因为这是一条中止链接的信号.
// 如果收到了某一条完整的消息或者不完整的消息.他都会去修改缓冲区类中的Wirteidx.在调用OnReceiveData去处理数据。
        // 接收回调
        private static void ReceiveCallback(IAsyncResult ar) {
            try {
                Socket socket = (Socket)ar.AsyncState;
                //获取接收数据的长度
                int count = socket.EndReceive(ar);
                if (count == 0) {
                    Close();
                    return;
                }
                readBuff.writeIdx += count;
                //处理消息
                OnReceiveData();
                //继续接收消息
                if (readBuff.remain < 8) {
                    readBuff.MoveBytes();
                    readBuff.ReSize(readBuff.length * 2);
                }
                socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain,
                                    SocketFlags.None, ReceiveCallback, socket);
            }
            catch (Exception e ) {
                Debug.Log("recFail" + e.Message);
            }
        }

        // 数据处理
        private static void OnReceiveData() {
            // 消息严重不全
            if (readBuff.length <= 2)
                return;
            int readIdx = readBuff.readIdx;
            byte[] bytes = readBuff.bytes;
            Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | bytes[readIdx]);
            //消息不全
            if (readBuff.length < bodyLength) {
                return;
            }
            //消息全的,再加2,后面都是json的字节数组
            readBuff.readIdx += 2;
            //解析
            int nameCount = 0;
            string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
            if (protoName == "") {
                Debug.Log("Rec协议名fail");
                return;
            }
            readBuff.readIdx += nameCount;
            //解析协议体
            int bodyCount = bodyLength - nameCount;
            MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);

            readBuff.readIdx += bodyCount;
            readBuff.CheckAndMoveBytes();
            // 消息添加到对列中
            lock (msgList) {
                msgList.Add(msgBase);
            }
            msgCount++;
            //test 解析
            if (msgCount > 0) {
                MsgBase m = msgList.First();
                Debug.Log("及时消息的协议名称:"+ m.protoName);
            }
            // 继续解析消息
            if (readBuff.length > 2)
                OnReceiveData();
        }
        
        static bool isConnecting = false;
        static bool isClosing = false;

        public static void Close() {
            if (socket == null || !socket.Connected) {
                Debug.Log("已经断开连接");
                return;
            }
            if (isConnecting) {
                return;
            }
            // 数据还未发送完成
            if (writeQueue.Count > 0) {
                isClosing = false;
            } else {
                // 断开连接
                socket.Close();
                FireEvent(NetEvent.Close, "");
            }
        }
        
        // 消息列表
        static List<MsgBase> msgList = new List<MsgBase>();
        // 列表的长度
        static int msgCount = 0;
        // 每一帧处理消息的条数
        readonly static int MAX_MESSAGE_FIRE = 10;
        
#region 心跳检测
        // 是否需要心跳检测
        public static bool isUsePing = true;
        // 心跳间隔时间
        public static int pingInterval = ;
        // 上一次发送Ping的事间
        static float lastPingTime = ;
        // 上一次收到Pong时间
        static float lastPongTime = ;
#endregion

        // 初始化
        public static void InitState() {
            msgList = new List<MsgBase>();
            msgCount = 0;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 初始化缓冲区
            readBuff = new ByteArray();
            writeQueue = new Queue<ByteArray>();
            isConnecting = false;
            isClosing = false;

            //上一次发送Ping协议的时间
            lastPingTime = Time.time;
            //上一次接收Pong协议的时间
            lastPongTime = Time.time;
            if (!msgListeners.ContainsKey("MsgPong"))
            {
                msgListeners.Add("MsgPong", OnMsgPong);
            }
        }
#region Send 发送消息 
        public static void Send(MsgBase msg) {
            //状态判断
            if (socket == null || !socket.Connected)
                return;
            if (isConnecting)
                return;
            if (isClosing)
                return;
            //消息编码
            byte[] nameBytes = MsgBase.EncodeName(msg);//消息体长度已经带有+2
            byte[] bodyBytes = MsgBase.Encode(msg);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[len + 2];
            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            //复制名字字节数组
            Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
            //复制消息体字节数组
            Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
            //消息体写入队列
            ByteArray ba = new ByteArray(sendBytes);
            int count = 0;
            lock (writeQueue) {
                writeQueue.Enqueue(ba);
                count = writeQueue.Count;
            }
            //Send
            if (count == 1) {
                socket.BeginSend(sendBytes, 0, sendBytes.Length,
                                 SocketFlags.None, SendCallback, socket);
            }
        }
        private static void SendCallback(IAsyncResult ar) {
            Socket socket = ar.AsyncState as Socket;
            if (socket == null || socket.Connected)
                return;
            //EndSend
            int Count = socket.EndSend(ar);
            //获取写入队列的第一条数据
            ByteArray ba;
            lock (writeQueue) {
                ba = writeQueue.First();
            }
            //判断是否完全发送完
            ba.readIdx += Count;
            //发送完毕
            if (ba.length == 0) {
                lock (writeQueue) {
                    writeQueue.Dequeue();
                    ba = writeQueue.First();
                }
            }
            //若一整条没法送完,或者 还有新的消息,继续发送
            if (ba != null)
                socket.BeginSend(ba.bytes, ba.readIdx, ba.length,
                                 SocketFlags.None, SendCallback, socket);
            else if(isClosing) {
                socket.Close();
            }
        }
#endregion

#region 消息处理
        public static void Update() {
            MsgUpdate();
            PingUpdate();
        }
        public static void MsgUpdate() {
            if (msgCount <= )
            {
                return;
            }
            //每帧处理10条消息
            for (int i = ; i < MAX_MESSAGE_FIRE; i++)
            {
                //初始化json数据容器
                MsgBase msgBase;
                msgBase = null;
                lock (msgList)
                {
                    if (msgCount > )
                    {
                        //msgBase = msgList.First();
                        msgBase = msgList[];
                        msgList.RemoveAt();
                        msgCount--;
                    }
                }
                //分发消息
                if (msgBase != null)
                {
                    FireMsg(msgBase.protoName, msgBase);
                }
                else
                {
                    break;
                }
            }
        }
#endregion
#region 心跳检测
        //心跳检测
        public static void PingUpdate() {
            if (!isUsePing)
                return;
            //发送ping协议
            if (Time.time - lastPingTime > pingInterval)
            {
                MsgPing msgPing = new MsgPing {

                };
                Send(msgPing);
                Debug.Log("发送心跳检测");
                //更新发送ping的时间
                lastPingTime = Time.time;
            }
            //Pong的回应情况
            if (Time.time - lastPongTime > pingInterval * ) {
                Debug.Log("无回应,关闭连接");
                //5次没有回应
                Close();
            }
        }
//Pong的回应
        private static void OnMsgPong(MsgBase msgBase)
            {
                lastPongTime = Time.time;
            }
#endregion

#region MsgLister Management
        //消息事件委托类型(与状态事件相似)
        public delegate void MsgLisener(MsgBase msgBase);
        //消息监听列表 字典
        private static Dictionary<string, MsgLisener>
        msgListeners = new Dictionary<string, MsgLisener>();
        //增加消息监听
        public static void AddMsgListener(string msgName, MsgLisener lisener) {
            //添加
            if (msgListeners.ContainsKey(msgName)) {
                msgListeners[msgName] = lisener;
            } else { //新增                     
                msgListeners[msgName] = lisener;
            }
        }
        //删除消息监听
        public static void RemoveMsgListener(string msgName, MsgLisener lisener)
            {
                if (msgListeners.ContainsKey(msgName))
                {
                    msgListeners[msgName] -= lisener;
                    if (msgListeners[msgName] == null)
                        msgListeners.Remove(msgName);
                }
            }

        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="msgBase"></param>
        private static void FireMsg(string msgName, MsgBase msgBase)
            {
                if (msgListeners.ContainsKey(msgName))
                {
                    //执行方法
                    msgListeners[msgName](msgBase );
                }
            }
#endregion
    }
}
