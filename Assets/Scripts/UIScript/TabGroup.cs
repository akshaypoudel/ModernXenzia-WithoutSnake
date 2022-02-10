using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> objectToSwipe;
    public List<GameObject> objectToAnimate;
    public List<GameObject> TabPanels;
    public Vector3 sizeOfObjectAfterAnimation;
    public Vector3 normalScaleOfObject;

    private void Start()
    {
        for(int i = 0;i<objectToAnimate.Count;i++)
        {
            if(objectToSwipe[i].activeSelf)
            {
                Animate(i);
                break;
            }
        }
    }

    public void Subscribe(TabButton button)
    {                                           
        if(tabButtons==null)
        {
            tabButtons = new List<TabButton>();
        }                      
        tabButtons.Add(button);
    }
    public void OnTabSelected(TabButton button)
    {
        int index = button.transform.GetSiblingIndex()-1;
        for(int position=0;position<objectToSwipe.Count; position++)
        {
            if(position==index)
            {
                Animate(position);
                TabPanels[position].SetActive(true);
                objectToSwipe[position].SetActive(true);
            }
            else
            {
                BackToNormal(position);
                TabPanels[position].SetActive(false);
                objectToSwipe[position].SetActive(false);
            }
        }
    }
    public void Animate(int positionOfElement)
    {     
        objectToAnimate[positionOfElement]
        .transform.localScale = sizeOfObjectAfterAnimation;
    }
    public void BackToNormal(int positionOfElement)
    {
        objectToAnimate[positionOfElement]
        .transform.localScale = normalScaleOfObject;
    }









}
