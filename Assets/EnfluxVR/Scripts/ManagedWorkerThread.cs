//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Retrieve data and messages from native thread. Use of this
// prevents conflict with Unity main thread
//
//======================================================================

using UnityEngine;
using System.Collections;
using System.Text;
using System.Threading;
using EnflxStructs;

public class ManagedWorkerThread {
   
    private bool _checking;

    private Thread mwThread;

    public void startMwThread()
    {
        mwThread = new Thread(this.getResponses);
        _checking = true;
        mwThread.Start();
    }

    public void stopMwThread()
    {
        _checking = false;
        mwThread.Abort();
    }

    public void getResponses()
    {
        EnfluxVRSuit.registerResponseCallbacks(new AttachedPort());

        while (_checking)
        {            
            EnfluxVRSuit.getResponses();
            Thread.Sleep(20);
        }

    //    EnfluxVRSuit.unregisterResponseCallbacks();
    }

    private class AttachedPort : EnfluxVRSuit.IOperationCallbacks
    {
        public void messageCallback(sysmsg msgresult)
        {
            if(msgresult.msg != null)
                Debug.Log(msgresult.msg);
        }

        public void streamCallback(streamdata streamresult)
        {
            Debug.Log(streamresult.data);
        }

        public void scanCallback(scandata scanresult)
        {
            ThreadDispatch.instance.AddScanItem(scanresult);
        }
    }
}
