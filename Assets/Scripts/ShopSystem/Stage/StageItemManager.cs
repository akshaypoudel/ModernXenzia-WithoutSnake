using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityCipher;
using System.IO;

namespace SnakeGame.ShopSystem
{

    public class StageItemManager : MonoBehaviour
    {
        private int coins;
        public TMP_Text coinUI;
        public bool isSkinTab;
        public StageItemSO[] stageItemSO;
        public GameObject[] stagePanelsGameObjects;
        public StageTemplate[] stageTemplates;
        public Sprite[] stageBGImage;


        private List<GameObject> PurchaseButton;
        private List<GameObject> SelectButtons;
        private List<GameObject> SelectedButtons;
        private List<GameObject> FruitIcon;

        public int buttonIndexInStageTemplateGameObject;

        [SerializeField]private string password;
        [SerializeField] private string passwordforsavefile;
        PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

        private string jsonFileName = "LOG_UNIT_GRADLE_ID_99gh68tt33.json";
        private string fruitsEncrypted = "FruitsEncrypted";
        private string fruitsPrefs = "Fruits";
        //private string isSaved= "isStageSaved";
        private string indexOfMat = "indexOfMaterial1";
        private string persistantDataPath="";

        public SkinItemManager skinItemManager;
        public SkyItemManager SkyItemManager;
        
        
        private SaveLoadJSONData saveSystemWithJson;
        PlayerDataNumber playerDataNumber;
        

        private void Awake()
        {
            saveSystemWithJson = new SaveLoadJSONData();
            playerDataNumber = new PlayerDataNumber();
            persistantDataPath=Application.persistentDataPath+Path.AltDirectorySeparatorChar;
            //CheckPurchasable();
            

        }
        void Start()
        {
            GetCoins();
            AssignButtons();
            for (int i = 0; i < stagePanelsGameObjects.Length; i++)
            {
                stagePanelsGameObjects[i].SetActive(true);
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
            coins = saveSystem.ReturnDecryptedScore(fruitsEncrypted);
        }
        public void AssignButtons()
        {
            if (PurchaseButton == null) PurchaseButton = new List<GameObject>();
            if (SelectButtons == null) SelectButtons = new List<GameObject>();
            if (SelectedButtons == null) SelectedButtons = new List<GameObject>();
            if (FruitIcon == null) FruitIcon = new List<GameObject>();

            for (int i = 0; i < stagePanelsGameObjects.Length; i++)
            {
                PurchaseButton.Add(stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject);
                SelectButtons.Add(stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject+1).gameObject);
                SelectedButtons.Add(stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject+2).gameObject);
                FruitIcon.Add(stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject+3).gameObject);
            }
        }

        public void LoadPanel()
        {
            for (int i = 0; i < stageItemSO.Length; i++)
            {
                stageTemplates[i].TitleText.text = stageItemSO[i].Price;
                stageTemplates[i].BGSprite = stageBGImage[i];
                stageTemplates[i].NameOfStage = stageItemSO[i].name;
                LoadImageOfPanels(i);
            }
        }
        public void LoadImageOfPanels(int i)
        {
            stagePanelsGameObjects[i].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = stageTemplates[i].BGSprite;
        }
        public void CheckPurchasable()
        {
            for (int i = 0; i < stagePanelsGameObjects.Length; i++)
            {
                if (coins >= stageItemSO[i].baseCost)
                {
                    stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject.GetComponent<Button>().interactable = true;
                }
                else
                {
                    stagePanelsGameObjects[i].transform.GetChild(buttonIndexInStageTemplateGameObject).gameObject.GetComponent<Button>().interactable = false;
                }
            }
        }
        public void PurchaseItem(int buttonNumber)
        {
            if (coins >= stageItemSO[buttonNumber].baseCost)
            {
                stagePanelsGameObjects[buttonNumber].transform.GetChild(3).gameObject.SetActive(false);
                coins = coins - stageItemSO[buttonNumber].baseCost;
                coinUI.text = coins.ToString();
                int tempCoin = stageItemSO[buttonNumber].baseCost;
                saveSystem.EncryptPrefsNegative(tempCoin,fruitsEncrypted);
                CheckPurchasable();
                Select(buttonNumber);

                SkyItemManager.GetCoins();
                SkyItemManager.CheckPurchasable();

                skinItemManager.GetCoins();
                skinItemManager.CheckPurchasable();

            }
        }
        private void SetSelectedButtonActive()
        {
            for(int i =0; i<SelectedButtons.Count;i++)
            { 
                SelectedButtons[i].SetActive(false);    
            }
            SelectedButtons[PlayerPrefs.GetInt(indexOfMat)].SetActive(true);
        }
        public void Selected(int buttonNumber)
        {
            if(SelectButtons[buttonNumber].activeSelf)
            {
                for(int i = 0;i < SelectedButtons.Count;i++)
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
            stageTemplates[buttonNumber].UnlockedObjectText.text = stageItemSO[buttonNumber].Name;
            FruitIcon[buttonNumber].SetActive(isNotActive);
            SaveButtonState(buttonNumber, stageItemSO[buttonNumber].Name, isActive, isNotActive);
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
            saveSystemWithJson.SavePlayerDataNumber(playerDataNumber,jsonFileName);
        }
        public void LoadButtonState()
        {
            PlayerDataEncrypted playerdata = new PlayerDataEncrypted();
            playerDataNumber = saveSystemWithJson.LoadPlayerDataNumber(jsonFileName);


            for (int i = 0; i < playerDataNumber.NetworkingCommand.Count; i++)
            {

                playerdata = playerDataNumber.NetworkingCommand[i];
                //decrypting the buttonNumber of the object
                int number = int.Parse(RijndaelEncryption.Decrypt(playerdata.No, passwordforsavefile));
                //decrypting the name of the unlocked object
                string data2 = RijndaelEncryption.Decrypt(playerdata.name, passwordforsavefile);
                    

                SelectButtons[number].SetActive(playerdata.yieldd);
                PurchaseButton[number].SetActive(playerdata.NetworkBuild);
                FruitIcon[number].SetActive(playerdata.TIO00_UGG);
                stageTemplates[number].UnlockedObjectText.text = data2 ;

            }
            

            
        }
    }
}
