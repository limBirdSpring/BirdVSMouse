using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class PhotoSellectCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject photo;
        [SerializeField]
        private Transform photoContentTransform;

        private List<GameObject> photoButtons;

        private void Awake()
        {
            photoButtons = new List<GameObject>();
        }

        private void OnEnable()
        {
            InitializePhotos();
        }

        private void InitializePhotos()
        {
            for (int i = 0; i < DataManager.Instance.earnedCollectionItemList.Count; i++)
            {
                if (DataManager.Instance.earnedCollectionItemList[i].type != ItemType.Photo)
                    continue;

                if (photoButtons.Count == 0) // 처음 시작
                {
                    GameObject photoButton = Instantiate(photo, photoContentTransform, false);
                    PhotoButton photoImg = photoButton.GetComponent<PhotoButton>();
                    photoImg.photoCollectionItem = DataManager.Instance.earnedCollectionItemList[i];
                    photoButtons.Add(photoButton);
                }
                else
                {
                    if (photoButtons.Count == DataManager.Instance.EarnedPhotosNum)
                        break;
                    else
                    {
                        GameObject photoButton = Instantiate(photo, photoContentTransform, false);
                        PhotoButton photoImg = photoButton.GetComponent<PhotoButton>();
                        photoImg.photoCollectionItem = DataManager.Instance.earnedCollectionItemList[i];
                        photoButtons.Add(photoButton);
                    }
                }
            }
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
