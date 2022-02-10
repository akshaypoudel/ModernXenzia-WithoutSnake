using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageTemplate : MonoBehaviour
{
    public TMP_Text TitleText;
    [HideInInspector] public string NameOfStage;
    [HideInInspector]public Sprite BGSprite;
    [HideInInspector]public TMP_Text CostText;
}
