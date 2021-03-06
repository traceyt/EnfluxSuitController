﻿//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Display BLED112 Dongle and COM port
//
//======================================================================

using System;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_COM : MonoBehaviour {

    private EVRSuitManager _manager;
    private Dropdown self;

    void Update()
    {
        // This is like a start function guaranteed to execute after EVRSuitManager
        if (self != null) return;
        //get object that has list of ports
        _manager =  GameObject.Find("EVRSuitManager").GetComponent<EVRSuitManager>();
        self = gameObject.GetComponent<Dropdown>();
        RefreshPorts();
    }

    private void RefreshPorts()
    {
        _manager.RefreshPorts();
        //get UI element for displaying port
        self.ClearOptions();

        //display info
        foreach (string port in _manager.ports)
        {
            self.options.Add(new Dropdown.OptionData(port));
        }
        //refresh view
        gameObject.GetComponent<Dropdown>().RefreshShownValue();
    }

    public void attachSelected()
    {
        string item = self.options[self.value].text;
        _manager.attachPort(item);
    }

    public void detachSelected()
    {
        _manager.detachPort();
    }
}
