using OscCore;
using UnityEngine;

public class OscClientData
{
    //送信用のクライアントデータ
    public OscClient client;

    //このクライアントを使用しているかどうか
    bool isUsing;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OscClientData()
    {
        this.client = new OscClient(OSCManager.broadcastAddress, OSCManager.startPort);
        this.isUsing = false;

        return;
    }

    /// <summary>
    /// このクライアントを有効化する
    /// </summary>
    /// <param name="_address">IPアドレス</param>
    /// <param name="_port">ポート番号</param>
    public void Assign(string _address, int _port)
    {
        this.client = new OscClient(_address, _port);
        this.isUsing = true;

        return;
    }

    /// <summary>
    /// このクライアントを無効化する
    /// </summary>
    public void Release()
    {
        this.client = new OscClient(OSCManager.broadcastAddress, OSCManager.startPort);
        this.isUsing = false;

        return;
    }

    /// <summary>
    /// このクライアントを使用しているかどうかの取得
    /// </summary>
    /// <returns>このクライアントの有効化判定</returns>
    public bool IsUsing()
    {
        return isUsing;
    }
}
