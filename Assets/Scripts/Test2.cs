using Assets.Scripts.Framework;
using protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {

	internal class Test2 : MonoBehaviour {

        MsgMove msgMove;
        
        private void Start() {
            msgMove = new MsgMove();
            msgMove.x = 100;
            msgMove.y = -20;
            msgMove.z = 0;
            NetManager.Connect("127.0.0.1",8888);
        }

        public void Onclick() {
            NetManager.Send(msgMove);
        }
	}
}
