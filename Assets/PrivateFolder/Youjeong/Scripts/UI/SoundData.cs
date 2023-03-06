using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundData : MonoBehaviour
{
    public float allValue = 1;
    public float bgmValue = 1;
    public float sfxValue = 1;

    public static SoundData Instance { get; private set; }
    private void Awake()
    {
        if (GameObject.Find("DataManager") != null)
        {
            SoundData[] data = GameObject.FindObjectsOfType<SoundData>();
            if (data.Length == 1)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(this.gameObject);
        }
    }
}
