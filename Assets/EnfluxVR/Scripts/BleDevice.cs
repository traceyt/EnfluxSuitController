//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Enflux Modules Sensor model
//
//======================================================================

using EnflxStructs;
using System.Collections.Generic;

public class BleDevice
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
        { state.disconnected, "disconnected"},
        { state.idle, "idle" },
        { state.connecting, "connecting" },
        { state.disconnecting, "disconnecting" },
        { state.interval_changing, "interval_changing" },
        { state.starting_stream, "starting_stream" },
        { state.streaming, "streaming" },
        { state.ending_stream, "ending_stream" },
        { state.interval_request, "interval_request" },
        { state.error_connecting, "error_connecting" },
        { state.error_disconnecting, "error_disconnecting" },
        { state.error_streaming, "error_streaming" },
        { state.error_interval_change, "error_interval_change" },
        { state.error_interval_request, "error_interval_request" }

    };
    private string _name;
    public string name
    {   set { _name = value;}
        get { return _name;}
    }
    private string _rssi;
    public string rssi
    {   set { _rssi = value; }
        get { return _rssi;  }
    }
    private string _mac;
    public string mac
    {   set { _mac = value.ToUpper();}
        get { return _mac; }
    }

    public BleDevice(string mac, string rssi, string name)
    {
        _mac = mac.ToUpper();        
        _rssi = rssi;
        _name = name;
    }

    public override string ToString()
    {
        return "  " + _mac + "    " + _name + "    " + _rssi;
    }

    public BleDevice(scandata scan)
    {
        name = scan.name;
        rssi = scan.rssi;
        mac = scan.addr;
    }
    public BleDevice()
    {

    }
}

public class ConnectedDevice : BleDevice
{
    public ConnectedDevice(statusreport report)
    {
        name = stateString[(state)report.deviceState];
        rssi = "";
        mac = "Device" + report.deviceHandle;
    }
}