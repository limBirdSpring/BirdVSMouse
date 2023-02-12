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
        private InitializeNames initializeNames;
        [HideInInspector]
        public int targetBadgeButton;

        private MyInfo myInfo;

        private void Start()
        {
            myInfo = DataManager.Instance.myInfo;
            InitializePlayerPanel();
        }

        #region 플레이어 패널 변경시 호출되는 함수

        public void InitializePlayerPanel()
        {
            if (myInfo.lastChosenCharacter != -1)
                playerImg.sprite = DataManager.Instance.earnedPhotoCollectionItemList[myInfo.lastChosenCharacter].itemIcon;
            else
                RandomCharacter();

            if (myInfo.lastChosenName != "")
            {
                playerName.text = myInfo.lastChosenName;
                PhotonNetwork.LocalPlayer.NickName = myInfo.lastChosenName;
            }
            else
                RandomName();

            if(myInfo.lastChosenBadge1 != -1)
                playerBadge1.sprite = DataManager.Instance.earnedBadgeCollectionItemList[myInfo.lastChosenBadge1].itemIcon;
            else
                playerBadge1.color = Color.clear;

            if (myInfo.lastChosenBadge2 != -1)
                playerBadge2.sprite = DataManager.Instance.earnedBadgeCollectionItemList[myInfo.lastChosenBadge2].itemIcon;
            else
                playerBadge2.color = Color.clear;

            Hashtable props = new Hashtable()
            {
                { "CharNum", myInfo.lastChosenCharacter },
                { "Badge1Num", myInfo.lastChosenBadge1 },
                { "Badge2Num", myInfo.lastChosenBadge2 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public void RandomCharacter()
        {
            int imgNum = Random.Range(0, DataManager.Instance.earnedPhotoCollectionItemList.Count);
            playerImg.sprite = DataManager.Instance.earnedPhotoCollectionItemList[imgNum].itemIcon;
            myInfo.lastChosenCharacter = imgNum;
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

            DataManager.Instance.SaveToJson();
        }
        #endregion 

        #region 플레이어 Photo

        public void ChangePhoto(Sprite photo)
        {
            playerImg.sprite = photo;
            myInfo.lastChosenCharacter = DataManager.Instance.FindPhotoSpriteNum(photo);

            Hashtable props = new Hashtable()
            {
                { "CharNum", myInfo.lastChosenCharacter },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
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
            playerBadge1.sprite = null;
            myInfo.lastChosenBadge1 = -1;

            Hashtable props = new Hashtable()
            {
                { "Badge1Num", myInfo.lastChosenBadge1 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public void BadgeButton2Clicked()
        {
            targetBadgeButton = 1;
            playerBadge2.color = Color.clear;
            badgeWindow.SetActive(true);
            badgeWindow.GetComponent<BadgeSellectCanvas>().FindBadgeAndEnable(playerBadge2.sprite);
            playerBadge2.sprite = null;
            myInfo.lastChosenBadge2 = -1;

            Hashtable props = new Hashtable()
            {
                { "Badge2Num", myInfo.lastChosenBadge2 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public void ChangeBadge1(Sprite badge)
        {
            playerBadge1.sprite = badge;
            playerBadge1.color = Color.white;
            myInfo.lastChosenBadge1 = DataManager.Instance.FindBadgeSpriteNum(badge);

            Hashtable props = new Hashtable()
            {
                { "Badge1Num", myInfo.lastChosenBadge1 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public void ChangeBadge2(Sprite badge)
        {
            playerBadge2.sprite = badge;
            playerBadge2.color = Color.white;
            myInfo.lastChosenBadge2 = DataManager.Instance.FindBadgeSpriteNum(badge);

            Hashtable props = new Hashtable()
            {
                { "Badge2Num", myInfo.lastChosenBadge2 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public bool CheckIfIsSameBadge(Sprite targetSprite)
        {
            if ((playerBadge1.sprite == targetSprite) || (playerBadge2.sprite == targetSprite))
                return true;
            else
                return false;
        }

        #endregion 플레이어 Badge
    }
}
