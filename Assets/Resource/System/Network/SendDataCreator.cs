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
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public byte[] sendData;
        public byte[] receiveData;
    }

    /// <summary>
    /// 毎フレーム送るべきデータ群構造体
    /// </summary>
    public struct PacketDataForPerFrame
    {
        public UsersBaseData comData;
        public GameData inGameData;
    }

    /// <summary>
    /// 通信時に必須なデータ事項構造体
    /// </summary>
    public struct UsersBaseData
    {
        public int myPort;       //送信ポート番号
        public int targetPort;    //受信ポート番号
        public string myIP;           //ユーザIP
        public string targetIP;       //ユーザIP
        public string sendAddress;    //送信アドレス
        public string receiveAddress; //受信アドレス
    }

    /// <summary>
    /// インゲーム内の通信させたいデータ格納用
    /// </summary>
    public struct GameData
    {

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
    public byte[] StructToByte(PlayerNetData _data)
    {
        int size = Marshal.SizeOf(_data.mainPacketData);

        byte[] bytes = new byte[size];

        GCHandle gchw = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        Marshal.StructureToPtr(_data, gchw.AddrOfPinnedObject(), false);
        gchw.Free();

        return bytes;
    }

    /// <summary>
    /// バイト配列から構造体に組み直します
    /// </summary>
    /// <returns>組まれた構造体です</returns>
    public PacketDataForPerFrame ByteToStruct()
    {
        PacketDataForPerFrame _data = new PacketDataForPerFrame();
        int size = Marshal.SizeOf(_data);

        byte[] buffer = new byte[size];

        GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        _data = (PacketDataForPerFrame)Marshal.PtrToStructure(gch.AddrOfPinnedObject(), typeof(PacketDataForPerFrame));
        gch.Free();

        return _data;
    }
}
