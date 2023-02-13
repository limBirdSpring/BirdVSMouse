using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class MailCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject mail;
        [SerializeField]
        private Transform mailContentTransform;

        [HideInInspector]
        public List<GameObject> Mails;

        private void Awake()
        {
            Mails = new List<GameObject>();
        }

        private void OnEnable()
        {
            InitializeMails();
        }

        private void InitializeMails()
        {
            // 모든 메일 삭제
            foreach (GameObject obj in Mails)
                Destroy(obj);
            Mails.Clear();

            for (int i = 0; i < DataManager.Instance.mailedCollectionItemList.Count; i++)
            {
                GameObject mailObj = Instantiate(mail, mailContentTransform, false);
                Mail _mail = mailObj.GetComponent<Mail>();
                _mail.mailedCollectionItem = DataManager.Instance.mailedCollectionItemList[i];
                Mails.Add(mailObj);
            }
        }

        public void BackButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
