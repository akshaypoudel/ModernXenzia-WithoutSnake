using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SnakeGame;
using SnakeGame.ShopSystem;

public class LoadImportantData : MonoBehaviour
{
    public TMP_Text fruitsCount;
    public TMP_Text jellyCount;
    // public GameObject adbutton;
    public TMP_Text fruitstext;
    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    private string jellyPrefs = "Jelly";
    public int welcomeBonusFruitCount;
    private string jellyEncrypted = "JellyEncrypted";
    private string bonusClaimedPrefs = "isBonusClaimed";
    //private string canWatchAd = "WatchAD";
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();
    public GameObject WelcomeBonusWindow;
    public GameObject welcomeBonusButton;

    public DisableBGComponents bgDisable;
    public StageItemManager stageItemManager;
    public SkyItemManager skyItemManager;
    public SkinItemManager skinItemManager;

    [SerializeField] private string password;
    private void Start()
    {
            saveSystem.DecryptPrefs(fruitsCount, password, fruitsEncrypted, fruitsPrefs);
            saveSystem.DecryptPrefs(jellyCount, password, jellyEncrypted, jellyPrefs);
        if (PlayerPrefs.HasKey(bonusClaimedPrefs))
            welcomeBonusButton.SetActive(false);
        /*z  
                if(PlayerPrefs.HasKey(canWatchAd))
                {
                    adbutton.SetActive(true);

                }
                else
                {
                    adbutton.SetActive(false);
                }

                */
    }
    public void WelcomeBonusClaim()
    {
        PlayerPrefs.SetInt(bonusClaimedPrefs, 1);
        welcomeBonusButton.SetActive(false);
        WelcomeBonusWindow.SetActive(true);
        GiveFruitsToUser(welcomeBonusFruitCount);
    }
    private void GiveFruitsToUser(int fruitCount)
    {
        int totalFruits = saveSystem.ReturnDecryptedScore(password, fruitsEncrypted, fruitsPrefs);
        int fruitsToGive = fruitCount;
        int totalFruitAfterRewarding = fruitsToGive + totalFruits;
        fruitstext.text = totalFruitAfterRewarding.ToString();
        fruitstext.text = totalFruitAfterRewarding.ToString();

        bgDisable.DisableAllBgComponents();
        //if (fruitCount < 100)
        //     FruitsGainedWindow.SetActive(true);
        saveSystem.EncryptPrefsPositive(fruitsToGive, password, fruitsEncrypted, fruitsPrefs);

        RefreshTheCoinAmount();

        //if (fruitCount < 100)
        //    this.RequestFruitRewardedAD();

    }

    private void RefreshTheCoinAmount()
    {
        stageItemManager.GetCoins();
        stageItemManager.CheckPurchasable();

        skyItemManager.GetCoins();
        skyItemManager.CheckPurchasable();

        skinItemManager.GetCoins();
        skinItemManager.CheckPurchasable();
    }
}
