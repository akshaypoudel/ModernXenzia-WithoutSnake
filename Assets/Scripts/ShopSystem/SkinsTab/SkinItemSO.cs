using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SkinMenu", menuName = "Scriptable Objects/New Skin Item", order = 1)]
public class SkinItemSO : ScriptableObject
{
    public string Price;
    public string Name;
    private Image PreviewImage;
    public Material materialOfObject;
    public int baseCost;
}


