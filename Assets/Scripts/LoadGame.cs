using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


namespace SnakeGame
{

    public class LoadGame : MonoBehaviour
    {
        
        private string touchControls = "TouchControls";
        public GameObject PauseMenuUI;
        public GameObject SelectControls;
        public GameObject PauseButton;

        private string controls = "WhichControls";
        
        private string nameOfScene="";

        public void LoadCurrentScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void PlayGame(string _name)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_name);
        }
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void PlayFreeMode(string sceneName)
        {
            if(!PlayerPrefs.HasKey(touchControls))
            {
                SelectControls.SetActive(true);
                nameOfScene = sceneName;
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        public void PlayHardMode(string sceneName)
        {
            if (!PlayerPrefs.HasKey(touchControls))
            {
                SelectControls.SetActive(true);
                nameOfScene=sceneName;
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public void PlayWithTouchControls()
        {
            PlayerPrefs.SetInt(controls,1);
            PlayerPrefs.SetInt(touchControls, 11);
            SceneManager.LoadScene(nameOfScene);
        }
        public void PlayWithButtonControls()
        {
            PlayerPrefs.SetInt(controls,0);
            PlayerPrefs.SetInt(touchControls, 11);
            SceneManager.LoadScene(nameOfScene);

        }

        public void Resume()
        {
            Time.timeScale = 1f;
            PauseMenuUI.SetActive(false);
            PauseButton.SetActive(true);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            PauseMenuUI.SetActive(true);
            PauseButton.SetActive(false);
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}
