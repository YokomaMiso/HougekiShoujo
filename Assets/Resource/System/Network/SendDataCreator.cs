using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendDataCreator
{
    ////// c#では構造体内のアクセスが厳しいためとりあえず全public宣言
    //////（構造体のアクセスは現状のpublicが一番軽いみたいなので処理余裕があればプロパティでカプセル化）

    /// <summary>
    /// データ送受信時に使うデータを全てまとめたやつ
    /// </summary>
    /// 構造体インスタンス作成時にメモリ上での並びを固定
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] byteData;
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
        public int myPort;        //送信ポート番号
        public int targetPort;    //受信ポート番号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]  //送信データがstringの場合必須　SizeConstに代入した値分のバイト長で固定
        public string myIP;           //ユーザIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string targetIP;       //ユーザIP
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string sendAddress;    //送信アドレス
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string receiveAddress; //受信アドレス
    }

    /// <summary>
    /// インゲーム内の通信させたいデータ格納用　ゲーム内のデータはここに定義推奨
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameData
    {
        public Vector3 playerPos;
    }

    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    // NONE

    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // NONE

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    /// <summary>
    /// 構造体からバイト配列へ変換します
    /// </summary>
    /// <typeparam name="T">バイト配列にしたい構造体</typeparam>
    /// <param name="_data">バイト配列にしたい構造体のインスタンス</param>
    /// <returns>バイト配列</returns>
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

        return bytes;
    }

    /// <summary>
    /// バイト配列から構造体へ変換します
    /// </summary>
    /// <typeparam name="T">バイト配列から変換させたい構造体</typeparam>
    /// <param name="_data">バイト配列</param>
    /// <returns>テンプレートで指定した構造体データ</returns>
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
