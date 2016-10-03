//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Enflux Modules Sensor model
//
//======================================================================

using EnflxStructs;
using System.Collections.Generic;
using System;

public abstract class SensorDevice
{
    public abstract bool isActive();

    public abstract string ID { get; }
}

public class BleDevice : SensorDevice
{ 
    public string name;
    public string rssi;
    public string mac;

    public float lastUpdate;

    public override bool isActive()
    {
        return (
            name == "Enfl" &&
            UnityEngine.Time.time - lastUpdate < 3);
    }

    public override string ID
    {
        get
        {
            return mac;
        }
    }

    public BleDevice(string _mac, string _rssi, string _name)
    {
        mac = mac.ToUpper();        
        rssi = _rssi;
        name = _name;
    }

    public override string ToString()
    {
        return "  " + ID + "    " + name + "    " + rssi;
    }

    public BleDevice(scandata scan)
    {
        name = scan.name;
        rssi = scan.rssi;
        mac = scan.addr;
        lastUpdate = UnityEngine.Time.time;
    }
    public BleDevice()
    {

    }
}

public class ConnectedDevice : SensorDevice
{
    public enum state
    {
        // The start configuration for a sensor device and the result fo a successful disconnect.
        disconnected,
        // No current operation.
        idle,
        // Attempting to establish a connection.
        connecting,
        // In the process of disconnecting.
        disconnecting,
        // Connected and is negotiating the connection interval.
        interval_changing,
        // Attempting to set the mode to stream data.
        starting_stream,
        //Streaming the sensor values. Used for animation and calibration.
        streaming,
        // Attempting to halt streaming data.
        ending_stream,
        // Requesting the connection interval.
        interval_request,
        // Attempted to connect, but failed.
        error_connecting = -(int)connecting,
        // Attempted to disconnect, but failed.
        error_disconnecting = -(int)disconnecting,
        // Failed to set streaming mode.
        error_streaming = -(int)starting_stream,
        // Connected, but failed to negotiate the connection interval.
        error_interval_change = -(int)interval_changing,
        // Interval request failed.
        error_interval_request = -(int)interval_request,
    };

    public static Dictionary<state, string> stateString = new Dictionary<state, string>()
    {
        { state.disconnected, "Disconnected"},
        { state.idle, "Idle" },
        { state.connecting, "Connecting" },
        { state.disconnecting, "Disconnecting" },
        { state.interval_changing, "Interval changing" },
        { state.starting_stream, "Starting stream" },
        { state.streaming, "Streaming" },
        { state.ending_stream, "Ending stream" },
        { state.interval_request, "Interval request" },
        { state.error_connecting, "Error connecting" },
        { state.error_disconnecting, "Error disconnecting" },
        { state.error_streaming, "Error streaming" },
        { state.error_interval_change, "Error interval change" },
        { state.error_interval_request, "Error interval request" }

    };

    state status;
    int devHandle;
    public string mac = "";
    private string v;

    public override bool isActive()
    {
        return status != state.disconnected;
    }

    public ConnectedDevice(statusreport report)
    {
        status = (state)report.deviceState;
        // Can be replaced by call to GetDeviceInfo
        mac = "";
        devHandle = report.deviceHandle;
    }

    public ConnectedDevice(statusreport report, string _mac) : this(report)
    {
        mac = _mac;
    }

    public override string ID
    {
        get { return devHandle.ToString(); }
    }

    public string getName()
    {
        if (mac == "") return "Device " + devHandle.ToString();
        else return mac;
    }

    public override string ToString()
    {
        return "  " + getName() + "    " + stateString[status];
    }
}