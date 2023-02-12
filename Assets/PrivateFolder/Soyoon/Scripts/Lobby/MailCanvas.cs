using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class MailCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject Mail;
        [SerializeField]
        private Transform mailContentTransform;

        private void OnEnable()
        {
            InitializeMails();
        }

        private void InitializeMails()
        {
            //for (int i = 0; i < DataManager.Instance.mailedPhotoCollectionItemList.Count; i++)
            //{
            //    GameObject photoButton = Instantiate(photo, PhotoContentTransform, false);
            //    PhotoButton photoImg = photoButton.GetComponent<PhotoButton>();
            //    photoImg.photo.GetComponent<Image>().sprite = DataManager.Instance.earnedPhotoCollectionItemList[i].itemIcon;
            //}
            //
            //for(int i=0; i< DataManager.Instance.mailedBadgeCollectionItemList.Count; i++)
            //{
            //
            //}
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
