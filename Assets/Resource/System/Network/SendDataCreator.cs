using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendDataCreator
{
    ////// c#では構造体内のアクセスが厳しくゲッターセッターをひとつづつ作るかクラスにするかする必要があるためとりあえず全public宣言

    /// <summary>
    /// データ送受信時に使うデータを全てまとめたやつ
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] sendByteData;
        public byte[] receivedByteData;
    }

    /// <summary>
    /// 毎フレーム送るべきデータ群構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PacketDataForPerFrame
    {
        public UsersBaseData comData;
        public GameData inGameData;
    }

    /// <summary>
    /// 通信時に必須なデータ事項構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UsersBaseData
    {
        public int myPort;       //送信ポート番号
        public int targetPort;    //受信ポート番号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string myIP;           //ユーザIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string targetIP;       //ユーザIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string sendAddress;    //送信アドレス
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string receiveAddress; //受信アドレス
    }

    /// <summary>
    /// インゲーム内の通信させたいデータ格納用
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string test;
        public int num;
    }

    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    List<PlayerNetData> userData;
    

    //////////////////////
    //////// 関数 ////////
    //////////////////////
    
    /// <summary>
    /// ゲームデータをバイト配列へ変換します
    /// </summary>
    /// <param name="_data">外部で作成されたインスタンスを指定します</param>
    /// <returns>渡したインスタンスをバイト配列として返します</returns>
    public byte[] StructToByte<T>(T _data)where T : struct
    {
        int size = Marshal.SizeOf(_data);

        byte[] bytes = new byte[size];

        GCHandle gchw = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        

        try
        {
            IntPtr ptr = gchw.AddrOfPinnedObject();
            Marshal.StructureToPtr(_data, ptr, true);
        }
        finally
        {
            gchw.Free();
        }

        Debug.Log("バイト配列は " + bytes.Length);

        return bytes;
    }

    /// <summary>
    /// バイト配列から構造体に組み直します
    /// </summary>
    /// <returns>組まれた構造体です</returns>
    public T ByteToStruct<T>(byte[] _data) where T : struct
    {
        T strData = default(T);

        GCHandle gch = GCHandle.Alloc(_data, GCHandleType.Pinned);

        try
        {
            IntPtr ptr = gch.AddrOfPinnedObject();
            strData = Marshal.PtrToStructure<T>(ptr);
        }
        finally
        {
            gch.Free();
        }

        return strData;
    }
}
