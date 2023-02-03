using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField]
        private Image playerImg;
        [SerializeField]
        private TMP_InputField playerName;
        [SerializeField]
        private Image playerBadge1;
        [SerializeField]
        private Image playerBadge2;

        [SerializeField]
        private MyInfo myInfo;
        [SerializeField]
        private InitializeNames initializeNames;

        private void Start()
        {
            RandomPlayerPanel();
        }

        #region 플레이어 패널 변경시 호출되는 함수
        public void RandomPlayerPanel()
        {
            int imgNum = Random.Range(0, myInfo.collectedCharacters.Length);
            playerImg.sprite = myInfo.collectedCharacters[imgNum];
            int adjectNum = Random.Range(0, initializeNames.adjectives.Length);
            int nameNum = Random.Range(0, initializeNames.names.Length);
            playerName.text = string.Format("{0} {1}", initializeNames.adjectives[adjectNum], initializeNames.names[nameNum]);
            playerBadge1.gameObject.SetActive(false);
            playerBadge2.gameObject.SetActive(false);

            myInfo.lastChosenName = playerName.text;
            PhotonNetwork.LocalPlayer.NickName = myInfo.lastChosenName;
            myInfo.lastChosenCharacter = playerImg.sprite;
            myInfo.lastChosenBadge1 = null;
            myInfo.lastChosenBadge2 = null;

            Hashtable props = new Hashtable()
            {
                { "CharNum", DataManager.Instance.FindCharSpriteNum(myInfo.lastChosenCharacter) },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public void InitializePlayerPanel()
        {
            playerImg.sprite = myInfo.lastChosenCharacter;
            playerName.text = myInfo.lastChosenName;
            playerBadge1.sprite = myInfo.lastChosenBadge1;
            playerBadge2.sprite = myInfo.lastChosenBadge2;
        }

        public void ChangePlayerImg(Sprite playerSprite)
        {
            playerImg.sprite = playerSprite;
            myInfo.lastChosenCharacter = playerImg.sprite;
        }

        public void EndChangePlayerName()
        {
            if (playerName.text == "")
            {
                int adjectNum = Random.Range(0, initializeNames.adjectives.Length);
                int nameNum = Random.Range(0, initializeNames.names.Length);
                playerName.text = string.Format("{0} {1}", initializeNames.adjectives[adjectNum], initializeNames.names[nameNum]);
                playerBadge1.gameObject.SetActive(false);
                playerBadge2.gameObject.SetActive(false);
            }
            myInfo.lastChosenName = playerName.text;
        }

        public void ChangeBadge1(Sprite badge)
        {
            playerBadge1.sprite = badge;
            playerBadge1.gameObject.SetActive(true);
            myInfo.lastChosenBadge1 = playerBadge1.sprite;
        }

        public void ChangeBadge2(Sprite badge)
        {
            playerBadge2.sprite = badge;
            playerBadge2.gameObject.SetActive(true);
            myInfo.lastChosenBadge2 = playerBadge2.sprite;
        }
        #endregion
    }
}
