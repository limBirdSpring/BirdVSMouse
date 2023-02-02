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

        //�÷��̾� ������ �ֱ�
        [SerializeField]
        private GameObject player;

    }
}
