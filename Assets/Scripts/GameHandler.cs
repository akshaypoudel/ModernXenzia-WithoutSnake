using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SnakeGame
{

    public class GameHandler : MonoBehaviour
    {
        public Material []BGArray;
        public GameObject ButtonControls;

        public GameObject snake;
        public Sprite[] snakeBodySprite;
        public int spawnPosX, spawnPosY;
        [SerializeField] private SnakeHandler snakeHandler;
        public GameObject BG;
        private SpawnFood spawnFood1;
        private string indexOfMat = "indexOfMaterial1";
        private string indexOfSkyBox = "indexOfSkyBoxes1";
        private string indexOfSkin = "indexOfSkins1";
        private string controls = "WhichControls";



        private void Start()
        {
            spawnFood1 = new SpawnFood(spawnPosX,spawnPosY);

            snakeHandler.Setup(spawnFood1);
            spawnFood1.Setup(snakeHandler);

            if(PlayerPrefs.HasKey(controls))
            CheckControls();

            RenderSettings.skybox = GameAssets.i.skyboxes[PlayerPrefs.GetInt(indexOfSkyBox)];
            BG.GetComponent<MeshRenderer>().material = BGArray[PlayerPrefs.GetInt(indexOfMat)];                                             
            snake.GetComponent<SpriteRenderer>().sprite = snakeBodySprite[PlayerPrefs.GetInt(indexOfSkin)];
            
        }
        
        public void CheckControls()
        {
            if (PlayerPrefs.GetInt(controls) == 0)
            {
                ButtonControls.SetActive(true);
                snakeHandler.isTouchControls = false;
            }
            else if (PlayerPrefs.GetInt(controls) == 1)
            {
                ButtonControls.SetActive(false);
                snakeHandler.isTouchControls = true;
            }
        }

    }
}
