using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SnakeGame
{

    public class GameAssets : MonoBehaviour
    {
        public static GameAssets i;

        private void Awake()
        {
            i = this;
        }

        public Sprite snakeHeadSprite;
        public Material[] skyboxes;
        public GameObject[] foodSprite;
        public GameObject snakeBodySprite;
        public SoundAudioClip[] soundAudioClipArray;
        public Material[] foodMat;
        public int indexOfBGMaterial;
        public GameObject InvincibleApple;
        public ParticleSystem snakeEatInvincibleApple;
        
        [Serializable]
        public class SoundAudioClip
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;
        }

    }
}
