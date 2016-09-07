//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Interface to native EnfluxVR plugin
//
//======================================================================

using System.Text;
using System.Runtime.InteropServices;
using EnflxStructs;

internal static class EnfluxVRSuit {

    public static int MESSAGESIZE = 256;
    private const string dllName = "ModuleInterface";
    private delegate void ScanCallbackDel(scandata scanresult);
    private delegate void StreamCallbackDel(streamdata streamresult);
    private delegate void MessageCallbackDel(sysmsg msgresult);

    private static class EVRSUIT_0_0_1
    {
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void scanPortNames(StringBuilder returnBuffer);        

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int attachPort(StringBuilder port, StringBuilder returnBuffer, 
            ScanCallbackDel scb, MessageCallbackDel mcb);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int detachPort(StringBuilder returnBuffer);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int connectDevices(StringBuilder devices, int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int disconnectDevices(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int performCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int finishCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int streamRealTime(int numdevices, bool record);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopRealTime(int numdevices);
    }

    public static int connect(StringBuilder devices, int numdevices)
    {   
        return EVRSUIT_0_0_1.connectDevices(devices, numdevices);
    }

    public static int disconnect(int numdevices)
    {
        return EVRSUIT_0_0_1.disconnectDevices(numdevices);
    }

    public static int performCalibration(int numdevices)
    {
        return EVRSUIT_0_0_1.performCalibration(numdevices);
    }

    public static int finishCalibration(int numdevices)
    {
        return EVRSUIT_0_0_1.finishCalibration(numdevices);
    }

    public static int streamRealTime(int numdevices, bool record)
    {
        return EVRSUIT_0_0_1.streamRealTime(numdevices, record);
    }

    public static int stopRealTime(int numdevices)
    {
        return EVRSUIT_0_0_1.stopRealTime(numdevices);
    }

    public static void startScanPorts(StringBuilder returnBuffer)
    {
        //gets any avaiable COM ports on PC
        EVRSUIT_0_0_1.scanPortNames(returnBuffer);
    }    

    public static int attachSelectedPort(StringBuilder port,
        StringBuilder returnBuffer,
        IOperationCallbacks ocb)
    {
        //attach to a selected COM port, if BlueGiga port then scans for BLE
        return EVRSUIT_0_0_1.attachPort(port, returnBuffer,
            new ScanCallbackDel(ocb.scanCallback),
            new MessageCallbackDel(ocb.messageCallback));
    }

    public static int detachPort(StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_1.detachPort(returnBuffer);
    }

    //Callbacks to support device operations
    public interface IOperationCallbacks
    {
        //results from scanning such as address, name, and rssi
        void scanCallback(scandata scanresult);       
        //system messages such as state or errors
        void messageCallback(sysmsg msgresult);
    }
}
