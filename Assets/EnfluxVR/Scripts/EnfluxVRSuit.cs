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
    private delegate void StatusCallbackDel(statusreport statusresult);

    private static class EVRSUIT_0_0_2
    {
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int attachPort(StringBuilder port);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int registerResponseCallbacks(ScanCallbackDel scb, 
            MessageCallbackDel mcb, StreamCallbackDel strcb, StatusCallbackDel srcb);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int unregisterResponseCallbacks();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int detachPort();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int connectDevices(StringBuilder devices,
            int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int disconnectDevices(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int performCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int finishCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int streamRealTime(int numdevices, 
            bool record);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopRealTime(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void scanPortNames(StringBuilder portNames);
    }

    public static void registerResponseCallbacks(IOperationCallbacks ocb)
    {
        EVRSUIT_0_0_2.registerResponseCallbacks(new ScanCallbackDel(ocb.scanCallback),
            new MessageCallbackDel(ocb.messageCallback),
            new StreamCallbackDel(ocb.streamCallback),
            new StatusCallbackDel(ocb.statusCallback));
    }

    public static void unregisterResponseCallbacks()
    {
        EVRSUIT_0_0_2.unregisterResponseCallbacks();
    }

    //public static void getResponses()
    //{
    //    EVRSUIT_0_0_2.getResponses();
    //}

    public static int connect(StringBuilder devices, int numdevices, StringBuilder returnBuffer)
    {   
        return EVRSUIT_0_0_2.connectDevices(devices, numdevices);
    }

    public static int disconnect(int numdevices, StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.disconnectDevices(numdevices);
    }

    public static int performCalibration(int numdevices, StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.performCalibration(numdevices);
    }

    public static int finishCalibration(int numdevices, StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.finishCalibration(numdevices);
    }

    public static int streamRealTime(int numdevices, bool record, StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.streamRealTime(numdevices, record);
    }

    public static int stopRealTime(int numdevices, StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.stopRealTime(numdevices);
    }

    public static void scanPortNames(StringBuilder ports)
    {
        EVRSUIT_0_0_2.scanPortNames(ports);
    }

    public static int attachSelectedPort(StringBuilder port,
        StringBuilder returnBuffer)
    {
        //attach to a selected COM port, if BlueGiga port then scans for BLE
        return EVRSUIT_0_0_2.attachPort(port);
    }

    public static int detachPort(StringBuilder returnBuffer)
    {
        return EVRSUIT_0_0_2.detachPort();
    }

    //Callbacks to support device operations
    public interface IOperationCallbacks
    {
        void statusCallback(statusreport report);

        //results from scanning such as address, name, and rssi
        void scanCallback(scandata scanresult);
        //streaming data
        void streamCallback(streamdata streamresult);
        //system messages such as state or errors
        void messageCallback(sysmsg msgresult);
    }
}
