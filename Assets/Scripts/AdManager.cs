using UnityEngine;
using UnityEngine.Advertisements;
using System;
using SnakeGame.ShopSystem;
using SnakeGame;
using TMPro;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    public InterstitialAds interstitialAd;
    public RewardedAds rewardedAd;
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();


    [SerializeField]private string password;
    public bool isMenuScene;
    private bool _isTestMode = true;

    [Header("---------Menu Scene Only Data---------\n")]

    public TMP_Text fruitstext;
    public int fruitRewardAfterWatchingAD;

    public GameObject AdButton;
    public GameObject FruitsGainedWindow;

    public DisableBGComponents bgDisable;
    public StageItemManager stageItemManager;
    public SkyItemManager skyItemManager;
    public SkinItemManager skinItemManager;

    private string _gameId = "4752355";
    
    private bool canInitializeAds;

    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";

    private string interestialAdId = "Interstitial_Android";
    private string rewardAdId = "Rewarded_Android";


    void Awake()
    {
        _isTestMode = true;
        InitializeAds();
        if (canInitializeAds)
        {
            rewardedAd.InitializeRewarded();
            interstitialAd.InitializeInterstitial();
        }
    }
    private void InitializeAds()
    {
        if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.WindowsEditor)
        {
            canInitializeAds = true;
            Advertisement.Initialize(_gameId, false, this);
        }
        else
            canInitializeAds = false;
    }

    public void GiveFruitsToUser()
    {
        int existingFruits = saveSystem.ReturnDecryptedScore(fruitsEncrypted);
        int fruitsToGive = fruitRewardAfterWatchingAD;
        int totalFruitAfterRewarding = fruitsToGive + existingFruits;
        fruitstext.text = totalFruitAfterRewarding.ToString();

        bgDisable.DisableAllBgComponents();
        FruitsGainedWindow.SetActive(true);
        saveSystem.EncryptPrefsPositive(fruitsToGive,fruitsEncrypted);

        stageItemManager.GetCoins();
        stageItemManager.CheckPurchasable();

        skyItemManager.GetCoins();
        skyItemManager.CheckPurchasable();

        skinItemManager.GetCoins();
        skinItemManager.CheckPurchasable();

        
    }
    public void OnInitializationComplete()
    {
        rewardedAd.LoadAd();
        interstitialAd.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        AdButton.SetActive(false);
    }
}
