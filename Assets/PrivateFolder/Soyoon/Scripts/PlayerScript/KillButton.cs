using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class KillButton : MonoBehaviour
    {
        [HideInInspector]
        public GameObject target;

        public void OnClickedKillButton()
        {
            target?.GetComponent<PlayerControllerTest>().Die();
            // 죽은 상황에서는 kill 범위 감지 꺼주기
            target?.transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
