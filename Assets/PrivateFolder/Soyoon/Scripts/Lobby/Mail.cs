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
            subscription.text = string.Format("{0}이(가) 우편함에 도착했어요!", mailedCollectionItem.itemName);
        }

        public void OkayButtonClicked()
        {
            // 얻는 함수 구현 (sprite로 index 찾기)
            // 리스트나 저장된 리스트에서 빼주기 -> 얻는 함수에서 하면 될듯
            Destroy(gameObject);
        }
    }
}