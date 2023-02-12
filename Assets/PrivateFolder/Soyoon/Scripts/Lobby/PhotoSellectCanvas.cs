using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class PhotoSellectCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject photo;
        [SerializeField]
        private Transform PhotoContentTransform;

        private void Awake()
        {
            InitializePhotos();
        }

        private void InitializePhotos()
        {
            for (int i = 0; i < DataManager.Instance.earnedPhotoCollectionItemList.Count; i++)
            {
                GameObject photoButton = Instantiate(photo, PhotoContentTransform, false);
                PhotoButton photoImg = photoButton.GetComponent<PhotoButton>();
                photoImg.photo.GetComponent<Image>().sprite = DataManager.Instance.earnedPhotoCollectionItemList[i].itemIcon;
            }
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
