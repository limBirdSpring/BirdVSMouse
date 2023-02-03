using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField]
    private float viewRadius;

    private LayerMask layer = LayerMask.NameToLayer("Wall");

    private void OnDrawGizmos()
    {
        
    }
}
