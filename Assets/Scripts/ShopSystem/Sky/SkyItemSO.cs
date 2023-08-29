using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SkyMenu", menuName = "Scriptable Objects/New Sky Item", order = 1)]
public class SkyItemSO : ScriptableObject
{
    public string Price;
    public string Name;
    public int baseCost;
}