using UnityEngine;
using TMPro;
using UnityCipher;

namespace SnakeGame{
    [System.Serializable]
    public class PlayerPrefsSaveSystem
    {
        private string password = "((%##@&&)*";

        public void EncryptPrefsPositive(int score, string encryptedPrefs)
        {
            int Score = score;
            int temp;
            if (PlayerPrefs.HasKey(encryptedPrefs))
            {
                temp = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(encryptedPrefs), password));

                Score += temp;

                string encrypt = RijndaelEncryption.Encrypt(Score.ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }
            else
            {
                string encrypt = RijndaelEncryption.Encrypt(Score.ToString(), password);

                PlayerPrefs.SetString(encryptedPrefs, encrypt);
            }
        }



        public void DecryptPrefs(TMP_Text fruitsCount, string Eprefs)
        {
            if (PlayerPrefs.HasKey(Eprefs))
            {
                int a = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(Eprefs), password));
                fruitsCount.text = a.ToString();
            }
            else
            {
                fruitsCount.text = "0";
            }
        }


        public int ReturnDecryptedScore(string Eprefs)
        {
            if (PlayerPrefs.HasKey(Eprefs))
            {
                int a = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(Eprefs), password));
                return a;
            }
            else
            {
                return 0;
            }
        }
        public void EncryptPrefsNegative(int Score, string Eprefs)
        {
            int tempVar;
            if (Score < 0)
                tempVar = 0;
            else
                tempVar = Score;
            if (PlayerPrefs.HasKey(Eprefs))
            {
                int a = int.Parse(RijndaelEncryption.Decrypt(PlayerPrefs.GetString(Eprefs), password));

                tempVar = a - tempVar;

                string encrypt = RijndaelEncryption.Encrypt(tempVar.ToString(), password);

                PlayerPrefs.SetString(Eprefs, encrypt);
            }
            else
            {
                string encrypt = RijndaelEncryption.Encrypt(Score.ToString(), password);

                PlayerPrefs.SetString(Eprefs, encrypt);
            }

        }

    }
}
