using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nulttuigi : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;


    public void Jump()
    {
        gameObject.GetComponent<InterActionAdapter>().isActive = true;
        cam.Priority = 30;
        StartCoroutine(JumpCor());
    }

    private IEnumerator JumpCor()
    {
        yield return new WaitForSeconds(2f);
        cam.Priority = 1;
        gameObject.GetComponent<InterActionAdapter>().isActive = false;
    }
 
}
