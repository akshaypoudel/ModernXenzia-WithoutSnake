using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SpawnFood
    {
        private Vector2 foodGridPos;
        private int width, height;
        public static int randomPos;
        public GameObject foodGameObject;
        private FoodEatingAnimation anim;                                                                                                    
        private SnakeHandler snakeHandler;
        

        //constructor
        public SpawnFood(int _width, int _height)
        {
            this.width = _width;
            this.height = _height;
        }

        public void Setup(SnakeHandler snakeHandler)
        {
            this.snakeHandler = snakeHandler;
            SpawnApple();
        }

        public void SpawnApple()
        {    
            if(snakeHandler.isObstacleMode==true)
            {
                do
                {
                    do
                    {
                        foodGridPos = new Vector2(Random.Range(1, width - 1), Random.Range(1, height - 1));
                    } while (!PreventSpawnOverlap(foodGridPos));
                } while (snakeHandler.GetFullSnakeGridPosList().IndexOf(foodGridPos) != -1);
            }
            else if(snakeHandler.isObstacleMode==false)
            {
                do
                {
                    foodGridPos = new Vector2(Random.Range(1, width - 1), Random.Range(1, height - 1));

                } while (snakeHandler.GetFullSnakeGridPosList().IndexOf(foodGridPos) != -1);
            }
           
            randomPos = Random.Range(0, GameAssets.i.foodSprite.Length);
            FoodEatingAnimation.ReturnRandomPos(randomPos);
            foodGameObject = GameObject.Instantiate(GameAssets.i.foodSprite[randomPos]);
            
            foodGameObject.transform.position = new Vector3(foodGridPos.x,foodGridPos.y);
        }

        bool PreventSpawnOverlap(Vector2 spawnPos)
        {
            snakeHandler.colliders = Physics2D.OverlapCircleAll(spawnPos, snakeHandler.overlapSphereRadius);
            foreach (Collider2D col in snakeHandler.colliders)
            {
                if (col.tag == "obstacles")
                {
                    return false;
                }
            }
            return true;
        }
        public bool DoesSnakeAteFood(Vector2 SnakeGridPos)
        {
            if (SnakeGridPos == foodGridPos)
            {
                if(foodGameObject.tag=="Jelly")
                {
                    SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                    anim = GameObject.FindObjectOfType(typeof(FoodEatingAnimation)) as FoodEatingAnimation;
                    anim.StartFoodMove(foodGridPos,"Jelly", () =>
                    {
                        IncreaseScoreAfterFoodReach("Jelly");
                    });
                    Object.Destroy(foodGameObject);
                    SpawnApple();
                    return true;
                }
                else
                {
                    SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                    anim = GameObject.FindObjectOfType(typeof(FoodEatingAnimation)) as FoodEatingAnimation;
                    anim.StartFoodMove(foodGridPos,"Fruits", () =>
                    {
                        IncreaseScoreAfterFoodReach("Fruits");
                    });
                    Object.Destroy(foodGameObject);
                    SpawnApple();
                    return true;
                }

            }
            else
                return false;
        }
        public void IncreaseScoreAfterFoodReach(string nameOfConsumable)
        {
            snakeHandler.ScoreCounter(nameOfConsumable);
        }

        public Vector2 ValidateGridPositionForFreeMode(Vector2 snakePos)
        {
            if(snakePos.x < 0f)
                snakePos.x = width+1;
            if (snakePos.x > width+1)
                snakePos.x = 0;
            if(snakePos.y < 0)
                snakePos.y = height;
            if(snakePos.y> height)
                snakePos.y=0;

            return snakePos;
            
        }
        public Vector2 ValidateGridPositionForHardMode(Vector2 snakePos)
        {
            if (snakePos.x < 0f)
                snakePos.x = width-1;
            if (snakePos.x > width-1)
                snakePos.x = 0;
            if (snakePos.y < 1)
                snakePos.y = height-2;
            if (snakePos.y > height-2)
                snakePos.y = 1;

            return snakePos;
        }

    }
}
