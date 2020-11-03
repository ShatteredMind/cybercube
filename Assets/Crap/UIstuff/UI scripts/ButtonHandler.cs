﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{    
   TabSelector tab;

    private void Start()
    {
        tab = GameObject.Find("Canvas").GetComponent<TabSelector>();
    }

    public void ClickOnButton()
    {
            //SoundM.PlaySound(0);
           tab.OpenTab(this.name);
        
    }       
}
