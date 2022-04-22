using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using SnakeGame.ShopSystem;
using SnakeGame;
using TMPro;

public class AdManager : MonoBehaviour
{
    [HideInInspector]public InterstitialAd interstitial;
    [HideInInspector]public RewardedAd rewardedAd;
    [HideInInspector] public RewardedAd rewardedAdForFruits;
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();
    public TMP_Text fruitstext;
    public int fruitRewardAfterWatchingAD;
    public int welcomeBonusFruitCount;
    private string bonusClaimedPrefs = "isBonusClaimed";

    public GameObject AdButton;
    public GameObject FruitsGainedWindow;
    public GameObject WelcomeBonusWindow;
    public GameObject welcomeBonusButton;

    public DisableBGComponents bgDisable;
    public StageItemManager stageItemManager;
    public SkyItemManager skyItemManager;
    public SkinItemManager skinItemManager;



    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    [SerializeField]private string password;


   /* private string interestialAdId = "ca-app-pub-6957919137987763/9391523296";
    private string rewardAdIdForLives = "ca-app-pub-6957919137987763/7352339859";
    private string rewardAdIdForFruits = "ca-app-pub-6957919137987763/3800285260";*/

    private string interestialAdId = "	ca-app-pub-3940256099942544/1033173712";
    private string rewardAdIdForLives = "ca-app-pub-3940256099942544/5224354917";
    private string rewardAdIdForFruits = "ca-app-pub-3940256099942544/5224354917";

    public bool isMenuScene;
    public static AdManager instance;
    public bool canLoadBanner;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        MobileAds.Initialize(HandleInitCompleteAction);
        if(isMenuScene)
        {
            RequestFruitRewardedAD();
            if(PlayerPrefs.HasKey(bonusClaimedPrefs))
                welcomeBonusButton.SetActive(false);
        }
    }
    public void RequestFruitRewardedAD()
    {
            this.RequestRewardedForFruits();
            this.rewardedAdForFruits.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            this.rewardedAdForFruits.OnAdLoaded += HandleOnAdLoaded;
            this.rewardedAdForFruits.OnAdClosed += HandleOnAdClosed;
            this.rewardedAdForFruits.OnUserEarnedReward += HandleOnUserEarnedReward;

    }

    private void HandleOnUserEarnedReward(object sender, Reward e)
    {
        GiveFruitsToUser(fruitRewardAfterWatchingAD);
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        
    }

    private void HandleOnAdLoaded(object sender, EventArgs e)
    {

    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        AdButton.SetActive(false);
    }
    


    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            this.RequestInterstitial();
            this.RequestRewardedAd();
        });
    }
    public void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(interestialAdId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void RequestRewardedAd()
    {
        this.rewardedAd = new RewardedAd(rewardAdIdForLives);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    public void RequestRewardedForFruits()
    {
        this.rewardedAdForFruits = new RewardedAd(rewardAdIdForFruits);
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAdForFruits.LoadAd(request);
    }

    public void ShowRewardedForFruits()
    {
        //RequestFruitRewardedAD();
        if (this.rewardedAdForFruits.IsLoaded())
            this.rewardedAdForFruits.Show();
        //else
          //  AdButton.SetActive(false);
    }

    private void GiveFruitsToUser(int fruitCount)
    {
        int totalFruits = saveSystem.ReturnDecryptedScore(password,fruitsEncrypted,fruitsPrefs);
        int fruitsToGive = fruitCount;
        int totalFruitAfterRewarding = fruitsToGive + totalFruits;
        fruitstext.text = totalFruitAfterRewarding.ToString();
        fruitstext.text = totalFruitAfterRewarding.ToString();

        bgDisable.DisableAllBgComponents();
        if(fruitCount < 100)
            FruitsGainedWindow.SetActive(true);
        saveSystem.EncryptPrefsPositive(fruitsToGive,password,fruitsEncrypted,fruitsPrefs);

        stageItemManager.GetCoins();
        stageItemManager.CheckPurchasable();

        skyItemManager.GetCoins();
        skyItemManager.CheckPurchasable();

        skinItemManager.GetCoins();
        skinItemManager.CheckPurchasable();

        if(fruitCount < 100)
            this.RequestFruitRewardedAD();
        
    }
    public void SaveAdButtonState()
    {
        PlayerPrefs.SetInt("WatchAD", 1);
    }
    public void WelcomeBonusClaim()
    {
        PlayerPrefs.SetInt(bonusClaimedPrefs, 1);
        welcomeBonusButton.SetActive(false);
        WelcomeBonusWindow.SetActive(true);
        GiveFruitsToUser(welcomeBonusFruitCount);
    }
    
    
}
