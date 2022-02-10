using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityCipher;

namespace SnakeGame{
    [System.Serializable]
    public class PlayerPrefsSaveSystem
    {
        public void EncryptPrefsPositive(int Score,string password,string encryptedPrefs,string playerPrefs)
        {
            int tempVar;
            tempVar = Score;
            if (PlayerPrefs.HasKey(playerPrefs))
            {
                tempVar += PlayerPrefs.GetInt(playerPrefs);
                PlayerPrefs.SetInt(playerPrefs, tempVar);

                string encrypt = RijndaelEncryption.Encrypt(PlayerPrefs.GetInt(playerPrefs).ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }
            else
            {
                PlayerPrefs.SetInt(playerPrefs, Score);
                string encrypt = RijndaelEncryption.Encrypt(PlayerPrefs.GetInt(playerPrefs).ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }

        }
        public void DecryptPrefs(TMP_Text fruitsCount,string password,string encryptedPrefs,string playerPrefs)
        {
            if (PlayerPrefs.HasKey(encryptedPrefs))
            {
                int a = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(encryptedPrefs), password));
                fruitsCount.text = a.ToString();
            }
            else
            {
                if (PlayerPrefs.HasKey(playerPrefs))
                    fruitsCount.text = PlayerPrefs.GetInt(playerPrefs).ToString();
                else
                {
                    string a = "0";
                    fruitsCount.text = a;
                }
            }
        }
    
        public int ReturnDecryptedScore(string password,string encryptedPrefs,string playerPrefs)
        {
            if (PlayerPrefs.HasKey(encryptedPrefs))
            {
                int a = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(encryptedPrefs), password));
                return a;
            }
            else
            {
                if(PlayerPrefs.HasKey(playerPrefs))
                {
                    string encrypt = RijndaelEncryption.Encrypt(PlayerPrefs.GetInt(playerPrefs).ToString(),password);
                    PlayerPrefs.SetString(encryptedPrefs,encrypt);

                }
                else
                    PlayerPrefs.SetInt(playerPrefs,0);
                return 0;
            }
        }
        public void EncryptPrefsNegative(int Score,string password,string encryptedPrefs,string playerPrefs)
        {
            int tempVar;
            tempVar = Score;
            if (PlayerPrefs.HasKey(playerPrefs))
            {
                tempVar = PlayerPrefs.GetInt(playerPrefs) - tempVar;
                PlayerPrefs.SetInt(playerPrefs, tempVar);

                string encrypt = RijndaelEncryption.Encrypt(PlayerPrefs.GetInt(playerPrefs).ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }
            else
            {
                PlayerPrefs.SetInt(playerPrefs, Score);

                string encrypt = RijndaelEncryption.Encrypt(PlayerPrefs.GetInt(playerPrefs).ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }

        }
    
        
    }
}
