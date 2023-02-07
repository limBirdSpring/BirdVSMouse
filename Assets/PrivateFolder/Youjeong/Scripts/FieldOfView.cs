using SoYoon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private LayerMask wallLayer;
    private LayerMask shadowLayer;
    private LayerMask playerLayer;
    private LayerMask targetMask;
    private Collider2D[] targets;

    [SerializeField]
    private Vector2 size;

    private void Awake()
    {
        //wallLayer = LayerMask.NameToLayer("Wall");
        shadowLayer = LayerMask.NameToLayer("Shadow");
        playerLayer = LayerMask.NameToLayer("Player");
        targetMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Shadow");
        
    }
    private void FixedUpdate()
    {
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

            if(Physics2D.Raycast(transform.position, dirToTarget, disToTarget, wallLayer))
            {
                if (targets[i].gameObject == this.gameObject)
                    return;
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

    private void SetTargetLayer(GameObject target,LayerMask layer)
    {
        target.layer = layer;
        foreach (Transform child in target.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;

            if (child.gameObject.name == "KillRangeCollider")
                child.gameObject.layer = LayerMask.NameToLayer("KillRange");
        }
    }
}
