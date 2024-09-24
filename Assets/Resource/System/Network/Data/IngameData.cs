using System.Runtime.InteropServices;
using UnityEngine;

public class IngameData
{
    /// <summary>
    /// データ送受信時に使うデータを全てまとめたやつ
    /// </summary>
    /// 構造体インスタンス作成時にメモリ上での並びを固定
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerNetData
    {
        public PacketDataForPerFrame mainPacketData;
        public int PlayerID;
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
        //public int targetPort;    //受信ポート番号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]  //送信データがstringの場合必須　SizeConstに代入した値分のバイト長で固定
        public string myIP;           //ユーザIP
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        //public string targetIP;       //ユーザIP
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
        //For Each
        public Vector3 playerPos;
        public Vector3 playerStickValue;
        public PLAYER_STATE playerState;
        public bool fire;
        public bool useSub;
        public bool alive;
        public RADIO_CHAT_ID playerChatID;

        //For Server
        public bool play;
        public bool start;
        public bool end;

        public int roundCount;
        public float roundTimer;
        public int deadPlayerCountTeamA;
        public int deadPlayerCountTeamB;
        public int winCountTeamA;
        public int winCountTeamB;
        public int winner;

    }
}
