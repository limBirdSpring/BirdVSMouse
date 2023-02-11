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
            for (int i = 0; i < DataManager.Instance.earnedCollectionItemList.Count; i++)
            {
                if (DataManager.Instance.earnedCollectionItemList[i].type == ItemType.Photo)
                {
                    GameObject photoButton = Instantiate(photo, PhotoContentTransform, false);
                    PhotoButton photoImg = photoButton.GetComponent<PhotoButton>();
                    photoImg.photo.GetComponent<Image>().sprite = DataManager.Instance.earnedCollectionItemList[i].itemIcon;
                }
            }
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
