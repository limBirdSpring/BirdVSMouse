using System.Collections;
using System.Collections.Generic;
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

        public void InitializeMail(Sprite img, string name, string sub = "�������� ������ �����̿���!")
        {
            itemImage.sprite = img;
            itemName.text = name;
            subscription.text = sub;
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OkayButtonClicked()
        {
            // ��� �Լ� ���� (sprite�� index ã��)
            Destroy(gameObject);
        }
    }
}
