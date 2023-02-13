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
            // 모든 뱃지 삭제
            foreach (GameObject obj in badgeButtons)
                Destroy(obj);
            badgeButtons.Clear();

            for (int i = 0; i < DataManager.Instance.earnedCollectionItemList.Count; i++)
            {
                if (DataManager.Instance.earnedCollectionItemList[i].type != ItemType.Badge)
                    continue;

                GameObject badgeButton = Instantiate(badge, badgeContentTransform, false);
                BadgeButton badgeImg = badgeButton.GetComponent<BadgeButton>();
                badgeImg.badgeCollectionItem = DataManager.Instance.earnedCollectionItemList[i];
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