using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SoYoon
{
    public class BakMissionManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private BakMission bakMission;

        private float curBakProgress;
        public static BakMissionManager Instance { get; private set; }

        private bool updateBakProgress;
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
            updateBakProgress = false;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            // 바뀌는 프로퍼티들에 따라 다른 동작 진행
            if (changedProps["isBakMission"] != null)
            {
                if ((bool)changedProps["isBakMission"])
                {
                    Debug.Log("박 타는 사람 추가");
                    updateBakProgress = true;
                    curBakPlayerList.AddLast(targetPlayer);
                    bakMission.UpdatePercentage();
                }
                else
                {
                    Debug.Log("박 타는 사람 감소");
                    curBakPlayerList.Remove(targetPlayer);
                    if(curBakPlayerList.Count == 0)
                        updateBakProgress = false;
                    bakMission.UpdatePercentage();
                }
                CurBakPlayerCount = curBakPlayerList.Count;
            }
        }

        private void Update()
        {
            if (!updateBakProgress)
                return;

            // 여기서 bakMission.UpdateProgress();
            foreach(Player player in curBakPlayerList)
            {

            }
        }


    }
}
