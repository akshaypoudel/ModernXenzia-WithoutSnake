using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SnakeGame;

public class PlayModeItemManager : MonoBehaviour
{

    public PlayModeSO[] playModeItemSO;
    public GameObject[] playModeTemplateGameObjects;
    public PlayModeTemplate[] playModeTemplates;
    public Sprite[] stageBGImage;
    private int coins;
    public int positionOfImageInTheGameObject;

    public string password;
    PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();

    private string fruitsEncrypted = "FruitsEncrypted";
    private string fruitsPrefs = "Fruits";
    //private string jellyPrefs = "Jelly";
    //private string jellyEncrypted = "JellyEncrypted";

    private void Start()
    {
        //GetCoins();
        //AssignButtons();
        //for (int i = 0; i < playModeTemplateGameObjects.Length; i++)
        //{
        //    playModeTemplateGameObjects[i].SetActive(true);
        //}
        LoadPanel();
        //CheckPurchasable();
    }
    public void GetCoins()
    {
        coins = saveSystem.ReturnDecryptedScore(password, fruitsEncrypted,fruitsPrefs);
    }
    public void CheckPurchasable()
    {
        for (int i = 0; i < playModeTemplateGameObjects.Length; i++)
        {
            if (coins >= playModeItemSO[i].baseCost)
            {
                playModeTemplateGameObjects[i].transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                playModeTemplateGameObjects[i].transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void LoadPanel()
    {
        for (int i = 0; i < playModeItemSO.Length; i++)
        {
            playModeTemplates[i].TitleText.text = playModeItemSO[i].Price;
            playModeTemplates[i].BGSprite = stageBGImage[i];
            LoadImageOfPanels(i);
            //stageTemplates[i].CostText.text = stageItemSO[i].baseCost.ToString();
        }
    }
    public void LoadImageOfPanels(int i)
    {
        playModeTemplateGameObjects[i].transform.GetChild(positionOfImageInTheGameObject).gameObject.GetComponent<Image>().sprite = playModeTemplates[i].BGSprite;
    }



}
