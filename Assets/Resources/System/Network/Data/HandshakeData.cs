using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HandshakeData
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SendUserData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string IP;
        public int tempPort;

    }

}
