//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Retrieve data and messages from native thread. Use of this
// prevents conflict with Unity main thread
//
//======================================================================

using UnityEngine;
using EnflxStructs;
using System;
using DisruptorUnity3d;
using System.Collections.Generic;

public class CallbackQueue : EnfluxVRSuit.IOperationCallbacks
{
    public static RingBuffer<scandata> ScanQueue = new RingBuffer<scandata>(32);
    public static RingBuffer<statusreport> StatusQueue = new RingBuffer<statusreport>(32);
    
    public static void _messageCallback(sysmsg msgresult)
    {
        if (msgresult.msg != null)
            Debug.Log(msgresult.msg);
        // Hacky implementation. TODO create callback.
        if (msgresult.msg == "Port closed")
        {
            Debug.Log("Shutdown Complete");
        }
    }

    public static void _streamCallback(streamdata streamresult)
    {
        Debug.Log(streamresult.data);
    }

    public static void _scanCallback(scandata scanresult)
    {
        ScanQueue.Enqueue(scanresult);
    }

    public static void _statusCallback(statusreport report)
    {
        StatusQueue.Enqueue(report);
    }

    public void statusCallback(statusreport report)
    {
        _statusCallback(report);
    }

    public void scanCallback(scandata scanresult)
    {
        _scanCallback(scanresult);
    }

    public void streamCallback(streamdata streamresult)
    {
        _streamCallback(streamresult);
    }

    public void messageCallback(sysmsg msgresult)
    {
        _messageCallback(msgresult);
    }
}