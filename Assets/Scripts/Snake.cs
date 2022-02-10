/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SnakeGame
{



    public class Snake : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
        [Header("Buttons")]
        #region Buttons
        public Button upButton, upButton1;
        public Button leftButton, rightButton;
        public Button downButton, downButton1;

        #endregion
        [Header("Variable References")]
        #region VariableReferences
        private Vector2Int snakePosition, snakeMoveDirection;
        public int gridPosX, gridPosY;
        // private GameObject SnakeBody;
        public Text text;
        public TMP_Text scoreText;
        private int Score = 0;
        private float snakeMoveTimer, snakeMoveTimerMax = 0.6f;
        private SpawnFood spawnFood;
        public TMP_Text HighScoreText;
        public GameObject GameOverPanel;
        public GameObject PauseButton, TouchControl, ScoreText;
        private int snakeBodySize;
        private List<Vector2Int> snakeMovePosList = new List<Vector2Int>();
        private List<SnakeBodyPart> snakeBodyPartsList = new List<SnakeBodyPart>();
        public float snakeSpeed = 0.05f;
        public Transform foodDestination;
        [SerializeField] private float foodTravelSpeed = 1f;
        private Direction gridMoveDirection;
        private Vector2Int gridPosition;
        private float gridMoveTimer;
        private float gridMoveTimerMax;
        private SpawnFood levelGrid;
        // private int snakeBodySize;
        private List<SnakeMovePosition> snakeMovePositionList;
        private List<SnakeBodyPart> snakeBodyPartList;

        #endregion
        private void Awake()
        {
            snakePosition = new Vector2Int(10, 10);
            snakeMoveTimer = snakeMoveTimerMax;
            snakeBodySize = 1;
            Debug.Log("HighScore=" + PlayerPrefs.GetInt("HighScore"));
            if (PlayerPrefs.GetInt("HighScore") == 0)
            {
                PlayerPrefs.SetInt("HighScore", 0);
            }
            UIButtonClick();
        }
        private void Update()
        {
            HandleInput(); //Function for handling input
            HandleGridMovement();  //function for handling snake position
        }
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpButton();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                DownButton();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftButton();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightButton();
            }
        }

        //BUTTONS CLICK
        private void UIButtonClick()
        {
            upButton.onClick.AddListener(UpButton);
            upButton1.onClick.AddListener(UpButton);
            downButton.onClick.AddListener(DownButton);
            downButton1.onClick.AddListener(DownButton);
            leftButton.onClick.AddListener(LeftButton);
            rightButton.onClick.AddListener(RightButton);
        }
        private void UpButton()
        {
            if (snakeMoveDirection.y != -1)
            {
                snakeMoveDirection.x = 0;
                snakeMoveDirection.y = 1;
            }
        }
        private void DownButton()
        {
            if (snakeMoveDirection.y != 1)
            {
                snakeMoveDirection.x = 0;
                snakeMoveDirection.y = -1;
            }
        }
        private void RightButton()
        {
            if (snakeMoveDirection.x != -1)
            {
                snakeMoveDirection.x = 1;
                snakeMoveDirection.y = 0;
            }
        }
        private void LeftButton()
        {
            if (snakeMoveDirection.x != 1)
            {
                snakeMoveDirection.x = -1;
                snakeMoveDirection.y = 0;
            }
        }

        public void Setup(SpawnFood levelGrid)
        {
            this.levelGrid = levelGrid;
        }

 /*       private void Awake()
        {
            gridPosition = new Vector2Int(10, 10);
            gridMoveTimerMax = .2f;
            gridMoveTimer = gridMoveTimerMax;
            gridMoveDirection = Direction.Right;

            snakeMovePositionList = new List<SnakeMovePosition>();
            snakeBodySize = 0;

            snakeBodyPartList = new List<SnakeBodyPart>();
        }


        private void HandleGridMovement()
        {
            gridMoveTimer += Time.deltaTime;
            if (gridMoveTimer >= gridMoveTimerMax)
            {
                gridMoveTimer -= gridMoveTimerMax;

                SnakeMovePosition previousSnakeMovePosition = null;
                if (snakeMovePositionList.Count > 0)
                {
                    previousSnakeMovePosition = snakeMovePositionList[0];
                }

                SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
                snakeMovePositionList.Insert(0, snakeMovePosition);

                Vector2Int gridMoveDirectionVector;
                switch (gridMoveDirection)
                {
                    default:
                    case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                    case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                    case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                    case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
                }

                gridPosition += gridMoveDirectionVector;

                bool snakeAteFood = levelGrid.DoesSnakeAteFood(gridPosition);
                if (snakeAteFood)
                {
                        foodAnimation();
                        SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                        snakeBodySize++;
                        Score += 10;
                        text.text = Score.ToString();
                        //SNAKE SPEED AFTER EVERY FOOD EATEN   
                        if (snakeBodySize >= 2 && snakeBodySize <= 11)
                        {
                            snakeMoveTimerMax -= snakeSpeed;
                            Debug.Log("Speed of Snake is: " + snakeMoveTimerMax);
                        }
                        CreateSnakeBodyPart();
                }

                if (snakeMovePositionList.Count >= snakeBodySize + 1)
                {
                    snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
                }

                /*for (int i = 0; i < snakeMovePositionList.Count; i++) {
                    Vector2Int snakeMovePosition = snakeMovePositionList[i];
                    World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
                    FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
                }

                transform.position = new Vector3(gridPosition.x, gridPosition.y);
                transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

                UpdateSnakeBodyParts();
            }
        }

        private void CreateSnakeBodyPart()
        {
            snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
        }
        private void foodAnimation()
        {
            //GameObject demoFood = Instantiate(GameAssets.i.foodSprite[spawnFood.randomPos]);
            demoFood.transform.position = new Vector3(transform.position.x,
                                                      transform.position.y);
            for (int i = 0; demoFood.transform.position != foodDestination.position; i++)
            {
                demoFood.transform.position = Vector3.MoveTowards
                                            (demoFood.transform.position,
                                             foodDestination.position,
                                             foodTravelSpeed * Time.deltaTime);

            }
        }

        private void UpdateSnakeBodyParts()
        {
            for (int i = 0; i < snakeBodyPartList.Count; i++)
            {
                snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            }
        }


        private float GetAngleFromVector(Vector2Int dir)
        {
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        // Return the full list of positions occupied by the snake: Head + Body
        public List<Vector2Int> GetFullSnakeGridPositionList()
        {
            List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
            foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
            {
                gridPositionList.Add(snakeMovePosition.GetGridPosition());
            }
            return gridPositionList;
        }





        private class SnakeBodyPart
        {

            private SnakeMovePosition snakeMovePosition;
            private Transform transform1;

            public SnakeBodyPart(int bodyIndex)
            {
                //GameObject SnakeBody = Instantiate(GameAssets.i.snakeBodySprite);
                //transform1 = SnakeBody.transform;
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



        /*
         * Handles one Move Position from the Snake
         * 
        private class SnakeMovePosition
        {

            private SnakeMovePosition previousSnakeMovePosition;
            private Vector2Int gridPosition;
            private Direction direction;

            public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
            {
                this.previousSnakeMovePosition = previousSnakeMovePosition;
                this.gridPosition = gridPosition;
                this.direction = direction;
            }

            public Vector2Int GetGridPosition()
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
*/

