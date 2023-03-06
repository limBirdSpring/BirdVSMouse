using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoYoon
{
    public class TitleManager : MonoBehaviourPunCallbacks
    {
        private bool isClicked = false;

        private void Update()
        {
            if (isClicked)
                return;

            if (Input.anyKeyDown)
            {
                isClicked = true;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("접속 성공");
            SceneManager.LoadScene("LoadScene");
        }
    }
}
