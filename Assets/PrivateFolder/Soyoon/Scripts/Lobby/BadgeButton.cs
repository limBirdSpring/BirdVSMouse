using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class BadgeButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject badge;
        [HideInInspector]
        public Button badgeButton;
        [HideInInspector]
        public Image badgeButtonImg;

        [HideInInspector]
        public CollectionItem badgeCollectionItem;

        private PlayerPanel playerPanel;
        private BadgeSellectCanvas badgeCanvas;

        private void Awake()
        {
            playerPanel = GameObject.Find("Canvas").GetComponentInChildren<PlayerPanel>();
            badgeCanvas = GameObject.Find("PopUpCanvas").GetComponentInChildren<BadgeSellectCanvas>();
            badgeButton = badge.GetComponent<Button>();
            badgeButtonImg = badge.GetComponent<Image>();
        }

        private void Start()
        {
            InitializeBadgeButton();
        }

        private void InitializeBadgeButton()
        {
            badgeButtonImg.sprite = badgeCollectionItem.itemIcon;
        }

        public void OnClickedBadgeButton()
        {
            badgeButton.interactable = false;

            if (playerPanel.CheckIfIsSameBadge(badgeButtonImg.sprite))
            {
                badgeCanvas.gameObject.SetActive(false);
                return;
            }

            if (playerPanel.targetBadgeButton == 0)
                playerPanel.ChangeBadge1(badgeCollectionItem.itemName);
            else
                playerPanel.ChangeBadge2(badgeCollectionItem.itemName);
            badgeCanvas.gameObject.SetActive(false);
        }
    }
}
