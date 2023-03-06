using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tutorials;

    private int num=0;

    private void OnEnable()
    {
        foreach(GameObject obj in tutorials)
        {
            obj.SetActive(false);
        }

        num = 0;
        tutorials[0].SetActive(true);
    }

    public void OnRightButtonClick()
    {
        if (num == tutorials.Count-1)
        {
            return;
        }

        tutorials[num].SetActive(false);

        num++;
        tutorials[num].SetActive(true);
    }

    public void OnLeftButtonClick()
    {
        if (num == 0)
        {
            return;
        }

        tutorials[num].SetActive(false);

        num--;
        tutorials[num].SetActive(true);
    }
}
