using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TabButton : MonoBehaviour,IPointerClickHandler
{
    public TabGroup tabGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    void Start()
    {
        tabGroup.Subscribe(this);     
    }

}
