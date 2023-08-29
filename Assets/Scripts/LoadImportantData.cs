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
    public int welcomeBonusFruitCount;
    private string jellyEncrypted = "JellyEncrypted";
    private string bonusClaimedPrefs = "isBonusClaimed";
    private string freeGiftClaimPrefs = "isFreeGiftClaimed";
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();
    public GameObject WelcomeBonusWindow;
    public GameObject welcomeBonusButton;
    public GameObject freeGiftClaimButton;
    public GameObject freeGiftClaimWindow;

    public DisableBGComponents bgDisable;
    public StageItemManager stageItemManager;
    public SkyItemManager skyItemManager;
    public SkinItemManager skinItemManager;

    [SerializeField] private string password;
    private void Start()
    {
        saveSystem.DecryptPrefs(fruitsCount, fruitsEncrypted);
        saveSystem.DecryptPrefs(jellyCount, jellyEncrypted);

        if (PlayerPrefs.HasKey(bonusClaimedPrefs))
            welcomeBonusButton.SetActive(false);
        if(PlayerPrefs.HasKey(freeGiftClaimPrefs))
        {
            freeGiftClaimButton.SetActive(false);
        }
    }
    public void WelcomeBonusClaim()
    {
        PlayerPrefs.SetInt(bonusClaimedPrefs, 1);
        welcomeBonusButton.SetActive(false);
        WelcomeBonusWindow.SetActive(true);
        GiveFruitsToUser(welcomeBonusFruitCount);
    }
    public void FreeGiftClaim()
    {
        PlayerPrefs.SetInt(freeGiftClaimPrefs,1);
        freeGiftClaimButton.SetActive(false);
        freeGiftClaimWindow.SetActive(true);
        bgDisable.DisableAllBgComponents();
    }
    private void GiveFruitsToUser(int fruitCount)
    {
        int totalFruits = saveSystem.ReturnDecryptedScore( fruitsEncrypted);
        int fruitsToGive = fruitCount;
        int totalFruitAfterRewarding = fruitsToGive + totalFruits;
        fruitstext.text = totalFruitAfterRewarding.ToString();
        fruitstext.text = totalFruitAfterRewarding.ToString();

        bgDisable.DisableAllBgComponents();
        saveSystem.EncryptPrefsPositive(fruitsToGive, fruitsEncrypted);
        RefreshTheCoinAmount();
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
