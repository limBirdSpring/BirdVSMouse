using SoYoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMailButton : MonoBehaviour
{
    [SerializeField]
    private GameObject newCanvas;

    private void Start()
    {
        if(DataManager.Instance.myInfo.mailedItem.Count != 0)
            newCanvas.SetActive(true);
    }
}
