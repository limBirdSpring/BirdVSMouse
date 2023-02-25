using Photon.Pun;
using SoYoon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviourPun
{
    [SerializeField]
    private LayerMask wallLayer;
    private LayerMask shadowLayer;
    private LayerMask playerLayer;
    private LayerMask targetMask;
    private Collider2D[] targets;

    [SerializeField]
    private Vector2 size;

    private CullingMaskController cullingMask;

    private void Awake()
    {
        cullingMask = Camera.main.GetComponent<CullingMaskController>();

        //wallLayer = LayerMask.NameToLayer("Wall");
        shadowLayer = LayerMask.NameToLayer("Shadow");
        playerLayer = LayerMask.NameToLayer("Player");
        targetMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Shadow");
        
    }
    private void FixedUpdate()
    {
        if(photonView.IsMine)
            FindTarget();
    }

    private void FindTarget()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y);
        targets = Physics2D.OverlapBoxAll(center, size, 0, targetMask);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;
            float disToTarget = Vector3.Distance(transform.position, targets[i].transform.position);

            if (Physics2D.Raycast(transform.position, dirToTarget, disToTarget, wallLayer))
            {
                // 타겟 사이에 벽이 있으면 안보이게 처리
                if (targets[i].gameObject == this.gameObject)
                {
                    SetTargetLayer(targets[i].gameObject, targets[i].gameObject.layer);
                    return;
                }
                SetTargetLayer(targets[i].gameObject, shadowLayer);
            }
            else
            {
                SetTargetLayer(targets[i].gameObject, playerLayer);
            }

            Debug.DrawRay(transform.position, dirToTarget * disToTarget, Color.red);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void SetTargetLayer(GameObject target, LayerMask layer)
    {
        target.layer = layer;
        foreach (Transform child in target.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;

            if (child.gameObject.name == "KillRangeCollider")
            {
                child.gameObject.layer = LayerMask.NameToLayer("KillRange");

                if (layer == shadowLayer) // 시야에 보이지 않을 경우 killRange꺼주기
                    child.gameObject.SetActive(false);
                else
                    child.gameObject.SetActive(true);
            }

            if (child.gameObject.name == "CorpseRange")
            {
                child.gameObject.layer = LayerMask.NameToLayer("CorpseRange");

                if (layer == shadowLayer) // 시야에 보이지 않을 경우 CorpseRange꺼주기
                    child.gameObject.SetActive(false);
                else
                    child.gameObject.SetActive(true);
            }
        }
    }

    public void InNulttuigi()
    {
        if(photonView.IsMine)
            cullingMask.OnLayerMask(shadowLayer);
    }
    public void OutNulttuigi()
    {
        if (photonView.IsMine)
            cullingMask.OffLayerMask(shadowLayer);
    }
}
