using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SoYoon
{
    public class IntoRoomPanel : MonoBehaviour
    {
        private GameObject makeRoomPanel;

        private void Awake()
        {
            makeRoomPanel = GameObject.Find("PopUpCanvas").transform.GetChild(0).gameObject;
        }

        public void OnCreateRoomButtonClicked()
        {
            makeRoomPanel.SetActive(true);
        }

        public void OnRandomMatchingButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
}
