using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts{

    public class MsgBase {

        //协议名
        public string protoName = "";

        //实例json解析类
        static JavaScriptSerializer js = new JavaScriptSerializer();
        /// <summary> 
        ///编码协议名
        /// </summary>
        /// <param name="msgBase">继承MsgBase的子类</param>
        /// <returns>byte[]类型协议名 </returns>
        public static byte[] EncodeName(MsgBase msgBase)
            {
                byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
                //协议名长度
                Int16 len = (Int16)nameBytes.Length;
                byte[] bytes = new byte[2 + len];
                //组装协议长度byte数组
                bytes[0] = (byte)(len % 256);
                bytes[1] = (byte)(len / 256); 
                Array.Copy(nameBytes, 0, bytes, 2, len);
                return bytes;
            }
        /// <summary>
        /// 解码协议名
        /// </summary>
        /// <param name="bytes">协议字节数组</param>
        /// <param name="offset">开始解码的字节下标</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string DecodeName(byte[] bytes, int offset, out int count)
            {
                count = 0;
                //
                if (offset + 2 > bytes.Length)
                    return "";
                //读取长度
                Int16 len = (Int16)(bytes[offset + 1] << 8 | bytes[offset]);

                if (offset + 2 + len > bytes.Length)
                    return "";

                //解析协议名
                count = len + 2;//在字节数组占用的总长度
                string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
                return name;
            }

        //json->byte[]
        public static byte[] Encode(MsgBase msgBase)
            {
                string s = js.Serialize(msgBase);
                return System.Text.Encoding.UTF8.GetBytes(s);
            }
        /// <summary>
        /// byte[] ->json
        /// </summary>
        /// <param name="protoName">协议名</param>
        /// <param name="bytes">json字节</param>
        /// <param name="offset">便宜量</param>
        /// <param name="count">要转换的长度</param>
        /// <returns></returns>
        public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
            {
                Console.WriteLine("解析出来的协议名:" + protoName);
                string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
                MsgBase msgBase =(MsgBase) js.Deserialize(s, Type.GetType(protoName));
                return msgBase;
            }

        // // json->byte[]
        // public static byte[] Encode(MsgBase msgBase) {
        //     string s = JsonUtility.ToJson(msgBase);
        //     return System.Text.Encoding.UTF8.GetBytes(s);
        // }
        // // byte[] ->json
        // // <param name="protoName">协议名</param>
        // // <param name="bytes">json字节</param>
        // // <param name="offset">便宜量</param>
        // // <param name="count">要转换的长度</param>
        // // public static string Decode(string protoName, byte[] bytes, int offset, int count) {
        // //     string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
        // //     return s;
        // // }
        // public static MsgBase Decode(string protoName, byte [] bytes, int offset, int count) {
        //         string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
        //         MsgBase msgBase =(MsgBase) JsonUtility.FromJson(s, Type.GetType(protoName));
        //         return msgBase;
        // }
        // // 编码协议名
        // // <param name="msgBase">继承MsgBase的子类</param>
        // // <returns>byte[]类型协议名 </returns>
        // public static byte[] EncodeName(MsgBase msgBase) {
        //     byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        //     Debug.Log(msgBase.protoName);
        //     //协议名长度
        //     Int16 len = (Int16)nameBytes.Length;
        //     byte[] bytes = new byte[2 + len];
        //     //组装协议长度byte数组
        //     bytes[0] = (byte)(len % 256);
        //     bytes[1] = (byte)(len / 256); 
        //     Array.Copy(nameBytes, 0, bytes, 2, len);
        //     return bytes;
        // }
        // // 解码协议名
        // // <param name="bytes">协议字节数组</param>
        // // <param name="offset">开始解码的字节下标</param>
        // // <param name="count"></param>
        // public static string DecodeName(byte[] bytes, int offset, out int count) {
        //     // Debug.Log(bytes.Length);
        //     count = 0;
        //     if (offset + 2 > bytes.Length)
        //         return "";
        //     //读取长度
        //     Int16 len = (Int16)(bytes[offset + 1] << 8 | bytes[offset]);
        //     if (offset + 2 + len > bytes.Length)
        //         return "";
        //     //解析协议名
        //     count = len + 2;//在字节数组占用的总长度
        //     string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        //     return name;
        // }
    }
}