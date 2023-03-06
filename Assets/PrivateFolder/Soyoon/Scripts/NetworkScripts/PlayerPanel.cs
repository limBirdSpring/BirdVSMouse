using Photon.Pun;
using System.Collections.Generic;
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
            if (myInfo.lastChosenCharacter != "")
                playerImg.sprite = DataManager.Instance.GetCollectionItem(myInfo.lastChosenCharacter).itemIcon;
            else
                RandomCharacter();

            if (myInfo.lastChosenName != "")
            {
                playerName.text = myInfo.lastChosenName;
                PhotonNetwork.LocalPlayer.NickName = myInfo.lastChosenName;
            }
            else
                RandomName();

            if(myInfo.lastChosenBadge1 != "")
                playerBadge1.sprite = DataManager.Instance.GetCollectionItem(myInfo.lastChosenBadge1).itemIcon;
            else
                playerBadge1.color = Color.clear;

            if (myInfo.lastChosenBadge2 != "")
                playerBadge2.sprite = DataManager.Instance.GetCollectionItem(myInfo.lastChosenBadge2).itemIcon;
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
            List<CollectionItem> photoItems = new List<CollectionItem>();
            for(int i = 0; i < DataManager.Instance.earnedCollectionItemList.Count; i++)
            {
                if (DataManager.Instance.earnedCollectionItemList[i].type == ItemType.Photo)
                    photoItems.Add(DataManager.Instance.earnedCollectionItemList[i]);
            }
            int imgNum = Random.Range(0, photoItems.Count);
            playerImg.sprite = DataManager.Instance.earnedCollectionItemList[imgNum].itemIcon;
            myInfo.lastChosenCharacter = DataManager.Instance.earnedCollectionItemList[imgNum].itemName;
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

        public void ChangePhoto(string photoName)
        {
            CollectionItem targetItem = DataManager.Instance.GetCollectionItem(photoName);
            playerImg.sprite = targetItem.itemIcon;
            myInfo.lastChosenCharacter = targetItem.itemName;

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
            myInfo.lastChosenBadge1 = "";

            Hashtable props = new Hashtable()
            {
                { "Badge1Num", myInfo.lastChosenBadge1 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public void BadgeButton2Clicked()
        {
            targetBadgeButton = 1;
            playerBadge2.color = Color.clear;
            badgeWindow.SetActive(true);
            badgeWindow.GetComponent<BadgeSellectCanvas>().FindBadgeAndEnable(playerBadge2.sprite);
            playerBadge2.sprite = null;
            myInfo.lastChosenBadge2 = "";

            Hashtable props = new Hashtable()
            {
                { "Badge2Num", myInfo.lastChosenBadge2 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public void ChangeBadge1(string badgeName)
        {
            CollectionItem targetItem = DataManager.Instance.GetCollectionItem(badgeName);
            playerBadge1.sprite = targetItem.itemIcon;
            playerBadge1.color = Color.white;
            myInfo.lastChosenBadge1 = targetItem.itemName;

            Hashtable props = new Hashtable()
            {
                { "Badge1Num", myInfo.lastChosenBadge1 },
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            DataManager.Instance.SaveToJson();
        }

        public void ChangeBadge2(string badgeName)
        {
            CollectionItem targetItem = DataManager.Instance.GetCollectionItem(badgeName);
            playerBadge2.sprite = targetItem.itemIcon;
            playerBadge2.color = Color.white;
            myInfo.lastChosenBadge2 = targetItem.itemName;

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
