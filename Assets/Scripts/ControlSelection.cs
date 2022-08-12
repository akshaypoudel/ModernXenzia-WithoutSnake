using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnakeGame;

public class ControlSelection : MonoBehaviour
{
    public List<GameObject> SelectedButtons;
    private string controlsPrefs = "controls";
    private string controls = "WhichControls";
    public GameHandler gameHandler;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(controlsPrefs))
            SetSelectedButtonActive();
        else
        {
            PlayerPrefs.SetInt(controlsPrefs, PlayerPrefs.GetInt(controls));
            SetSelectedButtonActive();
        }
    }
    private void SetSelectedButtonActive()
    {
        for (int i = 0; i < SelectedButtons.Count; i++)
        {
            SelectedButtons[i].SetActive(false);
        }
        SelectedButtons[PlayerPrefs.GetInt(controlsPrefs)].SetActive(true);
    }

    public void Selected(int buttonNumber)
    {
        for (int i = 0; i < SelectedButtons.Count; i++)
        {
            SelectedButtons[i].SetActive(false);
        }
        SelectedButtons[buttonNumber].SetActive(true);
        PlayerPrefs.SetInt(controlsPrefs, buttonNumber);
        
        if(buttonNumber == 0)
            PlayerPrefs.SetInt(controls, 0);
        else if(buttonNumber == 1)
            PlayerPrefs.SetInt(controls, 1);

        if(gameHandler != null)
        gameHandler.CheckControls();
    }

}
