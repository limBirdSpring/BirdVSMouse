using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;

namespace SoYoon
{
    public class BakMissionManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private BakMission bakMission;
        [SerializeField]
        private BakUI bakUI;
        [SerializeField]
        private BarUI barUI;
        [SerializeField]
        private BakMissionPanel BakMissionPanel;
        [SerializeField]
        private TMP_Text progressText;

        private float curBakProgress;
        public static BakMissionManager Instance { get; private set; }

        public float CurBakProgress { get { return curBakProgress; } private set { curBakProgress = value; } }
        private LinkedList<Player> curBakPlayerList;
        private int curBakPlayerCount;

        public int CurBakPlayerCount { get { return curBakPlayerCount; } private set { curBakPlayerCount = value; } }

        private void Awake()
        {
            Instance = this;
            curBakPlayerList = new LinkedList<Player>();
        }

        private void Start()
        {
            curBakProgress = 0;
            progressText.text = "0 %";
            //if (!PhotonNetwork.InRoom)
            //{
            //    PhotonNetwork.ConnectUsingSettings();
            //}
        }
        //public override void OnConnectedToMaster()
        //{
        //    Debug.Log("Connected to Master");
        //    PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 12 }, null);
        //}
        //
        //public override void OnJoinedRoom()
        //{
        //    Debug.Log("Joined room");
        //    CreatePlayer(); // �ӽ�
        //}

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            // �ٲ�� ������Ƽ�鿡 ���� �ٸ� ���� ����
            if (changedProps["IsBakMission"] != null)
            {
                if ((bool)changedProps["IsBakMission"])
                {
                    Debug.Log("�� Ÿ�� ��� �߰�");
                    curBakPlayerList.AddLast(targetPlayer);
                    curBakPlayerCount = curBakPlayerList.Count;
                    barUI.ChangedPlayerCount();
                    bakMission.UpdatePercentage();
                }
                else
                {
                    Debug.Log("�� Ÿ�� ��� ����");
                    curBakPlayerList.Remove(targetPlayer);
                    curBakPlayerCount = curBakPlayerList.Count;
                    barUI.ChangedPlayerCount();
                    bakMission.UpdatePercentage();
                }
            }
        }

        //private void CreatePlayer()
        //{
        //    // �ӽ�
        //    PhotonNetwork.Instantiate("Player", new Vector3(40, 30, 0), Quaternion.identity);
        //}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                curBakProgress = 99; // ���߿� ���ֱ�
            if (Input.GetKeyDown(KeyCode.F2))
                SceneManager.LoadScene("MainMapTestScene"); // ���߿� ���ֱ�
            if (curBakProgress >= 100)
                return;
            if (curBakPlayerCount == 0)
                return;

            foreach(Player player in curBakPlayerList)
            {
                object isSawing;
                if(!player.CustomProperties.TryGetValue("IsSawing", out isSawing))
                {
                    isSawing = false;
                }

                if((bool)isSawing)
                {
                    // ������ �����̰� �ִ� ���
                    curBakProgress += bakMission.UpdateProgress();
                }
            }

            progressText.text = string.Format("{0} %", (int)curBakProgress);

            if (curBakProgress >= 100)
            {
                // �̼��� �Ϸ��� ��� 
                // ��Ʈ��ũ ��Ȳ�� �޶� ������� �ٸ� ��� IsBakDone property �߰��ؼ� ��� done�̸� �Ϸ�ǵ��� ���� ����
                HashTable props = new HashTable();
                props.Add("IsBakMission", false);
                props.Add("IsSawing", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                bakUI.BakMissionComplete();
                BakMissionPanel.BakMissionComplete();
                bakMission.BakMissionComplete();
                progressText.text = string.Format("100 %", (int)curBakProgress);
            }
            Debug.Log(CurBakProgress);
        }
    }
}
