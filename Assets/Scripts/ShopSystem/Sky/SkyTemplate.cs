using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyTemplate : MonoBehaviour
{
    public TMP_Text TitleText;
    public TMP_Text UnlockedObjectText;
    [HideInInspector] public string NameOfSky;
    [HideInInspector] public Sprite BGSprite;
    [HideInInspector] public TMP_Text CostText;
}
