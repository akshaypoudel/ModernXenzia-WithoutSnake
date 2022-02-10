using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SnakeGame;
using SnakeGame.ShopSystem;


public class ShopItemManager : MonoBehaviour
{                               

    private int availablejellyCount;
    public TMP_Text JellyUIText;
    public TMP_Text FruitsUIText;
    public ShopItemScriptableObjects[] shopItemSO;
    public GameObject[] shopPanelsGameObjects;
    public ShopTemplate[] shopTemplates;
    public string password;
    //private string fruits = "Fruits";
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

    public StageItemManager stageitemManager;
    public SkyItemManager skyItemManager;
    public SkinItemManager skinItemManager;

    public int indexOfBuyButton;

    //strings
    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    private string jellyPrefs = "Jelly";
    private string jellyEncrypted = "JellyEncrypted";

    void Start()
    {
        //Debug.Log("Fruits are: "+saveSystem.ReturnDecryptedScore(password,fruitsEncrypted,fruitsPrefs));
        GetCoins();
        //for(int i=0;i<shopPanelsGameObjects.Length;i++)
        //{
        //    shopPanelsGameObjects[i].SetActive(true);
        //}
        LoadPanel();
        CheckPurchasable();
    }
    public void GetCoins()
    {        
        availablejellyCount = saveSystem.ReturnDecryptedScore(password,jellyEncrypted,jellyPrefs);
    }



    public void LoadPanel()
    {
        for(int i=0; i < shopItemSO.Length; i++)
        {
            shopTemplates[i].TitleText.text = shopItemSO[i].Title;
            shopTemplates[i].DescriptionText.text = shopItemSO[i].Description;
            shopTemplates[i].bgSprite = shopItemSO[i].bgImage;
            LoadImageOfPanels(i);
        }
    }
    public void LoadImageOfPanels(int i)
    {
        shopPanelsGameObjects[i].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = shopItemSO[i].bgImage;
    }
    public void CheckPurchasable()
    {
        for(int i=0;i<shopPanelsGameObjects.Length;i++)
        {
            if(availablejellyCount>=shopItemSO[i].JellyCost)
            {
                shopPanelsGameObjects[i].transform.GetChild(indexOfBuyButton).gameObject.GetComponent<Button>().interactable=true;
            }
            else
            {
                shopPanelsGameObjects[i].transform.GetChild(indexOfBuyButton).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void PurchaseItem(int buttonNumber)
    {
        if(availablejellyCount >=shopItemSO[buttonNumber].JellyCost)
        {
            availablejellyCount = availablejellyCount - shopItemSO[buttonNumber].JellyCost;
            JellyUIText.text = availablejellyCount.ToString();
            int fruitsCountValue = saveSystem.ReturnDecryptedScore(password,fruitsEncrypted,fruitsPrefs);
            int totalFruits = fruitsCountValue + shopItemSO[buttonNumber].fruitsValue;
            FruitsUIText.text = totalFruits.ToString();
            int tempJelly = shopItemSO[buttonNumber].JellyCost;
            int tempFruits = shopItemSO[ buttonNumber].fruitsValue;
            saveSystem.EncryptPrefsPositive(tempFruits, password, fruitsEncrypted, fruitsPrefs);
            saveSystem.EncryptPrefsNegative(tempJelly, password,jellyEncrypted,jellyPrefs);
            CheckPurchasable();
            
            //checking for available coins to purchase in stage tab
            stageitemManager.GetCoins();
            stageitemManager.CheckPurchasable();

            //checking for available coins to purchase in sky tab
            skyItemManager.GetCoins();  
            skyItemManager.CheckPurchasable();
            
            //checking for available coins to purchase in sky tab
            skinItemManager.GetCoins();
            skinItemManager.CheckPurchasable();

            //Save(coins);
        }
    }
}
