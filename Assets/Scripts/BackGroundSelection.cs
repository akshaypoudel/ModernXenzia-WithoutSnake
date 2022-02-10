using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class BackGroundSelection : MonoBehaviour
    {
       /* public GameObject[] maps;
        public int selectedmaps = 0;

        private void Start()
        {
            selectedmaps = LoadGame();
            maps[selectedmaps].SetActive(true);
        }
        public void NextCharacter()
        {
           
            maps[selectedmaps].SetActive(false);

            selectedmaps = (selectedmaps + 1) % maps.Length;
            maps[selectedmaps].SetActive(true);
            SaveGame();
        }
        public void PreviousCharacter()
        {
            maps[selectedmaps].SetActive(false);
            selectedmaps--;
            if (selectedmaps < 0)
            {
                selectedmaps += maps.Length;
            }
            maps[selectedmaps].SetActive(true);
            SaveGame();
        }
        public void SaveGame()
        {
            PlayerPrefs.SetInt("completedlevelcount", selectedmaps);
        }
    
        public static int LoadGame()
        {
            return PlayerPrefs.GetInt("completedlevelcount");
        }
       */
    }
}
