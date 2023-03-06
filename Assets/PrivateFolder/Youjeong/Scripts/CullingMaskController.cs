using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingMaskController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void Start()
    {
        OffLayerMask(LayerMask.NameToLayer("Shadow"));
        OffLayerMask(LayerMask.NameToLayer("InActive"));
        OffLayerMask(LayerMask.NameToLayer("Ghost"));
    }
    public void OffLayerMask(LayerMask layer)
    {
        cam.cullingMask = cam.cullingMask & ~(1 << layer);
    }

    public void OnLayerMask(LayerMask layer)
    {
        cam.cullingMask |= 1 << layer;
    }
}
