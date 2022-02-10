using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "StageMenu", menuName = "Scriptable Objects/New Stage Item", order = 1)]
public class StageItemSO : ScriptableObject
{                        
    public string Price;
    public string Name;
    private Image PreviewImage;
    public Material materialOfObject;
    public int baseCost;
}

