using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "PlayMode", menuName = "Scriptable Objects/New Play Mode", order = 1)]
public class PlayModeSO : ScriptableObject
{
    public string Price;
    private Image PreviewImage;
    public int baseCost;
}
