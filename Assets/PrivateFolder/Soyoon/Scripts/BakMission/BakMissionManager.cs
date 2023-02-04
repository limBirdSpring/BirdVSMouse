using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoYoon
{
    public class BakMissionManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private BakMission bakMission;

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
            if(!PhotonNetwork.InRoom)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 12 }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room");
        }

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
                    bakMission.UpdatePercentage();
                }
                else
                {
                    Debug.Log("�� Ÿ�� ��� ����");
                    curBakPlayerList.Remove(targetPlayer);
                    curBakPlayerCount = curBakPlayerList.Count;
                    bakMission.UpdatePercentage();
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                SceneManager.LoadScene("MainMapTestScene");
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
            Debug.Log(CurBakProgress);
        }
    }
}
