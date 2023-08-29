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

    private void Start()
    {
        LoadPanel();
    }
    public void GetCoins()
    {
        coins = saveSystem.ReturnDecryptedScore( fruitsEncrypted);
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
        }
    }
    public void LoadImageOfPanels(int i)
    {
        playModeTemplateGameObjects[i].transform.GetChild(positionOfImageInTheGameObject).gameObject.GetComponent<Image>().sprite = playModeTemplates[i].BGSprite;
    }



}
