using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendDataCreator
{
    ////// c#では構造体内のアクセスが厳しいためとりあえず全public宣言
    //////（構造体のアクセスは現状のpublicが一番軽いみたいなので処理余裕があればプロパティでカプセル化）
    
    
    

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
