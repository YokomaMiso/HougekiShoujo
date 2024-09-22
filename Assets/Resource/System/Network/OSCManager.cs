using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
using OscCore;

//public class OSCManager : MonoBehaviour
//{
//    //////////////////////////////
//    //////// 本番使用変数 ////////
//    //////////////////////////////

//    //自身のインゲームデータ
//    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

//    //送られてきた相手のインゲームデータ
//    public IngameData.PlayerNetData receivedIngameData = new IngameData.PlayerNetData();

//    //ハンドシェイク時にブロードキャストで送信するデータ
//    public HandshakeData.SendUserData firstData = new HandshakeData.SendUserData();

//    //サーバ役になった時のハンドシェイクデータの受け皿
//    public HandshakeData.SendUserData receivedFirstData = new HandshakeData.SendUserData();

//    //接続確認時のサーバからの返答データ
//    public ResponseServerData.ResData resServerData = new ResponseServerData.ResData();

//    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();
//    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

//    public SendDataCreator netInstance = new SendDataCreator();

//    //自分がサーバかどうか
//    bool isServer = false;
//    bool isServerResponse = false;

//    const float waitHandshakeResponseTime = 4f;

//    ///////// OSCcore周り ////////

//    OscClient Client;
//    OscServer Server;
//    OscServer mainServer;
//    OscServer ingameServer;

//    bool isReceiveIngame = false;

//    //使用するポート番号の始め
//    const int startPort = 8000;


//    enum NetworkState
//    {
//        HandShaking,
//        Maching,
//        InGame
//    }

//    NetworkState nowNetState = NetworkState.HandShaking;

//    ////////////////////////////////
//    //////// デバック用変数 ////////
//    ////////////////////////////////

//    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
//    public static OSCManager OSCinstance;

//    [SerializeField]
//    bool isManual = true;

//    [SerializeField]
//    int port = 8000;

//    //[SerializeField]
//    //int otherPort = 8001;

//    int testNum = 0;

//    byte[] handshakeBytes = new byte[0];
//    byte[] roomDataBytes = new byte[0];
//    int receiveLong = 0;

//    bool handshaked = false;

//    //////////////////////
//    //////// 関数 ////////
//    //////////////////////

//    // Start is called before the first frame update
//    void Start()
//    {
//        /////////////////////////////
//        //////////初期通信処理///////
//        /////////////////////////////

//        roomData = initRoomData(roomData);
//        receiveRoomData = initRoomData(receiveRoomData);

//        handshakeBytes = netInstance.StructToByte(firstData);
//        roomDataBytes = netInstance.StructToByte(roomData);

//        //サーバがいた場合のデータセット
//        myNetIngameData.mainPacketData.comData.targetIP = "255.255.255.255";
//        myNetIngameData.mainPacketData.comData.receiveAddress = "/example";

//        //一旦自身のIPもブロードキャストアドレスに
//        firstData.IP = "255.255.255.255";

//        //一時的にサーバからの返答を受け付けるポート番号取得
//        firstData.tempPort = GetRandomTempPort();

//        if (Client == null)
//        {
//            //8000番（サーバ役のポート番号）へ宛てたクライアントを作成
//            Client = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, startPort);

//        }

//        if (Server == null)
//        {
//            //一時ポート番号で作成
//            Server = new OscServer(firstData.tempPort);
//        }

//        Server.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);

//        Debug.Log("ハンドシェイク開始");

//        //1秒ごとにハンドシェイクのデータ送信を試みる
//        InvokeRepeating("SendFirstHandshake", 0f, 1f);

//        //上のハンドシェイクを指定時間試したらタイムアウトさせる必要があるためこの中で上処理を止める
//        //もし返答がなければこの中でサーバを作成する
//        StartCoroutine(CheckForResponse());

//        /////////////////////////////
//        ////////OSCcore内処理////////
//        /////////////////////////////

//        OSCinstance = this;

//        //myNetIngameData = default;
//        //myNetIngameData.mainPacketData = default;
//        //myNetIngameData.byteData = null;
//        //
//        //receivedIngameData = default;
//        //receivedIngameData.byteData = new byte[0];
//        //
//        //if(isManual)
//        //{
//        //    myNetIngameData.mainPacketData.comData.myPort = port;
//        //}
//        //else
//        //{
//        //    //myNetIngameData.mainPacketData.comData.myPort = PortAssignor();
//        //}
//        //
//        //
//        //Managers.instance.playerID = myNetIngameData.mainPacketData.comData.myPort - 8000;
//        //
//        //Debug.Log("私のポート番号は : " + myNetIngameData.mainPacketData.comData.myPort);
//        //Debug.Log("私のプレイヤー番号は : " + Managers.instance.playerID);
//        //
//        ////myNetData.mainPacketData.comData.myIP = "255.255.255.255";
//        ////myNetData.mainPacketData.comData.targetIP = "255.255.255.255";
//        ////myNetData.mainPacketData.comData.targetPort = otherPort;
//        //
//        //myNetIngameData.mainPacketData.comData.sendAddress = "/example";
//        //
//        //if(myNetIngameData.mainPacketData.comData.myPort == startPort)
//        //{
//        //    myNetIngameData.mainPacketData.comData.targetPort = 8001;
//        //}
//        //else
//        //{
//        //    myNetIngameData.mainPacketData.comData.targetPort = 8000;
//        //}
//        //
//        //
//        //if (mainClient == null)
//        //{
//        //    mainClient = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, myNetIngameData.mainPacketData.comData.targetPort);
//        //
//        //}
//        //
//        //if (mainServer == null)
//        //{
//        //    //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//        //    mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);
//        //}
//        //
//        //mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);


//    }

//    // Update is called once per frame
//    void Update()
//    {
//        ////デバック用で任意のタイミングで送れるようにしておく
//        //if(Input.GetKeyDown(KeyCode.Space))
//        //{
//        //    SendValue();
//        //}

//        //if(Input.GetKeyDown(KeyCode.M))
//        //{
//        //    myNetData.mainPacketData.inGameData.num += 1;
//        //    myNetData.mainPacketData.inGameData.test = "HelloWorld!";
//        //    myNetData.mainPacketData.inGameData.vc3 += new Vector3(1.0f, 0.5f, 0.1f);



//        //}

//        //if(Input.GetKeyDown(KeyCode.P))
//        //{
//        //    Debug.Log(receivedData.mainPacketData.inGameData.num);
//        //    Debug.Log(receivedData.mainPacketData.inGameData.test);
//        //    Debug.Log(receivedData.mainPacketData.inGameData.vc3);
//        //}

//        Debug.Log(testNum);


//        //Debug.Log("ハンドシェイク用データ" + handshakeBytes.Length);
//        //Debug.Log("部屋情報" + roomDataBytes.Length);
//        //Debug.Log("受信バイトサイズ" + receiveLong);

//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            Debug.Log("ホストプレイヤーID : " + roomData.hostPlayer);
//        }

//        if (Input.GetKeyDown(KeyCode.M))
//        {
//            Debug.Log(Client.Destination);
//            SendMachingData();
//        }

//        if (Managers.instance.gameManager.play && isReceiveIngame == false)
//        {
//            isReceiveIngame = true;
//            if (Managers.instance.playerID == 0)
//            {
//                mainServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingServer);
//                mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);
//            }
//            else
//            {
//                mainServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingClient);
//                mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);
//            }
//        }
//    }

//    private void LateUpdate()
//    {
//        if (Managers.instance.state == GAME_STATE.IN_GAME)
//        {
//            SendIngameValue();
//        }
//    }

//    private void OnEnable()
//    {




//    }

//    private void OnDisable()
//    {

//        if (Server != null)
//        {
//            Server.Dispose();
//        }

//    }

//    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
//    {
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetBannerNum(i, MachingRoomData.bannerEmpty); }
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
//        _roomData.hostPlayer = 0;
//        _roomData.gameStart = false;


//        return _roomData;
//    }

//    //////////////////////////
//    //////インゲーム関数//////
//    //////////////////////////

//    /// <summary>
//    /// データ送信
//    /// </summary>
//    private void SendIngameValue()
//    {
//        //送信データのバイト配列化
//        myNetIngameData.byteData = netInstance.StructToByte(myNetIngameData.mainPacketData);

//        //データの送信
//        Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, myNetIngameData.byteData, myNetIngameData.byteData.Length);
//    }

//    /// <summary>
//    /// サーバ側でデータをキャッチすれば呼び出されます
//    /// </summary>
//    /// <param name="values">受信したデータ</param>
//    /// <remarks>サブスレッド動作のためUnity用のメソッドは動作しません！！！</remarks>
//    private void ReadIngameValue(OscMessageValues values)
//    {
//        //受信データのコピー
//        values.ReadBlobElement(0, ref receivedIngameData.byteData);

//        //データの構造体化
//        receivedIngameData.mainPacketData = netInstance.ByteToStruct<IngameData.PacketDataForPerFrame>(receivedIngameData.byteData);

//        return;
//    }

//    //////////////////////////////
//    ///////////初期通信関数///////
//    //////////////////////////////

//    int GetRandomTempPort()
//    {
//        return Random.Range(8006, 9000);
//    }

//    //ハンドシェイク送信関数
//    private void SendFirstHandshake()
//    {
//        byte[] bytes = netInstance.StructToByte(firstData);

//        Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, bytes, bytes.Length);

//        Debug.Log("1way送信");

//        return;
//    }

//    /// <summary>
//    /// ブロードキャスト送信時のサーバからの返答用
//    /// </summary>
//    /// <param name="value"></param>
//    private void ReceiveHandshakeForClient(OscMessageValues value)
//    {
//        byte[] _bytes = new byte[0];

//        value.ReadBlobElement(0, ref _bytes);

//        resServerData = netInstance.ByteToStruct<ResponseServerData.ResData>(_bytes);

//        receiveRoomData = resServerData.serverRoomData;

//        //このメソッドが呼び出されている時点でサーバからの返答が来ているためtrueにする
//        isServerResponse = true;

//        return;
//    }

//    /// <summary>
//    /// ハンドシェイク送信時の反応確認用
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator CheckForResponse()
//    {
//        yield return new WaitForSeconds(waitHandshakeResponseTime);

//        Debug.Log("コルーチン作動");

//        if (!isServerResponse)
//        {
//            //ハンドシェイク確認用パケット破棄前にサーバがなくなるとバグるためここに記述
//            CancelInvoke("SendFirstHandshake");
//            Debug.Log("サーバからの返答がありません、サーバ処理へ移行");

//            Managers.instance.playerID = 0;
//            Client = new OscClient("255.255.255.255", 8001);
//            StartServer();
//        }
//        else
//        {
//            CancelInvoke("SendFirstHandshake");
//            Debug.Log("サーバが存在しました、クライアント処理へ移行");

//            Managers.instance.playerID = 1;
//            Client = new OscClient("255.255.255.255", 8000);
//            StartClient();
//        }
//    }

//    //サーバが存在したため受信データをまとめる
//    private void StartClient()
//    {
//        Server.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
//        Server.Dispose();

//        if (mainServer == null)
//        {
//            mainServer = new OscServer(8001);
//        }

//        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingClient);

//        return;
//    }

//    private void ReceiveMachingClient(OscMessageValues value)
//    {
//        byte[] _recieveByte = new byte[0];

//        value.ReadBlobElement(0, ref _recieveByte);

//        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_recieveByte);

//        return;
//    }

//    //サーバ不在時に自分がサーバになる処理
//    void StartServer()
//    {
//        isServer = true;
//        isServerResponse = true;

//        Server.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
//        Server.Dispose();

//        if (mainServer == null)
//        {
//            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//            mainServer = new OscServer(startPort);
//        }

//        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingServer);

//        return;
//    }

//    /// <summary>
//    /// サーバ役になった際のクライアントハンドシェイク受信用
//    /// </summary>
//    /// <param name="value"></param>
//    private void ReceiveMachingServer(OscMessageValues value)
//    {
//        byte[] _receiveBytes = new byte[0];
//        byte[] _sendBytes = new byte[0];

//        resServerData.toClientPort = 8001;
//        resServerData.serverIP = "255.255.255.255";

//        value.ReadBlobElement(0, ref _receiveBytes);

//        receiveLong = _receiveBytes.Length;

//        //送信されてきたバイト配列サイズがハンドシェイク用と同じならハンドシェイクデータとして取り扱う
//        if (handshaked == false) //_receiveBytes.Length == 1048
//        {
//            handshaked = true;

//            testNum = 1;
//            receivedFirstData = netInstance.ByteToStruct<HandshakeData.SendUserData>(_receiveBytes);

//            Client = new OscClient(receivedFirstData.IP, receivedFirstData.tempPort);

//            resServerData.serverRoomData = roomData;

//            _sendBytes = netInstance.StructToByte(resServerData);

//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);

//            //3wayができたらそちらへ移行
//            Client = new OscClient("255.255.255.255", 8001);
//        }
//        else //if(_receiveBytes.Length == 1112)
//        {
//            testNum = 2;
//            receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);

//            //Client = new OscClient("255.255.255.255", 8001);

//            _sendBytes = netInstance.StructToByte(roomData);

//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }

//        return;
//    }

//    void SendMachingData()
//    {
//        byte[] _sendBytes = new byte[0];

//        _sendBytes = netInstance.StructToByte(roomData);

//        if (isServer)
//        {
//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }
//        else
//        {
//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }


//        return;
//    }

//    public void SendRoomData()
//    {
//        SendMachingData();

//        return;
//    }
//}

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    //送信データ構造体
    public AllGameData.AllData allData = new AllGameData.AllData();

    //自身のローカルインゲームデータ
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();
    
    //自身のローカルマッチングシーンデータ
    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

    //構造体変換処理があるため生成
    SendDataCreator netInstance = new SendDataCreator();

    //最大プレイヤー人数
    const int maxPlayer = 6;

    const string broadcastAddress = "255.255.255.255";

    int tempPort;

    const int startPort = 8000;

    string address = "/main";

    ///////// OSCcore周り ////////
    
    //送信先保存リスト
    //クライアントならホスト宛ての1つ、ホストならクライアント5人分が入る
    //ハンドシェイク時はホストからの応答を確認するため必ず一つだけ入る
    List<OscClient> clientList = new List<OscClient>();


    OscServer tempServer;
    OscServer mainServer;

    //自分がサーバかどうか
    bool isServer = false;
    bool isServerResponse = false;

    bool isFinishHandshake = false;

    const float waitHandshakeResponseTime = 4f;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
    public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    int testNum = 0;

    //ゲーム内で必要な送受信データリスト
    //　要素数＝プレイヤーID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    string testS;

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //自分のインスタンス
        OSCinstance = this;

        //インゲーム用データの初期化代入
        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;

        //ルームデータの初期化
        allData.rData = initRoomData(allData.rData);

        //ロ−カル用も同様に
        roomData = default;
        roomData = initRoomData(roomData);


        //以下Start処理はハンドシェイク完了後に行うため以降予定

        //playerIDの割り当て
        //Managers.instance.playerID = myPort - 8000;
        //roomData.myID = Managers.instance.playerID;
        //
        ////自分のデータに自分のポートとplayerIDを保存、データ初期化判定をtrueに
        //myNetIngameData.mainPacketData.comData.myPort = myPort;
        //myNetIngameData.PlayerID = Managers.instance.playerID;
        //roomData.isInData = true;

        //自分のデータだった時だけポート番号を入れる
        for (int i = 0; i < maxPlayer; i++)
        {
            //if (i == Managers.instance.playerID)
            //{
            //    //自分のplayerIDがデータリストの引数と同じならローカル用の自身のデータを送信用データにコピーする
            //    allData.pData.mainPacketData.comData.myPort = myNetIngameData.mainPacketData.comData.myPort;
            //    allData.pData.PlayerID = myNetIngameData.PlayerID;
            //    allData.rData = initRoomData(allData.rData);
            //    allData.rData.isInData = roomData.isInData;
            //    allData.rData.myID = myNetIngameData.PlayerID;
            //    playerDataList.Add(allData);
            //}
            //else
            //{
            //    //もし自分以外のデータなら一旦居ないものとして扱いデータをセットする
            //    allData.pData.mainPacketData.comData.myPort = -1;
            //    allData.pData.PlayerID = i;
            //    allData.rData = initRoomData(allData.rData);
            //    allData.rData.isInData = false;
            //    playerDataList.Add(allData);
            //}

            //全て初期値で最大人数分のデータをセットする
            //allData.pData.mainPacketData.comData.myPort = -1;
            allData.pData.PlayerID = i;
            allData.rData = initRoomData(allData.rData);
            allData.rData.isInData = false;
            playerDataList.Add(allData);
        }

        //allData.pData.PlayerID = 0;
        //allData.rData.myID = 0;
        //playerDataList[0]

        //もし自分がサーバ役ならクライアント五人分の送信データを作成
        //if (Managers.instance.playerID == 0)
        //{
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        OscClient _client = new OscClient(broadcastAddress, 8000 + i);
        //
        //        clientList.Add(_client);
        //    }
        //}
        ////そうでない（クライアント）ならサーバへのみ宛てたデータの作成
        //else
        //{
        //    OscClient _client = new OscClient(broadcastAddress, 8000);
        //
        //    clientList.Add(_client);
        //}
        //
        //if (server == null)
        //{
        //    //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
        //    server = new OscServer(myPort);
        //}
        
        //server.TryAddMethodPair(address, ReadValue, MainThreadMethod);

        CreateTempNet();
    }

    // Update is called once per frame
    //インゲームデータ処理中に送信されるろまずいのでUpdateは基本不使用
    void Update()
    {
        //デバック用で任意のタイミングで送れるようにしておく
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        Debug.Log("PlayerID is " + Managers.instance.playerID);
        Debug.Log("Port is " + myNetIngameData.mainPacketData.comData.myPort);
        Debug.Log("IPAddress is " + GetLocalIPAddress());
        Debug.Log(testS);
        Debug.Log(testNum);

    }

    private void LateUpdate()
    {
        allData.rData = roomData;
        allData.pData = myNetIngameData;
        playerDataList[Managers.instance.playerID] = allData;


        //ハンドシェイクが完了していれば毎フレームインゲームデータを送信する
        if (isFinishHandshake)
        {
            Debug.Log("インゲームデータ送信");

            /*
            //インゲームデータから送信用データへコピー
            allData.rData = roomData;
            allData.pData = myNetIngameData;

            //そのデータを送信用リストへ更にコピー
            playerDataList[Managers.instance.playerID] = allData;
            */

            //送信用データリストにある分送信を試みる
            for (int i = 0; i < playerDataList.Count; i++)
            {
                //ルームデータは初期化が行われていないと参照エラーが起きるため仮インスタンスを作成し代入
                AllGameData.AllData _data = new AllGameData.AllData();
                _data.rData = initRoomData(_data.rData);
            
                _data = playerDataList[i];
            
                SendValue(_data);
            }
        }
    }

    private void OnDisable()
    {
        if (tempServer != null)
        {
            tempServer.Dispose();
        }

        if(mainServer != null)
        {
            mainServer.Dispose();
        }
    }

    ////////////////////////////////////////////////////
    ///////////////　ハンドシェイク用関数　/////////////
    ////////////////////////////////////////////////////

    private string GetLocalIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            return ip.ToString();
        }

        Debug.LogError("コンピュータ内にIPv4が存在しません");
        return null;
    }

    private void CreateTempNet()
    {
        //サーバがいるかどうか応答を確認するためのクライアントを作成する
        OscClient _client = new OscClient(broadcastAddress, startPort);

        clientList.Add(_client);

        //一時ポート番号でサーバからの応答を待機
        tempPort = GetRandomTempPort();

        tempServer = new OscServer(tempPort);

        tempServer.TryAddMethod(address, ReadValue);

        //1秒ごとにハンドシェイクのデータ送信を試みる
        InvokeRepeating("SendFirstHandshake", 0f, 1f);

        //上のハンドシェイクを指定時間試したらタイムアウトさせる必要があるためこの中で上処理を止める
        //もし返答がなければこの中でサーバを作成する
        StartCoroutine(CheckForResponse());

        return;
    }

    private void SendFirstHandshake()
    {
        AllGameData.AllData _data = new AllGameData.AllData();
        _data.rData = initRoomData(_data.rData);

        _data.pData.mainPacketData.comData.myIP = broadcastAddress; //GetLocalIPAddress();
        _data.pData.mainPacketData.comData.myPort = tempPort;
        _data.rData.myID = -1;

        Debug.Log("ハンドシェイクの送信");

        if(roomData.isHandshaking == true)
        {
            SendValue(_data);
        }
        

        return;
    }

    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

            Debug.Log("コルーチン作動");

            if (roomData.isHandshaking)
            {
                //ハンドシェイク確認用パケット破棄前にサーバがなくなるとバグるためここに記述
                CancelInvoke("SendFirstHandshake");
                Debug.Log("サーバからの返答がありません、サーバ処理へ移行");

                Managers.instance.playerID = 0;
                myNetIngameData.PlayerID = Managers.instance.playerID;
                roomData.myID = Managers.instance.playerID;

                allData.pData = myNetIngameData;
                allData.rData = roomData;

                playerDataList[0] = allData;

                clientList.Clear();

                tempServer.Dispose();

                mainServer = new OscServer(startPort);

                mainServer.TryAddMethod(address, ReadValue);

                isServer = true;

                isFinishHandshake = true;
            }
            else
            {
                CancelInvoke("SendFirstHandshake");
                Debug.Log("サーバが存在しました、クライアント処理へ移行");

                tempServer.Dispose();

                mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);

                mainServer.TryAddMethod(address, ReadValue);

                isServer = false;

                isFinishHandshake = true;
            }
    }

    private void StartToClient()
    {

        return;
    }

    private int GetRandomTempPort()
    {
        return UnityEngine.Random.Range(8006, 9000);
    }

    ////////////////////////////////////////////////////
    /////////////////　接続安定後用関数　///////////////
    ////////////////////////////////////////////////////

    /// <summary>
    /// データ送信関数
    /// </summary>
    /// <typeparam name="T">構造体及び値型のデータ</typeparam>
    /// <param name="_struct">実際に送信したい構造体データ</param>
    private void SendValue<T>(T _struct) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //送信データのバイト配列化
        _sendBytes = netInstance.StructToByte(_struct);


        //サーバなら五回、クライアントなら一回
        foreach (OscClient _client in clientList)
        {
            //データの送信
            _client.Send(address, _sendBytes, _sendBytes.Length);
        }
    }

    private void SendValueTarget<T>(T _struct, OscClient _client) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //送信データのバイト配列化
        _sendBytes = netInstance.StructToByte(_struct);

        
        //データの送信
        _client.Send(address, _sendBytes, _sendBytes.Length);
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <param name="values">受信したデータ</param>
    /// <remarks>サブスレッド動作のためUnity用のメソッドは動作しません！！！</remarks>
    private void ReadValue(OscMessageValues values)
    {
        byte[] _receiveBytes = new byte[0];

        //受信データのコピー
        values.ReadBlobElement(0, ref _receiveBytes);

        //データの構造体化
        AllGameData.AllData _allData = new AllGameData.AllData();
        _allData.rData = initRoomData(_allData.rData);
        _allData = netInstance.ByteToStruct<AllGameData.AllData>(_receiveBytes);
        

        if (isServer)
        {
            //testS = "サーバ";

            //受信したプレイヤーデータがゲーム内に存在する場合データリストにセットする
            if (!_allData.rData.isHandshaking)
            {
                testS = "サーバとしてインゲーム受信";
                playerDataList[_allData.pData.PlayerID] = _allData;
            }
            else if (_allData.rData.myID == -1 && _allData.rData.isHandshaking)
            {
                //testS = "サーバとしてコネクション受信";
                

                for (int i = 0; i < playerDataList.Count; i++)
                {
                    if (playerDataList[i].rData.myID == -1)
                    {
                        AllGameData.AllData _handshakeAllData = new AllGameData.AllData();
                        _handshakeAllData.rData = initRoomData(_handshakeAllData.rData);

                        _handshakeAllData.pData.mainPacketData.comData.myIP = broadcastAddress; //GetLocalIPAddress();
                        _handshakeAllData.pData.mainPacketData.comData.myPort = startPort + i;
                        _handshakeAllData.pData.PlayerID = i;
                        _handshakeAllData.rData.myID = i;
                        _handshakeAllData.rData.isHandshaking = false;

                        testNum = _allData.pData.mainPacketData.comData.myPort;

                        playerDataList[i] = _handshakeAllData;

                        OscClient _tempClient = new OscClient("255.255.255.255", testNum);

                        SendValueTarget(_handshakeAllData, _tempClient);

                        OscClient _client = new OscClient("255.255.255.255", startPort + i);
                        clientList.Add(_client);

                        break;
                    }
                }
            }
        }
        else
        {
            if (!isFinishHandshake)
            {
                //testS = "クライアントとしてハンドシェイク受信";

                myNetIngameData = _allData.pData;
                roomData = _allData.rData;

                //roomData.isHandshaking = false;

                Managers.instance.playerID = _allData.rData.myID;
            }
            else
            {
                testS = "クライアントとしてインゲーム受信";

                playerDataList[_allData.pData.PlayerID] = _allData;
            }
        }
    }

    /// <summary>
    /// ルームデータの初期化処理
    /// </summary>
    /// <param name="_roomData">初期化したいルームデータ</param>
    /// <returns>ルームデータの初期化値</returns>
    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        _roomData.myBannerNum = -1;
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;
        _roomData.isInData = true;
        _roomData.isHandshaking = true;
        _roomData.myID = -1;

        return _roomData;
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
    private void MainThreadMethod()
    {

    }

    /// <summary>
    /// ルームデータのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDのルームデータ</returns>
    public MachingRoomData.RoomData GetRoomData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.rData;
    }

    /// <summary>
    /// インゲームデータのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDのインゲームデータ</returns>
    public IngameData.PlayerNetData GetIngameData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.pData;
    }

    /// <summary>
    /// 全通信データのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDの全データ</returns>
    public AllGameData.AllData GetAllData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata;
    }

    /// <summary>
    /// 指定したプレイヤーIDの取得
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>プレイヤーID</returns>
    public int GetPlayerID(int _num)
    {
        return playerDataList[_num].pData.PlayerID;
    }
}