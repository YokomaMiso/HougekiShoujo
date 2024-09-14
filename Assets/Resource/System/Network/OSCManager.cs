using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using UnityEngine;
using OscCore;
using System.Runtime.InteropServices;

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

    public AllGameData.AllData allData = new AllGameData.AllData();

    //自身のインゲームデータ
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

    //送られてきた相手のインゲームデータ
    public IngameData.PlayerNetData receivedIngameData = new IngameData.PlayerNetData();

    //自身のネットワークデータ
    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

    //送られてきた相手のデータ
    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

    SendDataCreator netInstance = new SendDataCreator();

    string broadcastAddress = "255.255.255.255";

    string address = "/example";

    ///////// OSCcore周り ////////

    OscClient client;
    List<OscClient> clientList = new List<OscClient>();

    OscServer server;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
    public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    [SerializeField]
    int otherPort = 8001;

    int testNum = 0;

    //ゲーム内で必要な送受信データリスト
    //　要素数＝プレイヤーID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        OSCinstance = this;

        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;
        
        allData.rData = initRoomData(allData.rData);

        roomData = default;
        receiveRoomData = default;

        roomData = initRoomData(roomData);
        receiveRoomData = initRoomData(receiveRoomData);

        // insert to playerID
        Managers.instance.playerID = myPort - 8000;

        //自分のデータに自分のポートを保存
        myNetIngameData.mainPacketData.comData.myPort = myPort;
        myNetIngameData.PlayerID = Managers.instance.playerID;
        roomData.isInData = true;
        
        //自分のデータだった時だけポート番号を入れる
        for (int i = 0; i <= 5; i++)
        {
            if(i == Managers.instance.playerID)
            {
                allData.pData.mainPacketData.comData.myPort = myNetIngameData.mainPacketData.comData.myPort;
                allData.pData.PlayerID = myNetIngameData.PlayerID;
                allData.rData = initRoomData(allData.rData);
                allData.rData.isInData = roomData.isInData;
                playerDataList.Add(allData);
            }
            else
            {
                allData.pData.mainPacketData.comData.myPort = -1;
                allData.pData.PlayerID = -1;
                allData.rData = initRoomData(allData.rData);
                allData.rData.isInData = false;
                playerDataList.Add(allData);
            }
        }

        //もし自分がサーバ役ならクライアント五人分の送信データを作成
        if(Managers.instance.playerID == 0)
        {
            for(int i = 1; i <= 5; i++)
            {
                OscClient _client = new OscClient(broadcastAddress, 8000 + i);

                clientList.Add(_client);
            }
        }
        //そうでない（クライアント）ならサーバへのみ宛てたデータの作成
        else
        {
            OscClient _client = new OscClient(broadcastAddress, 8000);

            clientList.Add(_client);
        }

        if (server == null)
        {
            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
            server = new OscServer(myPort);
        }

        server.TryAddMethodPair(address, ReadValue, MainThreadMethod);
    }

    // Update is called once per frame
    void Update()
    {
        //デバック用で任意のタイミングで送れるようにしておく
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        //Debug.Log(testNum);

    }

    private void LateUpdate()
    {
        allData.rData = roomData;
        allData.pData = myNetIngameData;

        playerDataList[Managers.instance.playerID] = allData;

        //存在するプレイヤー（固定で６回）の分送信する
        //foreach (AllGameData.AllData _data in playerDataList)
        //{
        //    SendValue(_data);
        //}

        for (int i = 0; i < playerDataList.Count; i++)
        {
            AllGameData.AllData _data = new AllGameData.AllData();
            _data.rData = initRoomData(_data.rData);

            _data = playerDataList[i];

            SendValue(_data);
        }
    }

    private void OnDisable()
    {
        if (server != null)
        {
            server.Dispose();
        }

    }

    /// <summary>
    /// データ送信
    /// </summary>
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


        if(_allData.pData.PlayerID != -1)
        {
            playerDataList[_allData.pData.PlayerID] = _allData;
        }
        
        //receiveRoomData = allData.rData;
        //receivedIngameData = allData.pData;
    }

    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetBannerNum(i, MachingRoomData.bannerEmpty); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;
        _roomData.isInData = true;

        return _roomData;
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
    private void MainThreadMethod()
    {

    }

    public MachingRoomData.RoomData GetRoomData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.rData;
    }

    public IngameData.PlayerNetData GetIngameData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.pData;
    }

    public AllGameData.AllData GetAllData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata;
    }

    public int GetPlayerID(int _num)
    {
        return playerDataList[_num].pData.PlayerID;
    }
}