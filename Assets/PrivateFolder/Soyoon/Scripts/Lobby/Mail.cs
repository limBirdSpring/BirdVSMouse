using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoYoon
{
    public class Mail : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private TMP_Text subscription;

        [HideInInspector]
        public CollectionItem mailedCollectionItem;
        [HideInInspector]
        public string sub = "";

        private void Start()
        {
            itemImage.sprite = mailedCollectionItem.itemIcon;
            itemName.text = mailedCollectionItem.itemName;
            subscription.text = string.Format("{0}��(��) �����Կ� �����߾��!", mailedCollectionItem.itemName);
        }

        public void OkayButtonClicked()
        {
            DataManager.Instance.EarnItem(mailedCollectionItem.itemName);
            Destroy(gameObject);
        }
    }
}