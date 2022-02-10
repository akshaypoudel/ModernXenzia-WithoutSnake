using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu",menuName ="Scriptable Objects/New Shop Item",order =1)]
public class ShopItemScriptableObjects : ScriptableObject
{
    public string Title;
    public string Description;
    public Sprite bgImage;
    public int fruitsValue;
    public int JellyCost;
}
