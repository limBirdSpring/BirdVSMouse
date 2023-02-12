using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class PlayerEntry : MonoBehaviour
    {
        [SerializeField]
        private Image playerChar;
        [SerializeField]
        private TMP_Text playerName;
        [SerializeField]
        private Image playerBadge1;
        [SerializeField]
        private Image playerBadge2;
        [SerializeField]
        private GameObject playerReady;
        [SerializeField]
        private GameObject master;
        [SerializeField]
        public Button exitButton;

        [SerializeField]
        private MyInfo myInfo;

        public int OwnerId { get; private set; }
        public bool IsMaster { get; private set; }

        public void Initialize(int id, string name, int charNum, int badge1Num, int badge2Num, bool isMaster)
        {
            OwnerId = id;
            playerName.text = name;
            // ã�� �� ��ȿ���� ���� ���� ����
            if (charNum != -1)
                playerChar.sprite = DataManager.Instance.FindSpriteWithPhotoNum(charNum);
            else
                playerChar.color = Color.clear;

            if (badge1Num != -1)
                playerBadge1.sprite = DataManager.Instance.FindSpriteWithBadgeNum(badge1Num);
            else
                playerBadge1.color = Color.clear;

            if (badge2Num != -1)
                playerBadge2.sprite = DataManager.Instance.FindSpriteWithBadgeNum(badge2Num);
            else
                playerBadge2.color = Color.clear;

            IsMaster = isMaster;

            if (isMaster)
            {
                playerReady.SetActive(false);
                master.SetActive(true);
            }
            else
            {
                object isPlayerReady;
                if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
                    isPlayerReady = false;
                playerReady.SetActive((bool)isPlayerReady);
                master.SetActive(false);
            }

            // �ڽ��� ������ ��� ���� ��ư Ȱ��ȭ
            if(PhotonNetwork.IsMasterClient)
                exitButton.gameObject.SetActive(true);
            else
                exitButton.gameObject.SetActive(false);
        }

        public void SetPlayerReady(bool ready)
        {
            if (!IsMaster)
            {
                playerReady.SetActive(ready);
                PhotonNetwork.AutomaticallySyncScene = ready; // ���� ���� �Ѿ����
            }
            else
                playerReady.SetActive(false);
        }

        public void OnExitButtonClicked()
        {
            //if (OwnerId == PhotonNetwork.LocalPlayer.ActorNumber)
            //{
               // Hashtable props = new Hashtable();
               // props.Add("IsKicked", true);
               // PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            //}
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(OwnerId == player.ActorNumber)
                {
                    Hashtable props = new Hashtable();
                    props.Add("IsKicked", true);
                    player.SetCustomProperties(props);
                }
            }
        }
    }
}
