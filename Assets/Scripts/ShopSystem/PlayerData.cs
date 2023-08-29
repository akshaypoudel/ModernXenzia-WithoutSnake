using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int buttonNumber;
    public bool dataToSetActive;
    public bool dataToSetInactive;
    public bool removeFruitIcon;
   
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
    public string No; //button number
    public bool yieldd; //button to set active
    public bool NetworkBuild; //button to set inactive
    public bool TIO00_UGG; //variable for removing fruit icon after purchasing
    public string name; //name of object
}
