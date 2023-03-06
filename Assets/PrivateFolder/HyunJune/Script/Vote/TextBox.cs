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
                playerMessage.color = new Color32(255, 105, 105, 255);
            else
                playerMessage.color = new Color32(107, 201, 255, 255);

            if (PlayGameManager.Instance.playerList[player.GetPlayerNumber()].isDie)
                playerMessage.color = Color.gray;
        }
    }
}

