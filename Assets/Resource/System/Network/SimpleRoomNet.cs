using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;


//public class OSCManager : MonoBehaviour
//{
//    //////////////////////////////
//    //////// 本番使用変数 ////////
//    //////////////////////////////

//    //自身のネットワークデータ
//    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

//    //送られてきた相手のデータ
//    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

//    SendDataCreator netInstance = new SendDataCreator();

//    string broadcastAddress = "255.255.255.255";

//    string address = "/example";

//    ///////// OSCcore周り ////////

//    OscClient client;
//    OscServer server;


//    ////////////////////////////////
//    //////// デバック用変数 ////////
//    ////////////////////////////////

//    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
//    public static OSCManager OSCinstance;

//    [SerializeField]
//    int myPort = 8000;

//    [SerializeField]
//    int otherPort = 8001;

//    //////////////////////
//    //////// 関数 ////////
//    //////////////////////

//    // Start is called before the first frame update
//    void Start()
//    {
//        roomData = default;
//        receiveRoomData = default;

//        if (client == null)
//        {
//            client = new OscClient(broadcastAddress, otherPort);

//        }

//        if (server == null)
//        {
//            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//            server = new OscServer(myPort);
//        }

//        server.TryAddMethodPair(broadcastAddress, ReadValue, MainThreadMethod);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //デバック用で任意のタイミングで送れるようにしておく
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            SendValue();
//        }
        
//    }

//    private void LateUpdate()
//    {
//        SendValue();
//    }

//    private void OnDisable()
//    {
//        if (server != null)
//        {
//            server.Dispose();
//        }

//    }

//    /// <summary>
//    /// データ送信
//    /// </summary>
//    private void SendValue()
//    {
//        byte[] _sendBytes = new byte[0];

//        //送信データのバイト配列化
//        _sendBytes = netInstance.StructToByte(roomData);

//        //データの送信
//        client.Send(address, _sendBytes, _sendBytes.Length);
//    }

//    /// <summary>
//    /// サーバ側でデータをキャッチすれば呼び出されます
//    /// </summary>
//    /// <param name="values">受信したデータ</param>
//    /// <remarks>サブスレッド動作のためUnity用のメソッドは動作しません！！！</remarks>
//    private void ReadValue(OscMessageValues values)
//    {
//        byte[] _receiveBytes = new byte[0];

//        //受信データのコピー
//        values.ReadBlobElement(0, ref _receiveBytes);

//        //データの構造体化
//        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);
//    }

//    /// <summary>
//    /// サーバ側でデータをキャッチすれば呼び出されます
//    /// </summary>
//    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
//    private void MainThreadMethod()
//    {

//    }
//}