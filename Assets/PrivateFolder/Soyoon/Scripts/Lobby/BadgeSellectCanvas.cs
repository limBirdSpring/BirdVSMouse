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
        private Transform badgeContentTransform;

        private List<GameObject> badgeButtons;

        private void Awake()
        {
            badgeButtons = new List<GameObject>();
        }

        private void OnEnable()
        {
            InitializeBadges();
        }

        private void InitializeBadges()
        {
            for (int i = 0; i < DataManager.Instance.earnedCollectionItemList.Count; i++)
            {
                if (DataManager.Instance.earnedCollectionItemList[i].type != ItemType.Badge)
                    continue;

                if(badgeButtons.Count == 0) // 처음 시작
                {
                    GameObject badgeButton = Instantiate(badge, badgeContentTransform, false);
                    BadgeButton badgeImg = badgeButton.GetComponent<BadgeButton>();
                    badgeImg.badgeCollectionItem = DataManager.Instance.earnedCollectionItemList[i];
                    badgeButtons.Add(badgeButton);
                }
                else
                {
                    if (badgeButtons.Count == DataManager.Instance.EarnedBadgesNum)
                        break;
                    else
                    {
                        GameObject badgeButton = Instantiate(badge, badgeContentTransform, false);
                        BadgeButton badgeImg = badgeButton.GetComponent<BadgeButton>();
                        badgeImg.badgeCollectionItem = DataManager.Instance.earnedCollectionItemList[i];
                        badgeButtons.Add(badgeButton);
                    }
                }
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