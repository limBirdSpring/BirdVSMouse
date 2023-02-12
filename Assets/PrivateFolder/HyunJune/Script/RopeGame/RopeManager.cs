using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;
using Unity.VisualScripting;

namespace Youjeong
{
    public class RopeManager : Mission
    {
        [SerializeField]
        private PhotonView photon;

        [SerializeField]
        protected internal RopeController control;

        public override bool GetScore()
        {
            if (TimeManager.Instance.isCurNight)
            {
                control.moon.ResetPos();

                if (control.moon.MissionSuccess)
                    return true;
                else
                    return false;

            }
            else
            {
                control.sun.ResetPos();

                if (control.sun.MissionSuccess)
                    return true;
                else
                    return false;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            GraphicUpdate();
        }

        public override void OnDisable()
        {
            PlayerUpdateCurMission();
            base.OnDisable();
        }

        public override void GraphicUpdate()
        {
            photon.RPC("LoadUIRPC", RpcTarget.All, null);
        }

        public override void PlayerUpdateCurMission()
        {
            RopeGame[] ropeGames = control.GetComponentsInChildren<RopeGame>();

            photon.RPC("SaveUIRPC", RpcTarget.All, ropeGames);
        }
    }
}

