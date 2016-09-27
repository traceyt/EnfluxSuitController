//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Scanning for Bluegiga BLED112 dongle
//
//======================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;
using System.Text;

public class ScanResultsUpdater : MonoBehaviour {

    private state status;
    private IScanUpdate updateView;

    public Dictionary<int, string> connectedDeviceMac = new Dictionary<int, string>();

    private enum state
    {
        state_updating,
        state_notupdating
    };

    // Use this for initialization
    void Start () {
        status = state.state_notupdating;
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
                updateView.postUpdate(new BleDevice(nextscan));
            }
            statusreport nextStatus;
            while (CallbackQueue.StatusQueue.TryDequeue(out nextStatus))
            {
                if(nextStatus.deviceState == (int)ConnectedDevice.state.interval_changing &&
                    !connectedDeviceMac.ContainsKey(nextStatus.deviceHandle))
                {
                    StringBuilder mac = new StringBuilder();
                    int bleDevice;
                    EnfluxVRSuit.getDeviceStatus(nextStatus.deviceHandle, mac, out bleDevice);
                    connectedDeviceMac.Add(nextStatus.deviceHandle, mac.ToString());

                }
                if (connectedDeviceMac.ContainsKey(nextStatus.deviceHandle))
                {
                    updateView.postUpdate(new ConnectedDevice(
                        nextStatus, connectedDeviceMac[nextStatus.deviceHandle]));

                }
                else
                {
                    updateView.postUpdate(new ConnectedDevice(nextStatus));
                }
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
        void postUpdate(SensorDevice device);
    }
}
