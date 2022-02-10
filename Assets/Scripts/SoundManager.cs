using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public static class SoundManager
    {
        public static float volume;
        public enum Sound
        {
            SnakeMove,
            SnakeDie,
            FoodReachDestination,
            ButtonClick,
            ButtonOver,
            DirectionChanging,
            SnakeEat,
            SnakeEatJelly
        }

        public static void PlaySound(Sound sound)
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound),volume);

            //DestroyThis.destroy(soundGameObject,2);
            Object.Destroy(soundGameObject,2);
        }
        public static void Mute()
        {
            AudioListener.volume = 0;
        }
        public static void AudioResume()
        {
            AudioListener.volume = 1;
        }
        private static AudioClip GetAudioClip(Sound sound)
        {
            foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
            {
                if (soundAudioClip.sound == sound)
                {
                    return soundAudioClip.audioClip;
                }
            }
           // Debug.LogError("Sound " + sound + " not found!");
            return null;
        }
    }
}
