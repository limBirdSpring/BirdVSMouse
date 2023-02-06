using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class KillButton : MonoBehaviour
    {
        public GameObject target;

        public void OnClickedKillButton()
        {
            target?.GetComponent<PlayerControllerTest>().Die();
        }
    }
}
