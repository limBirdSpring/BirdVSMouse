using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class BadgeSellectCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject badge;
        [SerializeField]
        private Transform BadgeContentTransform;

        private List<GameObject> badgeButtons;

        private void Awake()
        {
            badgeButtons = new List<GameObject>();
            InitializeBadges();
        }

        private void InitializeBadges()
        {
            for (int i = 0; i < DataManager.Instance.earnedBadgeCollectionItemList.Count; i++)
            {
                GameObject badgeButton = Instantiate(badge, BadgeContentTransform, false);
                BadgeButton badgeImg = badgeButton.GetComponent<BadgeButton>();
                badgeImg.badge.GetComponent<Image>().sprite = DataManager.Instance.earnedBadgeCollectionItemList[i].itemIcon;
                badgeButtons.Add(badgeButton);
            }
        }

        public void FindBadgeAndEnable(Sprite badgeSprite)
        {
            if (badgeSprite == null)
                return;

            for (int i = 0; i < badgeButtons.Count; i++)
            {
                BadgeButton badgeButton = badgeButtons[i].GetComponent<BadgeButton>();
                if (badgeButton.badgeButtonImg.sprite == badgeSprite)
                {
                    badgeButton.badgeButton.interactable = true;
                    return;
                }
            }
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}