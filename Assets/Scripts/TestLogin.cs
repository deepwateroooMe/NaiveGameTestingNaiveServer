using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {

    public class TestLogin : MonoBehaviour {

        // private void Start()
        //     {
		// 	protocol.LoginMsg loginMsg = new protocol.LoginMsg
		// 	{
        //             userName = "Jtro",
        //             userPsw = "99999"
        //         };
        //         byte[] bs_proto = Encode_Proto(loginMsg);
        //         Debug.Log(System.BitConverter.ToString(bs_proto));
        //     }
        // public static byte[] Encode_Proto(ProtoBuf.IExtensible msgBase)
        //     {
        //         using (var memory = new System.IO.MemoryStream())
        //         {
        //             ProtoBuf.Serializer.Serialize(memory, msgBase );
        //             return memory.ToArray();
        //         }
        //     }

// // 下面的几个方法，肯定是放错地方了吧
//         void Decode(byte [] bytes) {
//             // 消息格式是长度+协议名 
//             Int16 len = (Int16)(bytes[1] << 8 | bytes[0]);
//             Debug.Log(len);
//             string name = System.Text.Encoding.UTF8.GetString(bytes, 2,len);
//             Debug.Log(name);
//         }
//         // 编码协议名
//         // <param name="msgBase">继承MsgBase的子类</param>
//         // <returns>byte[]类型协议名 </returns>
//         public static byte[] EncodeName(MsgBase msgBase) {
//             byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
//             Debug.Log(msgBase.protoName);
//             //协议名长度
//             Int16 len = (Int16)nameBytes.Length;
//             byte[] bytes = new byte[2 + len];
//             //组装协议长度byte数组
//             bytes[0] = (byte)(len % 256);
//             bytes[1] = (byte)(len / 256); 
//             Array.Copy(nameBytes, 0, bytes, 2, len);
//             return bytes;
//         }
//         // 解码协议名
//         // <param name="bytes">协议字节数组</param>
//         // <param name="offset">开始解码的字节下标</param>
//         // <param name="count"></param>
//         public static string DecodeName(byte[] bytes, int offset, out int count) {
//             // Debug.Log(bytes.Length);
//             count = 0;
//             //
//             if (offset + 2 > bytes.Length)
//                 return "";
//             //读取长度
//             Int16 len = (Int16)(bytes[offset + 1] << 8 | bytes[offset]);
//             if (offset + 2 + len > bytes.Length)
//                 return "";
//             //解析协议名
//             count = len + 2;//在字节数组占用的总长度
//             string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
//             return name;
//         }

//         private void Start() {
//             LoginMsg loginMsg = new LoginMsg {
//                 userName = "Vincent",
//                 userPsw = "123456"
//             };
//             //获得协议名称的byte长度
//             byte[] protoName = System.Text.Encoding.UTF8.GetBytes(loginMsg.protoName);
//             Int16 len = (Int16)protoName.Length;
//             byte[] bytes = new byte[2 + len];
//             //组装
//             bytes[0] = (byte)(len % 256);
//             bytes[1] = (byte)(len / 256);
//             Array.Copy(protoName, 0, bytes, 2, len);
//             //解析
//             Debug.Log(BitConverter.ToString(bytes)); 
//             string name = System.Text.Encoding.UTF8.GetString(bytes, 2, len);
//             Debug.Log( name);
//         }

        // private void Start() {
        //         string s="{\"userName\":\"Vincent\",\"password\":\"123456\"}";
        //         // // 解析 v2
        //         // Login login2 = new Login();
        //         // login2= JsonUtility.FromJson<Login>(s);
        //         // Debug.Log(login2.userName + "--" + login2.password);
        //         // // 解析 v3
        //         // Login login2 = new Login();
        //         // JsonUtility.FromJsonOverwrite(s, login2);
        //         // Debug.Log(login2.userName + "--" + login2.password);
        //         Login login = new Login
        //         {
        //             userName = "Vincent",
        //             password ="123456"
        //         };
        //         byte[] bytes = MsgBase.Encode(loginMsg);
        //         string s = System.Text.Encoding.UTF8.GetString(bytes);
        //         Debug.Log(s);
        //         // string s = JsonUtility.ToJson(login);
        //         // Debug.Log(s);
        //     }
    }
}
