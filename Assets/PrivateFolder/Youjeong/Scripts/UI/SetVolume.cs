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

    [SerializeField]
    private Sprite mute;
    [SerializeField]
    private Sprite unmute;

    private SoundData soundData;
    
    private void Awake()
    {
        soundData = GameObject.FindObjectOfType<SoundData>();
    }

    private void OnEnable()
    {
        SetValue(soundData.allValue,allSound);
        SetValue(soundData.bgmValue,bgmSound);
        SetValue(soundData.sfxValue,sfxSound);
    }

    private void OnDisable()
    {
        soundData.allValue = allSound.value;
        soundData.bgmValue = bgmSound.value;
        soundData.sfxValue = sfxSound.value;
    }

    public void AllSoundVolume()
    {
        mixer.SetFloat("Master", Mathf.Log10(SetValue(allSound.value, allSound)) * 20);
    }

    public void BGMSoundVolume()
    {
        mixer.SetFloat("BGM", Mathf.Log10(SetValue(bgmSound.value, bgmSound)) * 20);
    }

    public void SFXSoundVolume()
    {
        mixer.SetFloat("SFX", Mathf.Log10(SetValue(sfxSound.value, sfxSound)) * 20);
    }

    private string type;
    private bool isClick;
    private Scrollbar sound;

    private void SetType(int num)
    {
        switch(num)
        {
            case 0:
                type = "Master";
                sound = allSound;
                break;
            case 1:
                type = "BGM";
                sound = bgmSound;
                break;
            case 2:
                type = "SFX";
                sound = sfxSound;
                break;
            default:
                break;
        }
        if (sound.value == 0)
            isClick = true;
        else
            isClick = false;
    }

    public void SoudMute(int num)
    {
        SetType(num);
        if (isClick)
        {
            sound.value = 1;
            mixer.SetFloat(type, Mathf.Log10(SetValue(1,sound)) * 20);
        }
        else
        {
            sound.value = 0;
            mixer.SetFloat(type, Mathf.Log10(SetValue(0,sound)) * 20);
        }
    }

    private float SetValue(float value,Scrollbar sound)
    {
        if (value == 0)
        {
            sound.transform.GetChild(1).GetComponent<Image>().sprite = mute;
            sound.value = value;
            return 0.0001f;
        }
        sound.transform.GetChild(1).GetComponent<Image>().sprite = unmute;
        sound.value = value;
        return value;
    }

    public void GameOver()
    {
        Application.Quit();
    }
}
