using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class HyperLinks : MonoBehaviour
{
    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
