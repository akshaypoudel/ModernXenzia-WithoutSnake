using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Advertisements;
using PhoneVibrator;

namespace SnakeGame
{
    public class SnakeHandler : MonoBehaviour
    {
        private enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        #region VariableReferences

        #region PlayerPrefsStrings
        private string fruitsEncrypted = "FruitsEncrypted";
        private string jellyEncrypted = "JellyEncrypted";
        private string controls = "WhichControls";
        #endregion

        #region Buttons
        [Header("Buttons")]

        public Button upButton;
        public Button upButton1;
        public Button leftButton;
        public Button rightButton;
        public Button downButton;
        public Button downButton1;

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
        private bool snakeAteInvincibleApple = false;
        private bool canSpawnAnotherInvincibleApple = true;
        [HideInInspector]public float overlapSphereRadius = 0.8f;
        private bool invincibleAppleEated = false;


        private bool isTimerActive = false;
        private int startTime = 4;
        private float currentTime;

        public Slider invincibleTimerSlider;
        [SerializeField] private float invincibilityTime;
        private bool stopInvinciblityTimer;
        private float time;

        private const float SNAKE_SPEED_INCREASE_PER_FOOD_EATEN=0.006f;

        #endregion

        #region Text Mesh Pro References
        [Header("Text Mesh Pro References")]
        public Text fruitsText;
        public Text jellyText;
        public TMP_Text TimerText;
        #endregion

        #region List References
        
        [HideInInspector]public BoxCollider2D[] obstacles;
        private List<SnakeMovePosition> snakeMovePosList = new List<SnakeMovePosition>();
        private List<SnakeBodyPart> snakeBodyPartsList = new List<SnakeBodyPart>();
        #endregion

        #region GameObject Refereneces
        [Header("GameObject Refereneces")]
        public GameObject GameOverPanel;
        public GameObject PauseButton;
        public GameObject adButton;
        public GameObject buttonControls;
        public GameObject InvincibilityTimerGameObject;
       [HideInInspector] public Collider2D[] colliders;
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
        [SerializeField] private InterstitialAds interstitialAds;
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

        #endregion
        
        private void Start()
        {
            UIButtonClick();
            InitializeVariables();
        }
        private void UIButtonClick()
        {
            upButton.onClick.AddListener(Up);
            upButton1.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);
            downButton1.onClick.AddListener(Down);
            leftButton.onClick.AddListener(Left);
            rightButton.onClick.AddListener(Right);
        }

        private void InitializeVariables()
        {
            settingsMenu = FindObjectOfType<SettingsMenu>();
            isSnakeMoving = false;
            canWePressButton = true;
            snakePosition = snakePosAtTheTimeOfSceneLoading;
            snakeMoveTimer = snakeMoveTimerMax;
            snakeBodySize = 1;
            gridMoveDirection = Direction.None;
            canSpawnAnotherInvincibleApple = true;
            currentTime = startTime;
            stopInvinciblityTimer = false;
            invincibleTimerSlider.maxValue = invincibilityTime;
            invincibleTimerSlider.value = invincibilityTime;
            time = invincibilityTime;
        }


        private void FixedUpdate()
        {
            if (isSnakeMoving)
                HandleSnakeMovement();  //function for handling snake position
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (canWePressButton)
                KeyBoardInput();
#endif
            if (canWePressButton && isTouchControls)
                 MobileControls();
            if (isTimerActive)
                Timer();
            if (isInvincible && !stopInvinciblityTimer)
            {
                InvinciblityTimer();
            }
        }

        private void KeyBoardInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Right();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Left();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Up();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Down();
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


        private void HandleSnakeMovement()
        {
            snakeMoveTimer += Time.deltaTime;
            if (snakeMoveTimer >= snakeMoveTimerMax)
            {
                snakeMoveTimer -= snakeMoveTimerMax;

                UpdateSnakePosition();

                SetSnakeMovePosition();

                ValidateSnakePosition();

                CheckIfSnakeEatInvincibleApple();

                CheckIfSnakeEatFood();

                if (snakeMovePosList.Count >= snakeBodySize + 1)
                {
                    snakeMovePosList.RemoveAt(snakeMovePosList.Count - 1);
                }
                
                CheckIfSnakeIsInvincibleOrNot();

                SetNewSnakePosition();
                UpdateSnakeBodyPart();
            }
        }
        private void UpdateSnakePosition()
        {
            if (snakeMovePosList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePosList[0];
            }
        }

        private void SetSnakeMovePosition()
        {
            snakeMovePosition1 = new SnakeMovePosition(previousSnakeMovePosition, snakePosition, gridMoveDirection);
            snakeMovePosList.Insert(0, snakeMovePosition1);

            switch (gridMoveDirection)
            {
                default:
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            snakePosition += gridMoveDirectionVector;

        }
        
        private void ValidateSnakePosition()
        {
            if (isObstacleMode)
                snakePosition = spawnFood.ValidateGridPositionForHardMode(snakePosition);
            else if (!isObstacleMode)
                snakePosition = spawnFood.ValidateGridPositionForFreeMode(snakePosition);
        }
        
        private void CheckIfSnakeEatInvincibleApple()
        {
            if (canSpawnAnotherInvincibleApple)
            {
                spawninvincibleapple();

            }
            snakeAteInvincibleApple = spawnFood.DoesSnakeAteInvincibleApple(snakePosition);
            if (snakeAteInvincibleApple)
            {
                canSpawnAnotherInvincibleApple = true;
                ResetInvincibilityTimerSlider();
                StartCoroutine(MakeTheSnakeInvinsibleForShortTime());
            }
        }
        private void ResetInvincibilityTimerSlider()
        {
            invincibleAppleEated = true;
            time = invincibilityTime;
        }


        private void CheckIfSnakeEatFood()
        {
            snakeAteFood = spawnFood.DoesSnakeAteFood(snakePosition);
            if (snakeAteFood)
            {
                ++snakeBodySize;
                CreateSnakeBodyPart();
                IncreaseSnakeSpeed();
            }
        }
        
        private void CheckIfSnakeIsInvincibleOrNot()
        {
            if (isInvincible == false)
                SnakeCollidingWithHisBody();
        }

        private void SetNewSnakePosition()
        {
            Vector3 pos = new Vector3(snakePosition.x, snakePosition.y);
            transform.position = Vector2.Lerp(transform.position, pos, 1f);

            transform.eulerAngles = new Vector3(0, 0, -GetAngleFromVector(gridMoveDirectionVector) - 180);
        }

        public void UpdateSnakeBodyPart()
        {
            for (int i = 0; i < snakeBodyPartsList.Count; i++)
            {
                snakeBodyPartsList[i].SetSnakeMovePosition(snakeMovePosList[i]);
            }
        }

        public void spawninvincibleapple()
        {
            if (snakeBodySize == 12 || snakeBodySize == 22 || snakeBodySize == 32 || snakeBodySize == 42 ||
                snakeBodySize == 52 || snakeBodySize == 60 || snakeBodySize == 72 || snakeBodySize == 82
                || snakeBodySize == 92 || snakeBodySize == 102 || snakeBodySize == 112 || snakeBodySize == 122
                || snakeBodySize == 132)
            {
                canSpawnAnotherInvincibleApple = false;
                spawnFood.SpawnInvincibleApple();
            }

        }

        public void CreateSnakeBodyPart()
        {
            snakeBodyPartsList.Add(new SnakeBodyPart());
        }

        private void IncreaseSnakeSpeed()
        {
            float speedAfterLongSnake=0.0021f;
            if (snakeBodySize >= 1 && snakeBodySize <= stopIncreasingSpeedOfSnakeAfter)
            {
                switch (snakeBodySize)
                {
                    case 1:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 2:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 3:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 4:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 5:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 6:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 7:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 8:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    case 9:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN;
                        break;
                    default:
                        snakeMoveTimerMax -= SNAKE_SPEED_INCREASE_PER_FOOD_EATEN-speedAfterLongSnake;
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
            saveSystem.EncryptPrefsPositive(fruitsCount, fruitsEncrypted);
            saveSystem.EncryptPrefsPositive(jellyCount,jellyEncrypted);
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

            StartCoroutine(WaitSomeTimeAfterGameOver());

        }
        IEnumerator WaitSomeTimeAfterGameOver()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 1f;
            GameOverPanel.SetActive(true);
            PauseButton.SetActive(false);

            DisplayAd();
        }


        public void DisplayAd()
        {
            int a = Random.Range(0, 5);
            if (a == 4 || a == 3)
                interstitialAds.ShowAd();
        }

        public void GiveNewLifeToPlayer()
        {
            isInvincible = true;
            PauseButton.SetActive(true);
            if (PlayerPrefs.GetInt(controls) == 0)
                buttonControls.SetActive(true);
            isSnakeMoving = true;
            canWePressButton = true;
            currentTime = startTime;
            StartCoroutine(MakeTheSnakeInvinsibleForShortTime());
        }

        public void StartTimerForNewLife()
        {
            Time.timeScale = 1f;
            TimerText.gameObject.SetActive(true);
            TimerText.text = "";
            GameOverPanel.SetActive(false);
            isTimerActive = true;
        }
        private void Timer()
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                isTimerActive = false;
                TimerText.gameObject.SetActive(false);
                GiveNewLifeToPlayer();
            }
            System.TimeSpan time = System.TimeSpan.FromSeconds(currentTime);
            TimerText.text = time.Seconds.ToString();
        }

        private void InvinciblityTimer()
        {
            InvincibilityTimerGameObject.SetActive(true);
            if (time >= 0)
            {
                time -= Time.deltaTime;
                invincibleTimerSlider.value = time;
            }
            else
            {
                stopInvinciblityTimer = true;
            }
        }

        private IEnumerator MakeTheSnakeInvinsibleForShortTime()
        {
            canSpawnAnotherInvincibleApple = false;
            isInvincible = true;
            InvincibilityTimerGameObject.SetActive(false);
            invincibleEffect.Play();
            
            CheckIfFirstTimeInvincibilityAppleEaten();

            yield return new WaitForSeconds(invincibilityTime);
            if(invincibleAppleEated)
            {
                float a = time;
                yield return new WaitForSeconds(a); 
                invincibleAppleEated = false;
            }

            invincibleEffect.Stop();
            isInvincible = false;
            yield return new WaitForSeconds(1.2f);
            InvincibilityTimerGameObject.SetActive(false);
            invincibleTimerSlider.value = invincibilityTime;
            time = invincibilityTime;
            stopInvinciblityTimer = false;
            canSpawnAnotherInvincibleApple = true;
        }
        public void CheckIfFirstTimeInvincibilityAppleEaten()
        {
            
        }

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
                    else if (Distance.y < -jumpRange)
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
            }

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

