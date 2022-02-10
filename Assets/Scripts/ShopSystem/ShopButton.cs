using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public GameObject Windows;
    public GameObject ShopPanel;
    public GameObject ShopPageArea;
    public GameObject[] otherItems;
    public TabGroup tabGroup;
    private int shopTabIndex = 0;

    public void ShopButtonActive()
    {
        Windows.SetActive(true);
        ShopPanel.SetActive(true);
        ShopPageArea.SetActive(true);
        DeActivateAllOtherPanelsAndTabs();
    }

    //Hide all other panels and page area when shop panel is active
    private void DeActivateAllOtherPanelsAndTabs()
    {
        for(int i=0; i<otherItems.Length;i++)
        {
            otherItems[i].SetActive(false);
        }
        for(int i=0; i<tabGroup.objectToAnimate.Count;i++)
            tabGroup.BackToNormal(i);
        tabGroup.Animate(shopTabIndex); //animate shop panel
    }
}
