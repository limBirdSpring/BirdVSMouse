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
        private GameObject photoWindow;
        [SerializeField]
        private GameObject badgeWindow;

        [SerializeField]
        private MyInfo myInfo;
        [SerializeField]
        private InitializeNames initializeNames;

        [HideInInspector]
        public int targetBadgeButton;

        private void Start()
        {
            InitializePlayerPanel();
        }

        #region 플레이어 패널 변경시 호출되는 함수

        public void RandomPlayerPanel()
        {
            int imgNum = Random.Range(0, myInfo.collectedCharacters.Length);
            playerImg.sprite = myInfo.collectedCharacters[imgNum];
            int adjectNum = Random.Range(0, initializeNames.adjectives.Length);
            int nameNum = Random.Range(0, initializeNames.names.Length);
            playerName.text = string.Format("{0} {1}", initializeNames.adjectives[adjectNum], initializeNames.names[nameNum]);
            //playerBadge1.gameObject.SetActive(false);
            //playerBadge2.gameObject.SetActive(false);

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
            if (myInfo.lastChosenCharacter != null)
                playerImg.sprite = myInfo.lastChosenCharacter;
            else
                RandomCharacter();

            if (myInfo.lastChosenName != null)
                playerName.text = myInfo.lastChosenName;
            else
                RandomName();

            if(myInfo.lastChosenBadge1 != null)
                playerBadge1.sprite = myInfo.lastChosenBadge1;
            else
                playerBadge1.color = Color.clear;

            if (myInfo.lastChosenBadge2 != null)
                playerBadge2.sprite = myInfo.lastChosenBadge2;
            else
                playerBadge2.color = Color.clear;
        }

        public void RandomCharacter()
        {
            int imgNum = Random.Range(0, myInfo.collectedCharacters.Length);
            playerImg.sprite = myInfo.collectedCharacters[imgNum];
            myInfo.lastChosenCharacter = playerImg.sprite;
        }

        public void RandomName()
        {
            int adjectNum = Random.Range(0, initializeNames.adjectives.Length);
            int nameNum = Random.Range(0, initializeNames.names.Length);
            playerName.text = string.Format("{0} {1}", initializeNames.adjectives[adjectNum], initializeNames.names[nameNum]);
            myInfo.lastChosenName = playerName.text;
            PhotonNetwork.LocalPlayer.NickName = myInfo.lastChosenName;
        }

        public void EndChangePlayerName()
        {
            if (playerName.text.Replace(" ", "") == "")
            {
                int adjectNum = Random.Range(0, initializeNames.adjectives.Length);
                int nameNum = Random.Range(0, initializeNames.names.Length);
                playerName.text = string.Format("{0} {1}", initializeNames.adjectives[adjectNum], initializeNames.names[nameNum]);
            }
            myInfo.lastChosenName = playerName.text;
            PhotonNetwork.LocalPlayer.NickName = myInfo.lastChosenName;
        }
        #endregion 

        #region 플레이어 Photo

        public void ChangePhoto(Sprite photo)
        {
            playerImg.sprite = photo;
            myInfo.lastChosenCharacter = photo;
        }

        public void PhotoButtonClicked()
        {
            photoWindow.SetActive(true);
        }

        #endregion 플레이어 Photo


        #region 플레이어 Badge
        public void BadgeButton1Clicked()
        {
            targetBadgeButton = 0;
            playerBadge1.color = Color.clear;
            badgeWindow.SetActive(true);
            badgeWindow.GetComponent<BadgeSellectCanvas>().FindBadgeAndEnable(playerBadge1.sprite);
            myInfo.lastChosenBadge1 = null;
        }

        public void BadgeButton2Clicked()
        {
            targetBadgeButton = 1;
            playerBadge2.color = Color.clear;
            badgeWindow.SetActive(true);
            badgeWindow.GetComponent<BadgeSellectCanvas>().FindBadgeAndEnable(playerBadge2.sprite);
            myInfo.lastChosenBadge2 = null;
        }

        public void ChangeBadge1(Sprite badge)
        {
            playerBadge1.sprite = badge;
            playerBadge1.color = Color.white;
            myInfo.lastChosenBadge1 = playerBadge1.sprite;
        }

        public void ChangeBadge2(Sprite badge)
        {
            playerBadge2.sprite = badge;
            playerBadge2.color = Color.white;
            myInfo.lastChosenBadge2 = playerBadge2.sprite;
        }

        #endregion 플레이어 Badge
    }
}
