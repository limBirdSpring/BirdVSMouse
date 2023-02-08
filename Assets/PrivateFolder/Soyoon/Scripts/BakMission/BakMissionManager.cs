using Photon.Pun;
using Photon.Realtime;
using Saebom;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Player = Photon.Realtime.Player;

namespace SoYoon
{
    public class BakMissionManager : Mission
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

            if (changedProps["BakMissionReset"] != null)
            {
                if ((bool)changedProps["BakMissionReset"])
                {
                    BakMissionReset();
                }
            }
            if (changedProps["BakMissionComplete"] != null)
            {
                if ((bool)changedProps["BakMissionComplete"])
                {
                    BakMissionComplete();
                }
            }
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F3)) // ���߿� ���ֱ�
            //    BakMissionResetCalled(); // ���߿� ������ ��� �Ҹ� �Լ�
            if (curBakProgress >= 100)
                return;
            if (curBakPlayerCount == 0)
                return;

            foreach (Player player in curBakPlayerList)
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
                BakMissionComplete();
            }
            Debug.Log(CurBakProgress);
        }

        public override bool GetScore()
        {
            // �������� Ȯ���� ��쿡�� ��� �÷��̾��� GetScore�Լ��� ȣ���� ��
            // �� ���̶� true�� ��ȯ�ϸ� �������� ó��
            if (curBakProgress >= 100)
                return true;
            else
                return false;
        }

        public void BakMissionCompleteCalled()
        {
            HashTable props = new HashTable();
            props.Add("BakMissionComplete", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public void BakMissionResetCalled()
        {
            HashTable props = new HashTable();
            props.Add("BakMissionReset", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }


        public void BakMissionComplete()
        {
            curBakProgress = 100;
            HashTable props = new HashTable();
            props.Add("IsBakMission", false);
            props.Add("IsSawing", false);
            props.Add("BakMissionComplete", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            bakUI.BakMissionComplete();
            BakMissionPanel.BakMissionComplete();
            bakMission.BakMissionComplete();
            progressText.text = "100 %";
        }

        public void BakMissionReset()
        {
            // ������Ƽ�� ó�� ���·� �ʱ�ȭ
            HashTable props = new HashTable();
            props.Add("IsBakMission", false);
            props.Add("IsSawing", false);
            props.Add("BakMissionReset", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            curBakProgress = 0;
            progressText.text = "0 %";
            bakUI.BakMissionReset();
            barUI.BakMissionReset();
            BakMissionPanel.BakMissionReset();
            bakMission.BakMissionReset();
        }
    }
}
