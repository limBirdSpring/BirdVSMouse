using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class PhotoButton : MonoBehaviour
    {
        public GameObject photo;

        private Image photoButtonImg;
        private PlayerPanel playerPanel;
        private PhotoSellectCanvas photoCanvas;

        private void Awake()
        {
            playerPanel = GameObject.Find("Canvas").GetComponentInChildren<PlayerPanel>();
            photoCanvas = GameObject.Find("PopUpCanvas").GetComponentInChildren<PhotoSellectCanvas>();
            photoButtonImg = photo.GetComponent<Image>();
        }

        public void OnClickedPhotoButton()
        {
            playerPanel.ChangePhoto(photoButtonImg.sprite);
            photoCanvas.gameObject.SetActive(false);
        }
    }
}
