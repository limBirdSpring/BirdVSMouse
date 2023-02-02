using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nulttuigi : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;

    public bool active;

    public void Jump()
    {
        active = true;
        cam.Priority = 30;
        StartCoroutine(JumpCor());
    }

    private IEnumerator JumpCor()
    {
        yield return new WaitForSeconds(2f);
        cam.Priority = 1;
        active = false;
    }
 
}
