
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SnakeGame;
using TMPro;
using System.IO;
using UnityCipher;
using SnakeGame.ShopSystem;

public class SkyItemManager : MonoBehaviour
{
    private int coins;
    public TMP_Text coinUI;
    public SkyItemSO[] skyItemSO;
    public GameObject[] skyPanelsGameObjects;
    public SkyTemplate[] skyTemplates;
    public Sprite[] skyBGImage;


    private List<GameObject> PurchaseButton;
    private List<GameObject> SelectButtons;
    private List<GameObject> SelectedButtons;
    private List<GameObject> FruitsIcon;

    PlayerDataNumber playerDataNumber;
    SaveLoadJSONData saveSystemWithJson;

    public int buttonIndexInskyTemplateGameObject;

    [SerializeField] private string password;
    [SerializeField] private string passwordforsavefile;
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

    public StageItemManager stageItemManager;
    public SkinItemManager skinItemManager;

    //private string isSaved="isSkySaved";
    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    private string indexOfSkyBox = "indexOfSkyBoxes1";
    private string jsonFileName = "FPdr994YYLT-98.json";
    private string persistantDataPath = "";
    private void Awake()
    {
        playerDataNumber = new PlayerDataNumber();
        saveSystemWithJson = new SaveLoadJSONData();
        CheckPurchasable();
        persistantDataPath = Application.persistentDataPath+Path.AltDirectorySeparatorChar;
        
    }
    void Start()
    {
        GetCoins();
        AssignButtons();
        for (int i = 0; i < skyPanelsGameObjects.Length; i++)
        {
            skyPanelsGameObjects[i].SetActive(true);
        }
        LoadPanel();
        CheckPurchasable();
        if (File.Exists(persistantDataPath + jsonFileName))
        {
            LoadButtonState();
        }



        if (PlayerPrefs.HasKey(indexOfSkyBox))
            SetSelectedButtonActive(); //this function will set the selected button active
        else
            PlayerPrefs.SetInt(indexOfSkyBox, 0);
    }

    public void GetCoins()
    {
        coins = saveSystem.ReturnDecryptedScore(password, fruitsEncrypted,fruitsPrefs);

    }
    private void AssignButtons()
    {
        if (PurchaseButton == null) PurchaseButton = new List<GameObject>();
        if (SelectButtons == null) SelectButtons = new List<GameObject>();
        if (SelectedButtons == null) SelectedButtons = new List<GameObject>();
        if (FruitsIcon == null) FruitsIcon = new List<GameObject>();

        for (int i = 0; i < skyPanelsGameObjects.Length; i++)
        {
            PurchaseButton.Add(skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject).gameObject);
            SelectButtons.Add(skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject + 1).gameObject);
            SelectedButtons.Add(skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject + 2).gameObject);
            FruitsIcon.Add(skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject + 3).gameObject);
        }
    }
    private void LoadPanel()
    {
        for (int i = 0; i < skyItemSO.Length; i++)
        {
            skyTemplates[i].TitleText.text = skyItemSO[i].Price;
            skyTemplates[i].BGSprite = skyBGImage[i];
            skyTemplates[i].NameOfSky = skyItemSO[i].name;
            LoadImageOfPanels(i);
        }
    }
    public void LoadImageOfPanels(int i)
    {
        skyPanelsGameObjects[i].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = skyTemplates[i].BGSprite;
    }
    public void CheckPurchasable()
    {
        for (int i = 0; i < skyPanelsGameObjects.Length; i++)
        {
            if (coins >= skyItemSO[i].baseCost)
            {
                skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                skyPanelsGameObjects[i].transform.GetChild(buttonIndexInskyTemplateGameObject).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void PurchaseItem(int buttonNumber)
    {
        if (coins >= skyItemSO[buttonNumber].baseCost)
        {
            skyPanelsGameObjects[buttonNumber].transform.GetChild(3).gameObject.SetActive(false);
            coins = coins - skyItemSO[buttonNumber].baseCost;
            coinUI.text = coins.ToString();
            int tempCoin = skyItemSO[buttonNumber].baseCost;
            saveSystem.EncryptPrefsNegative(tempCoin, password, fruitsEncrypted, fruitsPrefs);
            CheckPurchasable();

            stageItemManager.GetCoins();
            stageItemManager.CheckPurchasable();

            skinItemManager.GetCoins();
            skinItemManager.CheckPurchasable();

            Select(buttonNumber);
        }
    }
    private void SetSelectedButtonActive()
    {
        for (int i = 0; i < SelectedButtons.Count; i++)
        {
            SelectedButtons[i].SetActive(false);
        }
        SelectedButtons[PlayerPrefs.GetInt(indexOfSkyBox)].SetActive(true);
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
            PlayerPrefs.SetInt(indexOfSkyBox, buttonNumber);
        }
    }
    public void Select(int buttonNumber)
    {
        bool isActive = true;
        bool isNotActive = false;
        PurchaseButton[buttonNumber].SetActive(isNotActive);
        SelectButtons[buttonNumber].SetActive(isActive);
        skyTemplates[buttonNumber].UnlockedObjectText.text = skyItemSO[buttonNumber].Name;
        
        SaveButtonState(buttonNumber,skyItemSO[buttonNumber].Name,isActive,isNotActive);
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
            //playerdata = saveSystemWithJson.LoadData((playerDataNumber.buttonNumberList[i]+".json"));
            playerdata = playerDataNumber.NetworkingCommand[i];
            int number = int.Parse(RijndaelEncryption.Decrypt(playerdata.No, passwordforsavefile));
            string data2 = RijndaelEncryption.Decrypt(playerdata.name, passwordforsavefile);


            SelectButtons[number].SetActive(playerdata.yieldd);
            PurchaseButton[number].SetActive(playerdata.NetworkBuild);
            FruitsIcon[number].SetActive(playerdata.TIO00_UGG);

            skyTemplates[number].UnlockedObjectText.text = data2;
        }


    }
}
