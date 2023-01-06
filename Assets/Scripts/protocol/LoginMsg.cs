using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.protocol {

    public class LoginMsg {

        public string userName;
        public string userPsw;

        public LoginMsg() {
            protoName = "Login";
        }
	}
}
