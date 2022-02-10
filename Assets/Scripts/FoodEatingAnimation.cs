using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class FoodEatingAnimation : MonoBehaviour
    {
        public float speed = 1;
        public Transform fruitTarget;
        public Transform jellyTarget;
        public Camera cam;
        private static int random;
        private GameObject food;
        public GameObject fruitGoesToDestination;
        public GameObject jellyGoesToDestination;

        public ParticleSystem snakeConsume;
        public ParticleSystem starIcon3d;
        void Start()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
        }
        public void StartFoodMove(Vector2 _initial,string nameOfConsumable, Action onComplete)
        {
            food = Instantiate(GameAssets.i.foodSprite[random]);
            SnakeBodyColorChanging();
            snakeConsume.Play();
            if(nameOfConsumable == "Jelly")
                StartCoroutine(MoveFood(food.transform,nameOfConsumable,_initial,jellyTarget.position,onComplete));
            else
                StartCoroutine(MoveFood(food.transform,nameOfConsumable, _initial, fruitTarget.position, onComplete));

        }
        public void SnakeBodyColorChanging()
        {
            GameAssets.i.snakeBodySprite.GetComponent<MeshRenderer>().material = GameAssets.i.foodMat[random];
        }
        IEnumerator MoveFood(Transform obj,string nameOfConsumable1, Vector2 startPos, Vector3 endPos,Action onComplete)
        {
            float time = 0;
            while (time < 1)
            {
                time += speed * Time.deltaTime;                                                                     
                obj.position = Vector3.Lerp(new Vector3(startPos.x,startPos.y,0),endPos,time);
                yield return new WaitForEndOfFrame();
            }
            onComplete.Invoke();
            //GameObject fruitEffect = new GameObject();
            //GameObject fruit1Effect = new GameObject();

            if (nameOfConsumable1=="Fruits")
            {
                Destroy(Instantiate(fruitGoesToDestination,obj.position,Quaternion.identity),5);
            }
            else if (nameOfConsumable1=="Jelly")
            {
                Destroy(Instantiate(jellyGoesToDestination, obj.position, Quaternion.identity),5);
                
            }

            Destroy(obj.gameObject);
            //Destroy(fruitEffect);
            //Destroy(fruitGoesToDestination);
            
            //Destroy(jellyGoesToDestination);
            //
        }
        public static void ReturnRandomPos(int random1)
        {
            random=random1;
        }
    }
}
