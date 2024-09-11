using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ResponseServerData
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ResData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string serverIP;
        public int toClientPort;
        public int serverPlayerID;
    }
}
