using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {

    public class TestDecode : MonoBehaviour {

        // private void Start() {
        //     LoginMsg loginMsg = new LoginMsg {
        //         userName = "Jtro",
        //         userPsw = "99999"
        //     };
        //     byte[] bs_proto = Encode_Proto(loginMsg);
        //     Debug.Log(System.BitConverter.ToString(bs_proto));
        //     // 获取协议名 
        //     string protoName = loginMsg.ToString();
        //     // 解码
        //     ProtoBuf.IExtensible m;
        //     m = Decode_Proto(protoName, bs_proto, , bs_proto.Length);
        //     LoginMsg m2 = m as LoginMsg;
        //     Debug.Log(m2.userName);
        //     Debug.Log(m2.userPsw);
        // }

        // public static byte[] Encode_Proto(ProtoBuf.IExtensible msgBase) {
        //     using (var memory = new System.IO.MemoryStream()) {
        //         ProtoBuf.Serializer.Serialize(memory, msgBase );
        //         return memory.ToArray();
        //     }
        // }

        // // 解析
        // public static ProtoBuf.IExtensible Decode_Proto(string protoName, byte[] bytes, int offset, int count) {
        //     using (var memory = new System.IO.MemoryStream(bytes, offset, count)) {
        //         System.Type t = System.Type.GetType(protoName);
        //         return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
        //     }
        // }
    }
}
