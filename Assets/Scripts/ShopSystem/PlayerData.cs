using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int buttonNumber;
    public bool dataToSetActive;
    public bool dataToSetInactive;
   
    public string nameOfUnlockedObject;
}

[System.Serializable]
public class PlayerDataNumber
{
    public List<PlayerDataEncrypted> NetworkingCommand = new List<PlayerDataEncrypted>();
}

[System.Serializable]
public class PlayerDataEncrypted
{
    public string No;
    public bool yieldd;
    public bool NetworkBuild;
    public string name;
}
