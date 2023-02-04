using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCoroutine : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(Cor());
    }

    private IEnumerator Cor()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
