using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PhoneVibrator;
using SnakeGame;


public class SettingsMenu : MonoBehaviour
{
    #region References
    public Slider MusicSlider;
    public Slider SFXSlider;

    public GameObject vibrationDropDownMenu;
    private GameObject BGAudioGameObject;
    //public GameObject Controls;

    public AudioSource BackGroundAudioSource;
    public AudioSource[] SoundEffects;

    //public List<GameObject> SelectedButtons;

    public bool isGameMode;
    //public Auio listner;

    private string vibrationOnPref = "VibrationON";
    private string bgAudioPref = "BackGroundAudio";
    private string sfxAudioPref = "SFXAudio";

    //private string canTouch = "canTouch";

    #endregion
    private void Awake()
    {
        if (BackGroundAudioSource == null)
        {
            BGAudioGameObject = GameObject.FindWithTag("Audio");
            BackGroundAudioSource = BGAudioGameObject.GetComponent<AudioSource>();
        }
        CheckVibrationSettingAndLoad();
        CheckBackGroundAudioAndLoad();
        CheckSFXAudioAndLoad();
    }

    private void Start() {

        //CheckTouchControls();
        
    }

    private void CheckVibrationSettingAndLoad()
    {
        if (!PlayerPrefs.HasKey(vibrationOnPref))
        {
            PlayerPrefs.SetInt(vibrationOnPref, 0);
            VibrationSettingsLoad();
        }
        else
        {
            VibrationSettingsLoad();
        }
    }
    private void CheckBackGroundAudioAndLoad()
    {
        if (!PlayerPrefs.HasKey(bgAudioPref))
        {
            PlayerPrefs.SetFloat(bgAudioPref, 1);
            LoadBackGroundAudio();
        }
        else
        {
            LoadBackGroundAudio();
        }

    }
    private void CheckSFXAudioAndLoad()
    {
        if (!PlayerPrefs.HasKey(sfxAudioPref))
        {
            PlayerPrefs.SetFloat(sfxAudioPref, 1);
            LoadSFXAudio();
        }
        else
        {
            LoadSFXAudio();
        }
    }


    #region VibrationSettings
    public void SetVibration(int vibrationIndex)
    {
        if(vibrationIndex == 1)
            Vibrator.canVibrate = false;    
        else if (vibrationIndex ==0)
            Vibrator.canVibrate = true;

        VibrationSettingsSave(vibrationIndex);
    }
    public void VibrationSettingsSave(int index)
    {
        PlayerPrefs.SetInt(vibrationOnPref,index);
    }
    private void VibrationSettingsLoad()
    {
        vibrationDropDownMenu.GetComponent<Dropdown>().value=PlayerPrefs.GetInt(vibrationOnPref);    
        if(PlayerPrefs.GetInt(vibrationOnPref) == 0)
        {
            Vibrator.canVibrate=true;
        }
        else
        {
            Vibrator.canVibrate = false;
        }
    }

    #endregion

    #region BackGroundAudios
    public void SetBackGroundAudio()
    {
        BackGroundAudioSource.volume = MusicSlider.value;
        SaveBackGroundAudio();
    }
    public void SaveBackGroundAudio()
    {
        PlayerPrefs.SetFloat(bgAudioPref, MusicSlider.value);
    }
    private void LoadBackGroundAudio()
    {
        MusicSlider.value = PlayerPrefs.GetFloat(bgAudioPref);
        BackGroundAudioSource.volume = PlayerPrefs.GetFloat(bgAudioPref);
    }

    #endregion

    #region SFXAudio
    public void SetSFXAudio()
    {
        if(!isGameMode)
        {
            for (int i = 0; i < SoundEffects.Length; i++)
            {
                SoundEffects[i].volume = SFXSlider.value;
                
            }
            SaveSFXAudio();
        }
        else
        {
            SoundManager.volume = SFXSlider.value;
            SaveSFXAudio();
        }

    }
    public void SaveSFXAudio()
    {
        PlayerPrefs.SetFloat(sfxAudioPref, SFXSlider.value);
    }
    public void LoadSFXAudio()
    {
        if(!isGameMode)
        {
            for (int i = 0; i < SoundEffects.Length; i++)
            {
                SoundEffects[i].volume = SFXSlider.value;
            }
            SFXSlider.value = PlayerPrefs.GetFloat(sfxAudioPref);
        }
        else
        {
            SFXSlider.value = PlayerPrefs.GetFloat(sfxAudioPref);
            SoundManager.volume = SFXSlider.value;
        }
    }

    #endregion

}
