//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Scanning for Bluegiga BLED112 dongle
//
//======================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;

public class ScanResultsUpdater : MonoBehaviour {

    private state status;
    private IScanUpdate updateView;
        
    private enum state
    {
        state_updating,
        state_notupdating
    };

    // Use this for initialization
    void Start () {
        status = state.state_notupdating;
        //scannedDevices = ThreadDispatch.instance.GetScanItems();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void StartScanning()
    {
        StartCoroutine(processDevices());
    }

    public void StopScanning()
    {
        StopAllCoroutines();
    }

    private IEnumerator processDevices()
    {
        while (true)
        {
            scandata nextscan;
            while (CallbackQueue.ScanQueue.TryDequeue(out nextscan))
            {
                if (nextscan.name == "Enfl")
                {
                    updateView.postUpdate(new BleDevice(nextscan));
                }
                Debug.Log(nextscan.addr);
            }
            statusreport nextStatus;
            while (CallbackQueue.StatusQueue.TryDequeue(out nextStatus))
            {
                updateView.postUpdate(new ConnectedDevice(nextStatus));
            }
            yield return null;
        }
    }

    public void setUpdateView(IScanUpdate view)
    {
        updateView = view;
    }

    public interface IScanUpdate
    {
        void postUpdate(BleDevice device);
    }
}
