using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private Scrollbar allSound;
    [SerializeField]
    private Scrollbar bgmSound;
    [SerializeField]
    private Scrollbar sfxSound;

    public void AllSoundVolume()
    {
        mixer.SetFloat("Master", Mathf.Log10(SetValue(allSound.value)) * 20);
    }

    public void BGMSoundVolume()
    {
        mixer.SetFloat("BGM", Mathf.Log10(SetValue(bgmSound.value)) * 20);
    }

    public void SFXSoundVolume()
    {
        mixer.SetFloat("SFX", Mathf.Log10(SetValue(sfxSound.value)) * 20);
    }

    private float SetValue(float value)
    {
        if (value == 0)
            return 0.0001f;
        return value;
    }

    public void GameOver()
    {
        Application.Quit();
    }
}
