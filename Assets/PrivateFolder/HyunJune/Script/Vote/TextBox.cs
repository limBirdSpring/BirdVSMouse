using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Saebom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyunJune
{
    public class TextBox : MonoBehaviour
    {
        [SerializeField]
        private Image playerIcon;
        [SerializeField]
        private TMP_Text playerName;
        [SerializeField]
        private TMP_Text playerMessage;


        public void SetMessage(Photon.Realtime.Player player, string message)
        {
            playerIcon.sprite = PlayGameManager.Instance.playerList[player.GetPlayerNumber()].sprite;            
            playerName.text = PlayGameManager.Instance.playerList[player.GetPlayerNumber()].name;
            playerMessage.text = message;

            if (PlayGameManager.Instance.playerList[player.GetPlayerNumber()].isBird)
                playerName.color = Color.red;
            else
                playerName.color = Color.blue;

            if (PlayGameManager.Instance.playerList[player.GetPlayerNumber()].isDie)
                playerName.color = Color.gray;
        }
    }
}

