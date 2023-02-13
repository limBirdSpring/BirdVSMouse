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

        private List<GameObject> Mails;

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
            for (int i = 0; i < DataManager.Instance.mailedCollectionItemList.Count; i++)
            {
                if(Mails.Count == 0)
                {
                    GameObject mailObj = Instantiate(mail, mailContentTransform, false);
                    Mail _mail = mailObj.GetComponent<Mail>();
                    _mail.mailedCollectionItem = DataManager.Instance.mailedCollectionItemList[i];
                    Mails.Add(mailObj);
                }
                else
                {
                    if (Mails.Count == DataManager.Instance.myInfo.mailedItem.Count)
                        return;
                    else
                    {
                        GameObject mailObj = Instantiate(mail, mailContentTransform, false);
                        Mail _mail = mailObj.GetComponent<Mail>();
                        _mail.mailedCollectionItem = DataManager.Instance.mailedCollectionItemList[i];
                        Mails.Add(mailObj);
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
