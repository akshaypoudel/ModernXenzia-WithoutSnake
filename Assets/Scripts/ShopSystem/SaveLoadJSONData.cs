using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class SaveLoadJSONData
{
    private string persistentPath = "";
    public void SaveData(PlayerData playerData,string NameOfJsonFile)
    {
        //SetDataPath();   
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + NameOfJsonFile;

        string savePath = persistentPath;

        string json = JsonUtility.ToJson(playerData);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }
    public void SavePlayerDataNumber(PlayerDataNumber playerData, string NameOfJsonFile)
    {
        //SetDataPath();   
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + NameOfJsonFile;

        string savePath = persistentPath;

        string json = JsonUtility.ToJson(playerData);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public PlayerData LoadData(string NameOfJsonFile)
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + NameOfJsonFile;
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        //Debug.Log(data.ToString());
        return data;
    }
    public PlayerDataNumber LoadPlayerDataNumber(string NameOfJsonFile)
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + NameOfJsonFile;
        PlayerDataNumber datalist;
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();
        datalist = JsonUtility.FromJson<PlayerDataNumber>(json);
        return datalist;
    }

}
