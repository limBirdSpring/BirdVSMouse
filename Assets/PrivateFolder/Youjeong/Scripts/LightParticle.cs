using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lights;

    private Coroutine playLights;

    public void PlayParticle()
    {
        playLights=StartCoroutine(PlayLightParticle());
    }

    private IEnumerator PlayLightParticle()
    {
        int i = 0;
        while(true)
        {
            lights[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            lights[i].SetActive(false);
            i++;
            if(i>lights.Length)
            {
                StopCoroutine(playLights);
                break;
            }
        }
    }
}
