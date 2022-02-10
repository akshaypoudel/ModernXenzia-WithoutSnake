using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using GoogleMobileAds.Api;
using PhoneVibrator;
using System;

namespace SnakeGame
{
    public class SnakeHandler : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        #region VariableReferences

        #region PlayerPrefsStrings
        private string fruitsEncrypted = "FruitsEncrypted";
        private string fruitsPrefs = "Fruits";
        private string jellyPrefs = "Jelly";
        private string jellyEncrypted = "JellyEncrypted";
        private string controls = "WhichControls";
        #endregion

        #region Buttons
        [Header("Buttons")]

        public Button upButton, upButton1;
        public Button leftButton, rightButton;
        public Button downButton, downButton1;

        #endregion

        #region Basic Data Fields
        [Header("Basic Data Fields")]
        [SerializeField] private string password;
        private float snakeMoveTimer;
        [SerializeField] private float snakeMoveTimerMax = 0.525f;
        private int fruitsCount = 0;
        private int jellyCount = 0;
        public float snakeSpeed = 0.05f;
        public int stopIncreasingSpeedOfSnakeAfter = 17;
        private bool isSnakeMoving = false;
        private bool canWePressButton = true;
        private bool isInvincible = false;
        [HideInInspector] public static int snakeBodySize;
        public bool isObstacleMode;
        public float overlapSphereRadius = 0.5f;
        #endregion

        #region Text Mesh Pro References
        [Header("Text Mesh Pro References")]
        public Text fruitsText;
        public Text jellyText;
        public TMP_Text scoreText;
        //public TMP_Text HighScoreText;
        #endregion

        #region List References
        [Header("List References")]
        public BoxCollider2D[] obstacles;
        private List<SnakeMovePosition> snakeMovePosList = new List<SnakeMovePosition>();
        private List<SnakeBodyPart> snakeBodyPartsList = new List<SnakeBodyPart>();
        #endregion

        #region GameObject Refereneces
        [Header("GameObject Refereneces")]
        public GameObject GameOverPanel;
        public GameObject PauseButton;
        public GameObject adButton;
        public GameObject buttonControls;
        public Collider2D[] colliders;
        public ParticleSystem invincibleEffect;

        #endregion

        #region Enumeration Reference
        private Direction gridMoveDirection;
        #endregion

        #region Vector2 for storing snake position

        [Header("Vector2 for storing snake position")]
        public Vector2Int snakePosAtTheTimeOfSceneLoading;
        private Vector2 snakePosition;

        #endregion

        #region Other Scripts References
        [Header("Other Scripts References")]
        //public Transform foodDestination;
        public FoodEatingAnimation anim;
        private SpawnFood spawnFood;
        private SettingsMenu settingsMenu;
        private PlayerPrefsSaveSystem saveSystem = new PlayerPrefsSaveSystem();
        public AdManager admanager;
        #endregion

        #endregion


        private Vector2Int gridMoveDirectionVector;
        private SnakeMovePosition previousSnakeMovePosition = null;
        private SnakeMovePosition snakeMovePosition1;
        private bool snakeAteFood;
        private Vector2 snakebodypart;
        private Vector2 Distance;


        #region Mobilecontrols
        private Vector2 startTouchPosition;
        private Vector2 currentPosition;
        private Vector2 endTouchPosition;
        private bool stopTouch = false;
        [HideInInspector]
        public bool isTouchControls;

        [SerializeField] private float swipeRange;
        [SerializeField] private float jumpRange;

        #endregion

        private void Start()
        {
            UIButtonClick();
            settingsMenu = FindObjectOfType<SettingsMenu>();
            isSnakeMoving = false;
            canWePressButton = true;
            snakePosition = snakePosAtTheTimeOfSceneLoading;
            snakeMoveTimer = snakeMoveTimerMax;
            snakeBodySize = 1;
        }
        //BUTTONS CLICK
        private void UIButtonClick()
        {
            upButton.onClick.AddListener(Up);
            upButton1.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);
            downButton1.onClick.AddListener(Down);
            leftButton.onClick.AddListener(Left);
            rightButton.onClick.AddListener(Right);
        }
        private void Update()
        {
//#if UNITY_ANDROID
            if (canWePressButton && isTouchControls)
                MobileControls(); //Function for handling touch in android phones 
//#elif UNITY_EDITOR
 //           if (canWePressButton)
 //               KeyBoardInput(); //Function for handling input via keyboard
//#endif
            if (isSnakeMoving)
                HandleGridMovement();  //function for handling snake position


        }
        private void KeyBoardInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Up();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Down();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Left();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Right();
            }
        }

        #region Snake Moving Direction Methods

        private void Up()
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
                isSnakeMoving = true;
                SoundManager.PlaySound(SoundManager.Sound.DirectionChanging);
            }
        }
        private void Down()
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
                isSnakeMoving = true;
                SoundManager.PlaySound(SoundManager.Sound.DirectionChanging);
            }
        }
        private void Right()
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
                isSnakeMoving = true;
                SoundManager.PlaySound(SoundManager.Sound.DirectionChanging);
            }
        }
        private void Left()
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
                isSnakeMoving = true;
                SoundManager.PlaySound(SoundManager.Sound.DirectionChanging);
            }
        }
        #endregion


        private void HandleGridMovement()
        {
            snakeMoveTimer += Time.deltaTime;
            if (snakeMoveTimer >= snakeMoveTimerMax)
            {
                snakeMoveTimer -= snakeMoveTimerMax;

                if (snakeMovePosList.Count > 0)
                {
                    previousSnakeMovePosition = snakeMovePosList[0];
                }

                snakeMovePosition1 = new SnakeMovePosition(previousSnakeMovePosition, snakePosition, gridMoveDirection);
                snakeMovePosList.Insert(0, snakeMovePosition1);

                switch (gridMoveDirection)
                {
                    default:
                    case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                    case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                    case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                    case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
                }

                snakePosition += gridMoveDirectionVector;
                if (isObstacleMode)
                    snakePosition = spawnFood.ValidateGridPositionForHardMode(snakePosition);
                else
                    snakePosition = spawnFood.ValidateGridPositionForFreeMode(snakePosition);

                snakeAteFood = spawnFood.DoesSnakeAteFood(snakePosition);
                if (snakeAteFood)
                {
                    ++snakeBodySize;
                    CreateSnakeBodyPart();
                    //anim.StartFoodMove(snakePosition); //food going to Score board animation
                    //spawnFood.IncreaseScoreAfterFoodReach();
                }

                if (snakeMovePosList.Count >= snakeBodySize + 1)
                {
                    snakeMovePosList.RemoveAt(snakeMovePosList.Count - 1);
                }
                if (isInvincible == false)
                    SnakeCollidingWithHisBody();
                transform.position = new Vector3(snakePosition.x, snakePosition.y);
                transform.eulerAngles = new Vector3(0, 0, -GetAngleFromVector(gridMoveDirectionVector) - 180);

                UpdateSnakeBodyPart();
            }
        }

        public void ScoreCounter(string nameOfConsumable)
        {
            if (nameOfConsumable == "Jelly")
            {
                SoundManager.PlaySound(SoundManager.Sound.SnakeEatJelly);
                jellyCount += 1;
                jellyText.text = jellyCount.ToString();
            }
            else if (nameOfConsumable == "Fruits")
            {
                SoundManager.PlaySound(SoundManager.Sound.FoodReachDestination);
                fruitsCount += 1;
                fruitsText.text = fruitsCount.ToString();
            }
            //SNAKE SPEED AFTER EVERY FOOD EATEN   
            if (snakeBodySize >= 1 && snakeBodySize <= stopIncreasingSpeedOfSnakeAfter)
            {
                switch (snakeBodySize)
                {
                    case 1:
                        snakeMoveTimerMax -= (snakeSpeed * 1.4f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 2:
                        snakeMoveTimerMax -= (snakeSpeed * 1.4f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 3:
                        snakeMoveTimerMax -= (snakeSpeed * 1.25f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 4:
                        snakeMoveTimerMax -= (snakeSpeed * 1.1f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 5:
                        snakeMoveTimerMax -= (snakeSpeed * 0.9f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 6:
                        snakeMoveTimerMax -= (snakeSpeed * 0.5f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 7:
                        snakeMoveTimerMax -= (snakeSpeed * 0.45f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 8:
                        snakeMoveTimerMax -= (snakeSpeed * 0.3f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    case 13:
                        snakeMoveTimerMax -= (snakeSpeed * 0.2f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;
                    default:
                        snakeMoveTimerMax -= (snakeSpeed * 0.09f);
                        //Debug.Log("SnakeSpeed: " + snakeMoveTimerMax);
                        break;

                }
            }
        }
        public void SnakeCollidingWithHisBody()
        {
            foreach (SnakeMovePosition snakeBodyParts in snakeMovePosList)
            {
                snakebodypart = snakeBodyParts.GetGridPosition();
                if (snakePosition == snakebodypart)
                {
                    canWePressButton = false;
                    GameOver();
                }
            }
        }

        private float GetAngleFromVector(Vector2Int direction)
        {
            float n = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }

        public void Setup(SpawnFood spawnFood)
        {
            this.spawnFood = spawnFood;
        }

        public Vector2 GetGridPos()
        {
            return snakePosition;
        }

        public List<Vector2> GetFullSnakeGridPosList()
        {
            List<Vector2> gridPositionList = new List<Vector2>() { snakePosition };
            foreach (SnakeMovePosition snakeMovePosition in snakeMovePosList)
            {
                gridPositionList.Add(snakeMovePosition.GetGridPosition());
            }
            return gridPositionList;
        }

        public void CreateSnakeBodyPart()
        {
            snakeBodyPartsList.Add(new SnakeBodyPart());
        }
        public void UpdateSnakeBodyPart()
        {
            for (int i = 0; i < snakeBodyPartsList.Count; i++)
            {
                snakeBodyPartsList[i].SetSnakeMovePosition(snakeMovePosList[i]);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "obstacles" && !isInvincible)
            {
                isSnakeMoving = false;
                canWePressButton = false;
                GameOver();
            }
        }


        public void SaveFruitsAndJelly()
        {
            saveSystem.EncryptPrefsPositive(fruitsCount, password, fruitsEncrypted, fruitsPrefs);
            saveSystem.EncryptPrefsPositive(jellyCount, password, jellyEncrypted, jellyPrefs);
        }
        public void GameOver()
        {
            if (Vibrator.canVibrate)
            {
                Vibrator.VibratePhone();
            }
            SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
            if (buttonControls.activeSelf)
                buttonControls.SetActive(false);
            isSnakeMoving = false;
            canWePressButton = false;

            admanager.RequestRewardedAd();
            admanager.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            admanager.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;

            StartCoroutine(WaitSomeTimeAfterGameOver());
        }
        IEnumerator WaitSomeTimeAfterGameOver()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 1f;
            GameOverPanel.SetActive(true);
            PauseButton.SetActive(false);
            //TouchControl.SetActive(false);
            //SaveFruitsAndJelly();
            DisplayAd();
        }



        #region ADs
        public void DisplayAd()
        {
            int a = UnityEngine.Random.Range(0, 7);
            if (a == 5 || a==3)
                StartCoroutine(ShowInterestialAD());

            //showRewardedAd();
        }
        public void showRewardedAd()
        {
            //SoundManager.Mute();
            //admanager.RequestRewardedAd();
           // admanager.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            admanager.rewardedAd.OnAdLoaded += HandleOnAdLoaded;
            admanager.rewardedAd.OnAdOpening += HandleOnAdOpening;
            admanager.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            admanager.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
            if (admanager.rewardedAd.IsLoaded())
            {
                admanager.rewardedAd.Show();
            }
            //else
            //    adButton.SetActive(false);


            
            //admanager.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        }

        private void HandleOnAdOpening(object sender, EventArgs e)
        {
            SoundManager.Mute();
            admanager.bannerAd.Destroy();
        }

        private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            SoundManager.AudioResume();
            adButton.SetActive(false);
        }

        private void HandleOnAdLoaded(object sender, EventArgs e)
        {
            //SoundManager.Mute();
        }

        private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            adButton.SetActive(false);
        }

        private void HandleUserEarnedReward(object sender, Reward e)
        {
            GiveNewLifeToPlayer();
        }

        private IEnumerator ShowInterestialAD()
        {
            yield return new WaitForSeconds(0.2f);
            //admanager.RequestInterstitial();
            if (admanager.interstitial.IsLoaded())
            {
                admanager.bannerAd.Destroy();
                admanager.interstitial.Show();
            }
            admanager.interstitial.OnAdClosed += HandleOnAdClosed;
        }

        private void HandleOnAdClosed(object sender, EventArgs e)
        {
            admanager.interstitial.Destroy();
            admanager.RequestBanner();
        }
        public void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            SoundManager.AudioResume();
            admanager.RequestRewardedAd();
            admanager.RequestBanner();
        }


        #endregion
        #region Reward
        public void GiveNewLifeToPlayer()
        {
            //SoundManager.AudioResume();
            isInvincible = true;
            GameOverPanel.SetActive(false);
            PauseButton.SetActive(true);
            if(PlayerPrefs.GetInt(controls)==0)
                buttonControls.SetActive(true);
            Time.timeScale = 1f;
            isSnakeMoving = true;
            canWePressButton = true;
            StartCoroutine(MakeTheSnakeInvinsibleForShortTime());
        }
        
        private IEnumerator MakeTheSnakeInvinsibleForShortTime()
        {
            invincibleEffect.Play();
            yield return new WaitForSeconds(6f);
            invincibleEffect.Stop();
            yield return new WaitForSeconds(0.1f);
            invincibleEffect.Play();
            yield return new WaitForSeconds(0.1f);
            invincibleEffect.Stop();
            yield return new WaitForSeconds(0.1f);
            invincibleEffect.Play();
            yield return new WaitForSeconds(0.1f);
            invincibleEffect.Stop();
            yield return new WaitForSeconds(0.5f);

            isInvincible = false;
        }
        #endregion

        private void MobileControls()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                currentPosition = Input.GetTouch(0).position;
                Distance = currentPosition - startTouchPosition;

                if (!stopTouch)
                {

                    if (Distance.x < -swipeRange)
                    {
                        Left();
                        stopTouch = true;
                    }
                    else if (Distance.x > swipeRange)
                    {
                        Right();
                        stopTouch = true;
                    }
                    else if (Distance.y > jumpRange)
                    {
                        Up();
                        stopTouch = true;
                    }
                    else if(Distance.y < -jumpRange)
                    {
                        Down();
                        stopTouch = true;
                    }
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                stopTouch = false;

                endTouchPosition = Input.GetTouch(0).position;

                //Vector2 Distance = endTouchPosition - startTouchPosition;
            }

        }


        public void spawnAnotherFood()
        {
            spawnFood.SpawnApple();
        }


        private class SnakeBodyPart
        {

            private SnakeMovePosition snakeMovePosition;
            private Transform transform1;
            GameObject SnakeBody;

            public SnakeBodyPart()
            {
                SnakeBody = Instantiate(GameAssets.i.snakeBodySprite);

                transform1 = SnakeBody.transform;
            }

            public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
            {
                this.snakeMovePosition = snakeMovePosition;

                transform1.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

                float angle;
                switch (snakeMovePosition.GetDirection())
                {
                    default:
                    case Direction.Up: // Currently going Up
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 0;
                                break;
                            case Direction.Left: // Previously was going Left
                                angle = 0 + 45;
                                transform1.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Right: // Previously was going Right
                                angle = 0 - 45;
                                transform1.position += new Vector3(-.2f, .2f);
                                break;
                        }
                        break;
                    case Direction.Down: // Currently going Down
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 180;
                                break;
                            case Direction.Left: // Previously was going Left
                                angle = 180 - 45;
                                transform1.position += new Vector3(.2f, -.2f);
                                break;
                            case Direction.Right: // Previously was going Right
                                angle = 180 + 45;
                                transform1.position += new Vector3(-.2f, -.2f);
                                break;
                        }
                        break;
                    case Direction.Left: // Currently going to the Left
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = +90;
                                break;
                            case Direction.Down: // Previously was going Down
                                angle = 180 - 45;
                                transform1.position += new Vector3(-.2f, .2f);
                                break;
                            case Direction.Up: // Previously was going Up
                                angle = 45;
                                transform1.position += new Vector3(-.2f, -.2f);
                                break;
                        }
                        break;
                    case Direction.Right: // Currently going to the Right
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = -90;
                                break;
                            case Direction.Down: // Previously was going Down
                                angle = 180 + 45;
                                transform1.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Up: // Previously was going Up
                                angle = -45;
                                transform1.position += new Vector3(.2f, -.2f);
                                break;
                        }
                        break;
                }

                transform1.eulerAngles = new Vector3(0, 0, angle);
            }

        }

        private class SnakeMovePosition
        {

            private SnakeMovePosition previousSnakeMovePosition;
            private Vector2 gridPosition;
            private Direction direction;

            public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2 gridPosition, Direction direction)
            {
                this.previousSnakeMovePosition = previousSnakeMovePosition;
                this.gridPosition = gridPosition;
                this.direction = direction;
            }

            public Vector2 GetGridPosition()
            {
                return gridPosition;
            }

            public Direction GetDirection()
            {
                return direction;
            }

            public Direction GetPreviousDirection()
            {
                if (previousSnakeMovePosition == null)
                {
                    return Direction.Right;
                }
                else
                {
                    return previousSnakeMovePosition.direction;
                }
            }

        }

    }
}
