using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum UISFXName
{
    Start,
    Stop,
    Ending,
    GetJob,
    VoteDie,
    Click
}

[Serializable]
public struct UISound
{
    public UISFXName uiName;
    public AudioClip clip;
}

public class SoundManager : SingleTon<SoundManager>
{
    private AudioSource audio;

    [SerializeField]
    private List<UISound> uiSounds = new List<UISound>();


    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }


    public void PlayUISound(UISFXName name)
    {
        foreach(UISound sound in uiSounds)
        {
            if (sound.uiName == name)
            {
                audio.clip = sound.clip;
            }
        }

        audio.Play();
    }


}
