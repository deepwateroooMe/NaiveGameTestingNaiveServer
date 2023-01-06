using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.protocol {

	internal class MsgPing : MsgBase {
        public MsgPing() {
                protoName = "MsgPing";
            }
        public class MsgPong : MsgBase
        {
            public MsgPong()
                {
                    protoName = "MsgPong";

                }
        }
	}
}
