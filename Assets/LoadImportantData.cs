using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SnakeGame;

public class LoadImportantData : MonoBehaviour
{
    public TMP_Text fruitsCount;
    public TMP_Text jellyCount;
    public GameObject adbutton;
    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    private string jellyPrefs = "Jelly";
    private string jellyEncrypted = "JellyEncrypted";
    private string canWatchAd = "WatchAD";
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

    [SerializeField] private string password;
    private void Start()
    {
            saveSystem.DecryptPrefs(fruitsCount, password, fruitsEncrypted, fruitsPrefs);
            saveSystem.DecryptPrefs(jellyCount, password, jellyEncrypted, jellyPrefs);

        if(PlayerPrefs.HasKey(canWatchAd))
        {
            adbutton.SetActive(true);

        }
        else
        {
            adbutton.SetActive(false);
        }
            

    }
}
