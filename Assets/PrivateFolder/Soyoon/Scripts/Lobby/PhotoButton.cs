using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class PhotoButton : MonoBehaviour
    {
        public GameObject photo;

        [HideInInspector]
        public CollectionItem photoCollectionItem;

        private Image photoButtonImg;
        private PlayerPanel playerPanel;
        private PhotoSellectCanvas photoCanvas;

        private void Awake()
        {
            playerPanel = GameObject.Find("Canvas").GetComponentInChildren<PlayerPanel>();
            photoCanvas = GameObject.Find("PopUpCanvas").GetComponentInChildren<PhotoSellectCanvas>();
            photoButtonImg = photo.GetComponent<Image>();
        }
        private void Start()
        {
            InitializePhotoButton();
        }

        private void InitializePhotoButton()
        {
            photoButtonImg.sprite = photoCollectionItem.itemIcon;
        }

        public void OnClickedPhotoButton()
        {
            playerPanel.ChangePhoto(photoCollectionItem.itemName);
            photoCanvas.gameObject.SetActive(false);
        }
    }
}
