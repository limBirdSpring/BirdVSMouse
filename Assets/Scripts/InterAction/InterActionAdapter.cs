using Saebom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterActionAdapter : MonoBehaviour
{
    public UnityEvent OnInterAction = null;

//    [HideInInspector]
    public bool isActive = false;

    [SerializeField]
    private GameObject outLine = null;

    public void Interaction()
    {
        OnInterAction?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayGameManager.Instance.myPlayerState.playerPrefab)
            OutLineOn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayGameManager.Instance.myPlayerState.playerPrefab)
            OutLineOff();
    }

    public void OutLineOn()
    {
        if (outLine != null)
            outLine.SetActive(true);
    }


    public void OutLineOff()
    {
        if (outLine != null)
            outLine.SetActive(false);
    }
}