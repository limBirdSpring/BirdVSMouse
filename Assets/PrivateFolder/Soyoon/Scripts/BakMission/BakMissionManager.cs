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
            // 바뀌는 프로퍼티들에 따라 다른 동작 진행
            if (changedProps["IsBakMission"] != null)
            {
                if ((bool)changedProps["IsBakMission"])
                {
                    Debug.Log("박 타는 사람 추가");
                    curBakPlayerList.AddLast(targetPlayer);
                    curBakPlayerCount = curBakPlayerList.Count;
                    barUI.ChangedPlayerCount();
                    bakMission.UpdatePercentage();
                }
                else
                {
                    Debug.Log("박 타는 사람 감소");
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
            //if (Input.GetKeyDown(KeyCode.F3)) // 나중에 없애기
            //    BakMissionResetCalled(); // 나중에 리셋할 경우 불릴 함수
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
                    // 실제로 움직이고 있는 경우
                    curBakProgress += bakMission.UpdateProgress();
                }
            }

            progressText.text = string.Format("{0} %", (int)curBakProgress);

            if (curBakProgress >= 100)
            {
                // 미션을 완료한 경우
                BakMissionComplete();
            }
            Debug.Log(CurBakProgress);
        }

        public override bool GetScore()
        {
            // 최종으로 확인할 경우에는 모든 플레이어의 GetScore함수를 호출한 뒤
            // 한 명이라도 true를 반환하면 성공으로 처리
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
            // 프로퍼티를 처음 상태로 초기화
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
