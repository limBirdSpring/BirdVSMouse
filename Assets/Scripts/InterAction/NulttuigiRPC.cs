using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulttuigiRPC : MonoBehaviour
{
    [PunRPC]
    public void NulttuigiActiveUpdate(bool active)
    {
        gameObject.GetComponent<InterActionAdapter>().isActive = active;
    }
}
