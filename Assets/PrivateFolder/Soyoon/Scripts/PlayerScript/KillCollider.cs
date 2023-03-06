using Saebom;
using SoYoon;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour
{
    private GameObject killButton;

    private void Awake()
    {
        // 변수 세팅
        Transform uiCanvas = GameObject.Find("UICanvas").transform;
        killButton = uiCanvas.GetChild(11).gameObject;
    }

    public void FoundKillTarget()
    {
        float killRangeRadius = transform.GetComponent<CircleCollider2D>().radius;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.parent.position, killRangeRadius);//, LayerMask.NameToLayer("KillRange"));
        List<Collider2D> targetList = new List<Collider2D>();
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].gameObject.name != "KillRangeCollider")
                continue;
            if (targets[i].transform.parent.name == PlayGameManager.Instance.myPlayerState.playerPrefab.name)
                continue;

            targetList.Add(targets[i]);
        }

        if (targetList.Count == 0)
        {
            killButton.GetComponent<KillButton>().target = null;
            return;
        }

        GameObject target = targetList[0].transform.parent.gameObject;
        float minDistance = (PlayGameManager.Instance.myPlayerState.playerPrefab.transform.position - target.transform.position).sqrMagnitude;
        for (int i = 1; i < targetList.Count; i++)
        {
            float distance = (PlayGameManager.Instance.myPlayerState.playerPrefab.transform.position - targetList[i].transform.parent.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                target = targetList[i].transform.parent.gameObject;
            }
        }
        killButton.GetComponent<KillButton>().target = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.parent.position, transform.GetComponent<CircleCollider2D>().radius);
    }
}
