using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Saebom
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData", order = int.MaxValue)]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField]
        private string playerName;

        [SerializeField]
        private bool isBirdTeam;

        //플레이어 프리팹 넣기
        [SerializeField]
        private GameObject player;

    }
}
