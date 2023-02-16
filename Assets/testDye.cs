using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDye : MonoBehaviour
{
    [Header("Light Partucle")]
    [SerializeField]
    private LightParticle lightParticle;

    public void PlayEffect()
    { 
        lightParticle.PlayParticle();
    }
}
