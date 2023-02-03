using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoYoon
{
    public class TitleManager : MonoBehaviourPunCallbacks
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("접속 성공");
            SceneManager.LoadScene("LobbyTestScene");
        }
    }
}
