using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class BadgeButton : MonoBehaviour
    {
        public GameObject badge;
        [HideInInspector]
        public Button badgeButton;
        [HideInInspector]
        public Image badgeButtonImg;

        private PlayerPanel playerPanel;
        private BadgeSellectCanvas badgeCanvas;

        private void Awake()
        {
            playerPanel = GameObject.Find("Canvas").GetComponentInChildren<PlayerPanel>();
            badgeCanvas = GameObject.Find("PopUpCanvas").GetComponentInChildren<BadgeSellectCanvas>();
            badgeButton = badge.GetComponent<Button>();
            badgeButtonImg = badge.GetComponent<Image>();
        }

        public void OnClickedBadgeButton()
        {
            badgeButton.interactable = false;

            if (playerPanel.targetBadgeButton == 0)
                playerPanel.ChangeBadge1(badgeButtonImg.sprite);
            else
                playerPanel.ChangeBadge2(badgeButtonImg.sprite);
            badgeCanvas.gameObject.SetActive(false);
        }
    }
}
