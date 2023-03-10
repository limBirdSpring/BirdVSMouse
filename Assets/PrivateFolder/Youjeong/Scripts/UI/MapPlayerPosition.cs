using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;

public class MapPlayerPosition : MonoBehaviour
{
    [SerializeField]
    private RectTransform mapPlayer;
    [SerializeField]
    private Vector3 zeroPoint;

    private Transform player;
    private Vector3 playerPosition;

    private void Awake()
    {
        player = PlayGameManager.Instance.myPlayerState.playerPrefab.transform;
    }

    private void OnEnable()
    {
        SetMapPlayerPosition();
    }

    private void FixedUpdate()
    {
        SetMapPlayerPosition();
    }
    private void SetMapPlayerPosition()
    {
        playerPosition = player.position + zeroPoint;
        mapPlayer.anchoredPosition = playerPosition * 950 / 120;
    }
}
