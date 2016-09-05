//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Structs for callbacks fromEnflux native plugin
//
//======================================================================

using System.Runtime.InteropServices;

namespace EnflxStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct sysmsg
    {
        public string msg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct scandata
    {
        public string addr;
        public string rssi;
        public string name;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct streamdata
    {
        public string data;
    }
}

