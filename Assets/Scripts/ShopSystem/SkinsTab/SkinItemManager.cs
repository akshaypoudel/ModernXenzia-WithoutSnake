using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityCipher;
using SnakeGame;
using SnakeGame.ShopSystem;

public class SkinItemManager : MonoBehaviour
{
    private int coins;
    public TMP_Text coinUI;
    //public bool isSkinTab;
    public SkinItemSO[] skinItemSO;
    public GameObject[] skinPanelsGameObjects;
    public SkinTemplate[] skinTemplates;
    public Sprite[] skinBGImage;
    private List<GameObject> PurchaseButton;
    private List<GameObject> SelectButtons;
    private List<GameObject> SelectedButtons;
    private List<GameObject> FruitsIcon;

    public int buttonIndexInStageTemplateGameObject;

    [SerializeField] private string password;
    [SerializeField] private string passwordforsavefile;

    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

    private string jsonFileName = "CMD_LOG-CONTROL_754a7.json";
    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
   // private string isSaved = "isSkinSaved";
    private string indexOfMat = "indexOfSkins1";
    private string persistantDataPath = "";

    public SkyItemManager skyItemManager;
    public StageItemManager stageItemManager;


    private SaveLoadJSONData saveSystemWithJson;
    PlayerDataNumber playerDataNumber;


    private void Awake()
    {
        saveSystemWithJson = new SaveLoadJSONData();
        playerDataNumber = new PlayerDataNumber();
        persistantDataPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }
    void Start()
    {
        GetCoins();
        AssignButtons();
        for (int i = 0; i < skinPanelsGameObjects.Length; i++)
        {
            skinPanelsGameObjects[i].SetActive(true);
        }
        LoadPanel();
        CheckPurchasable();
        if (File.Exists(persistantDataPath + jsonFileName))
        {
            LoadButtonState();
        }


        if (PlayerPrefs.HasKey(indexOfMat))
            SetSelectedButtonActive();
        else
            PlayerPrefs.SetInt(indexOfMat, 0);
    }
    public void GetCoins()
    {
        coins = saveSystem.ReturnDecryptedScore(password, fruitsEncrypted, fruitsPrefs);
    }

    public void AssignButtons()
    {
        if (PurchaseButton == null) PurchaseButton = new List<GameObject>();
        if (SelectButtons == null) SelectButtons = new List<GameObject>();
        if (SelectedButtons == null) SelectedButtons = new List<GameObject>();
        if(FruitsIcon == null) FruitsIcon = new List<GameObject>();

        for (int i = 0; i < skinPanelsGameObjects.Length; i++)
        {
            PurchaseButton.Add(skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject);
            SelectButtons.Add(skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject + 1).gameObject);
            SelectedButtons.Add(skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject + 2).gameObject);
            FruitsIcon.Add(skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject + 3).GetChild(0).gameObject);
        }
    }
    public void LoadPanel()
    {
        for (int i = 0; i < skinItemSO.Length; i++)
        {
            skinTemplates[i].TitleText.text = skinItemSO[i].Price;
            skinTemplates[i].BGSprite = skinBGImage[i];
            skinTemplates[i].NameOfStage = skinItemSO[i].name;
            LoadImageOfPanels(i);
        }
    }
    public void LoadImageOfPanels(int i)
    {
        skinPanelsGameObjects[i].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = skinTemplates[i].BGSprite;
    }
    public void CheckPurchasable()
    {
        for (int i = 0; i < skinPanelsGameObjects.Length; i++)
        {
            if (coins >= skinItemSO[i].baseCost)
            {
                skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                skinPanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void PurchaseItem(int buttonNumber)
    {
        if (coins >= skinItemSO[buttonNumber].baseCost)
        {
            skinPanelsGameObjects[buttonNumber].transform.GetChild(3).GetChild(0).gameObject.SetActive(false);

            coins = coins - skinItemSO[buttonNumber].baseCost;
            coinUI.text = coins.ToString();
            int tempCoin = skinItemSO[buttonNumber].baseCost;
            saveSystem.EncryptPrefsNegative(tempCoin, password, fruitsEncrypted, fruitsPrefs);
            CheckPurchasable();
            skyItemManager.GetCoins();
            skyItemManager.CheckPurchasable();
            stageItemManager.GetCoins();
            stageItemManager.CheckPurchasable();


            Select(buttonNumber);
        }
    }
    private void SetSelectedButtonActive()
    {
        for (int i = 0; i < SelectedButtons.Count; i++)
        {
            SelectedButtons[i].SetActive(false);
        }
        SelectedButtons[PlayerPrefs.GetInt(indexOfMat)].SetActive(true);
    }
    public void Selected(int buttonNumber)
    {
        if (SelectButtons[buttonNumber].activeSelf)
        {
            for (int i = 0; i < SelectedButtons.Count; i++)
            {
                SelectedButtons[i].SetActive(false);
            }
            SelectedButtons[buttonNumber].SetActive(true);
            PlayerPrefs.SetInt(indexOfMat, buttonNumber);
            

        }
    }
    public void Select(int buttonNumber)
    {
        bool isActive = true;
        bool isNotActive = false;
        PurchaseButton[buttonNumber].SetActive(isNotActive);
        SelectButtons[buttonNumber].SetActive(isActive);
        skinTemplates[buttonNumber].UnlockedObjectText.text = skinItemSO[buttonNumber].Name;
        FruitsIcon[buttonNumber].SetActive(isNotActive);
        SaveButtonState(buttonNumber, skinItemSO[buttonNumber].Name, isActive, isNotActive);
    }
    public void SaveButtonState(int buttonNumber, string nameOfObject, bool isActive, bool isNotActive)
    {
        PlayerData playerdata = new PlayerData();
        playerdata.buttonNumber = buttonNumber;
        playerdata.dataToSetActive = isActive;
        playerdata.dataToSetInactive = isNotActive;
        playerdata.removeFruitIcon = isNotActive;
        playerdata.nameOfUnlockedObject = nameOfObject;
        SaveEncryptedData(playerdata);

       // playerDataNumber.playerDataList.Add(playerdata);

        //saveSystemWithJson.SavePlayerDataNumber(playerDataNumber, jsonFileName);
    }
    private void SaveEncryptedData(PlayerData playerdata)
    {
        PlayerDataEncrypted playerdataencrypt = new PlayerDataEncrypted();
        string encrypt1 = RijndaelEncryption.Encrypt(playerdata.buttonNumber.ToString(), passwordforsavefile);
        string encrypt4 = RijndaelEncryption.Encrypt(playerdata.nameOfUnlockedObject, passwordforsavefile);


        playerdataencrypt.No = encrypt1;
        playerdataencrypt.yieldd = playerdata.dataToSetActive;
        playerdataencrypt.NetworkBuild = playerdata.dataToSetInactive;
        playerdataencrypt.TIO00_UGG = playerdata.dataToSetInactive;
        playerdataencrypt.name = encrypt4;
        playerDataNumber.NetworkingCommand.Add(playerdataencrypt);
        saveSystemWithJson.SavePlayerDataNumber(playerDataNumber, jsonFileName);

    }
    public void LoadButtonState()
    {
        PlayerDataEncrypted playerdata = new PlayerDataEncrypted();

        playerDataNumber = saveSystemWithJson.LoadPlayerDataNumber(jsonFileName);


        for (int i = 0; i < playerDataNumber.NetworkingCommand.Count; i++)
        {
            playerdata = playerDataNumber.NetworkingCommand[i];
            int number = int.Parse(RijndaelEncryption.Decrypt(playerdata.No, passwordforsavefile));
            string data2 = RijndaelEncryption.Decrypt(playerdata.name, passwordforsavefile);


            SelectButtons[number].SetActive(playerdata.yieldd);
            PurchaseButton[number].SetActive(playerdata.NetworkBuild);
            FruitsIcon[number].SetActive(playerdata.TIO00_UGG);
            skinTemplates[number].UnlockedObjectText.text = data2;

        }
        



    }

}
