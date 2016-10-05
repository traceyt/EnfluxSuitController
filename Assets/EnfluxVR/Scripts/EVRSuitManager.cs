//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Managing state of EnfluxVR suit
//
//======================================================================

// If running a local server, define this flag.
// #define NO_APPLICATION


using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;


public class EVRSuitManager : MonoBehaviour
{
    public List<string> ports { get { return _ports; } }
    private List<string> _ports = new List<string>();
    public List<string> connectedDevices;
    private ConnectionState _operatingState;
    public ConnectionState operatingState {
        get { return _operatingState; }
        private set { _operatingState = value;
            foreach (var button in FindObjectsOfType<ConnectionStateButton>()) //todo events
                button.SetState(value);
        }
    }

    private ServerState serverState = ServerState.CLOSED;
    private string host = "127.0.0.1";
    private Int32 port = 12900;
    private NetworkStream stream;
    private StreamWriter streamWriter;
    private BinaryReader streamReader;
    private TcpClient client;
    private System.Diagnostics.Process serverProcess;
    private IAddOrientationAngles orientationAngles;
    private ScanResultsUpdater scanUpdater;
    private CallbackQueue callbacks = new CallbackQueue();

    public enum ConnectionState
    {
        NONE,
        ATTACHED,
        DETACHED,
        CONNECTED,
        DISCONNECTED,
        CALIBRATING,
        STREAMING
    };

    private enum ServerState
    {
        CLOSED,
        SET,
        STARTED
    };

    public interface IAddOrientationAngles
    {
        void addAngles(float[] angles);
        void setMode(int mode);
        string getMode();
    }

    void Start()
    {
        RefreshPorts();

        // required so that when socket server launches, does not pause Unity
        Application.runInBackground = true;
        StartCoroutine(launchServer());
        orientationAngles = GameObject.Find("[EnfluxVRHumanoid]")
            .GetComponent<EVRHumanoidLimbMap>();

        scanUpdater = GameObject.Find("ScanResultsUpdater").GetComponent<ScanResultsUpdater>();
        EnfluxVRSuit.registerResponseCallbacks(callbacks);
        operatingState = ConnectionState.NONE;
    }

    public void RefreshPorts()
    {
        StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);
        EnfluxVRSuit.scanPortNames(returnBuffer);
        _ports.Clear();
        if (returnBuffer != null)
        {
            _ports.Add(returnBuffer.ToString());
        }
    }

    void OnApplicationQuit()
    {
        //Just in case some steps were skipped
        Debug.Log("Making sure things are closed down");

        // Make sure sensors are disconnected, port is detached,
        // and client connection closed
        if(operatingState != ConnectionState.NONE ||
            operatingState != ConnectionState.DETACHED ||
            operatingState != ConnectionState.DISCONNECTED)
        {
            if(operatingState == ConnectionState.STREAMING)
            {
                disableAnimate();
            }

            disconnectEnflux();
        }

        if (operatingState != ConnectionState.NONE && operatingState
            != ConnectionState.DETACHED)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);
            EnfluxVRSuit.detachPort(returnBuffer);
        }

        if (client != null)
            client.Close();

        if (serverState != ServerState.CLOSED)
        {
#if !NO_APPLICATION
            serverProcess.Kill();
#endif
        }
        EnfluxVRSuit.unregisterResponseCallbacks();

    }

    /*
     * Uses coroutine in order to not block main thread
     * Launches Enflux Java socket server
     */
    private IEnumerator launchServer()
    {
        serverProcess = new System.Diagnostics.Process();
        string filePath = Path.Combine(Application.streamingAssetsPath + "/Sensors/",
            "EVRModuleServer.jar");
        serverProcess.StartInfo.FileName = filePath;


#if !NO_APPLICATION
        if (serverProcess.Start())
        {
          Debug.Log("Socket server started");
        }
#endif
        //Delay in order to give the server time
        //to launch
        yield return new WaitForSeconds(3);
        client = new TcpClient(host, port);
        stream = client.GetStream();
        streamWriter = new StreamWriter(stream);
        streamReader = new BinaryReader(stream, Encoding.UTF8);
        serverState = ServerState.STARTED;
    }

    /*
     * INPUT: Friendly name of COM port where dongle is located
     * OUTPUT: None
     *
     * SUMMARY: Checks for correct operational state,
     * gets COMX location from input, and attempts to
     * attach to the port
     *
     * ATTACH SUCCEED: update operational state, start processing scan results
     * ATTACH FAIL: state unchanged, prints error message to debug log
     *
     * RETURNS: NONE
     */
    public void attachPort(string friendlyName)
    {
        if(operatingState == ConnectionState.NONE || operatingState == ConnectionState.DETACHED)
        {
            StringBuilder comName = EnfluxUtils.parseFriendlyName(friendlyName);

            if (comName != null)
            {
                StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

                if (EnfluxVRSuit.attachSelectedPort(comName,
                    returnBuffer) < 1)
                {
                    operatingState = ConnectionState.ATTACHED;
                    scanUpdater.StartScanning();
                }
                else
                {
                    Debug.Log(returnBuffer);
                }
            }
        }else
        {
            Debug.Log("Unable to attach, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    //api expects input of all address to connect to, seperated by comma
    //example format: XX:XX:XX:XX:XX:XX,YY:YY:YY:YY:YY:YY
    public void connectEnflux(List<string> devices)
    {
        StringBuilder apiArg = new StringBuilder();
        for (int device = 0; device < devices.Count; device++)
        {
            apiArg.Append(devices[device]);
            if (device < (devices.Count - 1))
            {
                apiArg.Append(",");
            }
        }

        if(operatingState == ConnectionState.ATTACHED ||
            operatingState == ConnectionState.DISCONNECTED)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            if (EnfluxVRSuit.connect(apiArg, devices.Count, returnBuffer) < 1)
            {
                connectedDevices = devices;
                operatingState = ConnectionState.CONNECTED;
                Debug.Log("Devices connecting");
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }else
        {
            Debug.Log("Unable to connect to devices, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void disconnectEnflux()
    {
        if(operatingState == ConnectionState.CONNECTED)
        {

            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            if (EnfluxVRSuit.disconnect(connectedDevices.Count, returnBuffer) < 1)
            {
                Debug.Log("Devices disconnecting");
                operatingState = ConnectionState.DISCONNECTED;
                //scanUpdater.StartScanning();
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }
        if (operatingState == ConnectionState.DISCONNECTED)
        {
            // Already disconnected.
        }

        else
        {
            Debug.Log("Unable to disconnect, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void calibrateDevices()
    {
        if(operatingState == ConnectionState.CONNECTED)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            if (EnfluxVRSuit.performCalibration(connectedDevices.Count, returnBuffer) < 1)
            {
                operatingState = ConnectionState.CALIBRATING;
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }else
        {
            Debug.Log("Unable to calibrate, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void finishCalibration()
    {
        if(operatingState == ConnectionState.CALIBRATING)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            if (EnfluxVRSuit.finishCalibration(connectedDevices.Count, returnBuffer) < 1)
            {
                operatingState = ConnectionState.CONNECTED;
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }else
        {
            Debug.Log("Unable to stop calibration, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void enableAnimate(bool record)
    {

        if (operatingState == ConnectionState.CONNECTED)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            if (EnfluxVRSuit.streamRealTime(connectedDevices.Count,
                record,
                returnBuffer) < 1)
            {
                operatingState = ConnectionState.STREAMING;
                StartCoroutine(readAngles());
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }else
        {
            Debug.Log("Unable to stream, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    //determine mode then start reading
    private IEnumerator readAngles()
    {

        while (serverState != ServerState.SET)
        {
            if (setAnimationMode() < 1)
            {
                Debug.Log("Mode set");
                serverState = ServerState.SET;
                clearStream();
            }
            else
            {
                // Spin until the stream is initialized
                yield return null;
            }
        }

        //tell server to send data
        string mode = orientationAngles.getMode();
        streamWriter.WriteLine(mode);
        streamWriter.Flush();
        int formattedAnglesLength = 20;

        while (operatingState == ConnectionState.STREAMING)
        {
            //todo: this is a waste of an operation, but
            //server expects a string
            streamWriter.WriteLine("send");
            streamWriter.Flush();
            int multiplier = connectedDevices.Count;
            float[] result = new float[formattedAnglesLength * multiplier];
            if (stream.DataAvailable)
            {
                for (int i = 0; i < formattedAnglesLength * multiplier; i++)
                {
                    long v = System.Net.IPAddress.NetworkToHostOrder(streamReader.ReadInt64());
                    double angle = BitConverter.Int64BitsToDouble(v);
                    float fangle = Convert.ToSingle(angle);
                    result[i] = fangle;
                }
                orientationAngles.addAngles(result);
            }
            yield return null;
        }
    }

    private int setAnimationMode()
    {
        //read and set mode
        streamWriter.WriteLine("requestmode");
        streamWriter.Flush();
        int mode = 0;
        for(int i = 0; i < 2; i++)
        {
            mode = streamReader.Read();
        }
        // Animation mode 0 means the stream is not yet initialized
        if (mode == 0) return 1;
        orientationAngles.setMode(mode);
        Debug.Log("Mode: " + mode);
        return 0;
    }

    public void disableAnimate()
    {
        if(operatingState == ConnectionState.STREAMING)
        {
            StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

            //read and set mode
            streamWriter.WriteLine("stop");
            streamWriter.Flush();
            if (EnfluxVRSuit.stopRealTime(connectedDevices.Count, returnBuffer) < 1)
            {
                operatingState = ConnectionState.CONNECTED;
                clearStream();
                //stop animation mode
                orientationAngles.setMode(0);
                serverState = ServerState.STARTED;
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }
        else
        {
            Debug.Log("Unable to stop stream, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    private void clearStream()
    {
        while (stream.DataAvailable)
        {
            System.Net.IPAddress.NetworkToHostOrder(streamReader.ReadInt64());
        }
    }

    /*
     * INPUT: None
     * OUTPUT: None
     *
     * SUMMARY: Checks for correct operational state, then attempts
     * to detach from COM port
     *
     * ATTACH SUCCEED: update operational state, stop scanning for devices
     * ATTACH FAIL: state unchanged, prints error message to debug log
     *
     * RETURNS: NONE
     */
    public void detachPort()
    {
        StringBuilder returnBuffer = new StringBuilder(EnfluxVRSuit.MESSAGESIZE);

        if(operatingState == ConnectionState.ATTACHED ||
            operatingState == ConnectionState.DISCONNECTED ||
            operatingState == ConnectionState.CONNECTED)
        {
            if (EnfluxVRSuit.detachPort(returnBuffer) < 1)
            {
                operatingState = ConnectionState.DETACHED;
                Debug.Log(returnBuffer);
                scanUpdater.StopScanning();
            }
            else
            {
                Debug.Log(returnBuffer);
            }
        }
        else
        {
            Debug.Log("Unable to detach from port, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }
}
