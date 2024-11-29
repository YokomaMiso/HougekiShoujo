using OscCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class OSCManagerLocalOnlyRoom : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    //送信データ構造体
    public AllGameData.AllData allData;

    //自身のローカルインゲームデータ
    public IngameData.PlayerNetData myNetIngameData;

    //自身のローカルマッチングシーンデータ
    public MachingRoomData.RoomData roomData;

    //構造体変換処理があるため生成
    SendDataCreator netInstance = new SendDataCreator();

    //最大プレイヤー人数
    const int maxPlayer = 6;

    public const string broadcastAddress = "255.255.255.255";

    public const int startPort = 50000;

    int tempPort;

    [SerializeField]
    float sendPerSecond = 60.0f;

    string address = "/main";

    ///////// OSCcore周り ////////

    //送信先保存リスト
    //クライアントならホスト宛ての1つ、ホストならクライアント5人分が入る
    //ハンドシェイク時はホストからの応答を確認するため必ず一つだけ入る
    List<OscClientData> clientList = new List<OscClientData>();
    List<float> connectTimeList = new List<float>();

    OscServer tempServer;
    OscServer mainServer;


    //自分がサーバかどうか
    bool isServer = false;
    bool isServerResponse = false;

    bool isFinishHandshake = false;

    const float waitHandshakeResponseTime = 2f;

    const float timeoutSec = 5f;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
    //public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    int testNum = 0;

    //ゲーム内で必要な送受信データリスト
    //　要素数＝プレイヤーID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    string testS;

    bool cutSend = false;

    bool isOutServer = false;

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //自分のインスタンス
        //OSCinstance = this;

        //CreateTempNet();
    }

    // Update is called once per frame
    //インゲームデータ処理中に送信されるろまずいのでUpdateは基本不使用
    void Update()
    {
        if (isOutServer)
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.roomManager.Init();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            cutSend = true;
            //Debug.Log("送信処理カット中");
        }
        else
        {
            cutSend = false;
        }


        //Debug.Log("PlayerID is " + Managers.instance.playerID);
        //Debug.Log("IPAddress is " + GetLocalIPAddress());
        //Debug.Log(testS);
        
        //Debug.Log(testNum);

        if (isFinishHandshake)
        {
            TimeoutChecker();
        }
    }

    float sendDataTimer;
    float sendStartTimer;

    private void LateUpdate()
    {
        if (isOutServer)
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.roomManager.Init();
        }

        if (playerDataList.Count == 0)
        {
            //Debug.Log("nullです");
            return;
        }

        allData.rData = roomData;
        allData.pData = myNetIngameData;
        playerDataList[Managers.instance.playerID] = allData;

        float fixedDeltaTime = 1.0f / sendPerSecond;

        sendDataTimer += Time.deltaTime;

        if (!isFinishHandshake) { return; }

        if (sendStartTimer <= 2f)
        {
            sendStartTimer += Time.deltaTime;
            return;
        }

        if (sendDataTimer >= fixedDeltaTime)
        {
            //ハンドシェイクが完了していれば毎フレームインゲームデータを送信する
            if (isFinishHandshake)
            {
                if (isServer)
                {
                    //送信用データリストにある分送信を試みる
                    for (int i = 0; i < playerDataList.Count; i++)
                    {
                        //ルームデータは初期化が行われていないと参照エラーが起きるため仮インスタンスを作成し代入
                        AllGameData.AllData _data = new AllGameData.AllData();
                        _data.rData = initRoomData(_data.rData);

                        _data = playerDataList[i];

                        if (_data.rData.myID != -1)
                        {
                            //Debug.Log("ID " + i + " へデータ送信");
                            SendValue(_data);
                        }
                    }
                }
                else
                {
                    AllGameData.AllData _data = new AllGameData.AllData();
                    _data.rData = initRoomData(_data.rData);
                    _data = playerDataList[Managers.instance.playerID];

                    if (cutSend)
                    {
                        return;
                    }

                    if (_data.rData.myID != -1)
                    {
                        //Debug.Log("インゲームデータ送信(クライアント)");
                        SendValue(_data);
                    }
                }
            }

            sendDataTimer = 0;
        }

    }

    private void OnDisable()
    {
        DisPacket();
    }

    private void DisPacket()
    {
        if (tempServer != null)
        {
            tempServer.Dispose();
        }

        if (mainServer != null)
        {
            mainServer.Dispose();
        }

        if (clientList.Count > 0)
        {
            foreach (OscClientData _client in clientList)
            {
                _client.Release();
            }

            clientList.Clear();
        }
    }

    ////////////////////////////////////////////////////
    ///////////////　ハンドシェイク用関数　/////////////
    ////////////////////////////////////////////////////

    private string GetLocalIPAddress()
    {
        IPAddress ipv4Address = null;

        // イーサネットのIPv4アドレスを探す
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Ethernet);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // Wi-FiのIPv4アドレスを探す
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Wireless80211);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // 両方存在しない場合はブロードキャストアドレスを返す
        return "255.255.255.255";
    }

    private IPAddress GetIPv4AddressByType(NetworkInterfaceType type)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == type && ni.OperationalStatus == OperationalStatus.Up)
            {
                var ipProperties = ni.GetIPProperties();
                var ipv4Address = ipProperties.UnicastAddresses
                    .Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ua => ua.Address)
                    .FirstOrDefault();

                if (ipv4Address != null)
                {
                    return ipv4Address;
                }
            }
        }
        return null;
    }

    public void CreateTempNet()
    {
        DisPacket();

        AllGameData.AllData allData = new AllGameData.AllData();
        myNetIngameData = new IngameData.PlayerNetData();
        roomData = new MachingRoomData.RoomData();

        playerDataList.Clear();
        clientList.Clear();
        connectTimeList.Clear();

        isServer = false;
        isServerResponse = false;
        isFinishHandshake = false;
        isOutServer = false;

        //インゲーム用データの初期化代入
        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;
        allData.pData.mainPacketData.inGameData = initIngameData(allData.pData.mainPacketData.inGameData);

        //ルームデータの初期化
        allData.rData = initRoomData(allData.rData);

        //ロ－カル用も同様に
        roomData = default;
        roomData = initRoomData(roomData);

        myNetIngameData = default;
        myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);


        //最大人数分のデータを作成
        for (int i = 0; i < maxPlayer; i++)
        {
            //全て初期値で最大人数分のデータをセットする
            allData.pData.PlayerID = i;
            allData.rData = initRoomData(allData.rData);
            allData.rData.isInData = false;
            playerDataList.Add(allData);

            //人数分受信時間を作る
            connectTimeList.Add(0.0f);

            //人数分のクライアント
            clientList.Add(new OscClientData());
        }

        //サーバがいるかどうか応答を確認するためのクライアントを作成する
        clientList[0].Assign(broadcastAddress, startPort);

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

        _data.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
        _data.pData.mainPacketData.comData.myPort = tempPort;
        _data.rData.myID = -1;


        if (roomData.isHandshaking == true)
        {
            //Debug.Log("ハンドシェイクの送信");
            SendValue(_data);
        }

        return;
    }

    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

        //Debug.Log("コルーチン作動");

        if (roomData.isHandshaking)
        {
            //ハンドシェイク確認用パケット破棄前にサーバがなくなるとバグるためここに記述
            CancelInvoke("SendFirstHandshake");
            //Debug.Log("サーバからの返答がありません、サーバ処理へ移行");

            Managers.instance.playerID = 0;
            myNetIngameData.PlayerID = Managers.instance.playerID;
            roomData.myID = Managers.instance.playerID;

            allData.pData = myNetIngameData;
            allData.rData = roomData;

            playerDataList[0] = allData;

            clientList[0].Release();

            tempServer.Dispose();

            mainServer = new OscServer(startPort);

            isServer = true;

            isFinishHandshake = true;
            roomData.playerName = Managers.instance.optionData.playerName;

            mainServer.TryAddMethod(address, ReadValue);
        }
        else
        {
            CancelInvoke("SendFirstHandshake");
            //Debug.Log("サーバが存在しました、クライアント処理へ移行");

            tempServer.Dispose();

            mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);

            isServer = false;

            isFinishHandshake = true;
            roomData.playerName = Managers.instance.optionData.playerName;

            mainServer.TryAddMethod(address, ReadValue);
        }
    }

    private int GetRandomTempPort()
    {
        return UnityEngine.Random.Range(50006, 51000);
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

        int i = 0;

        //サーバなら五回、クライアントなら一回
        foreach (OscClientData _clientData in clientList)
        {
            if (_clientData.IsUsing())
            {
                //Debug.Log(i + " へデータ送信");

                //データの送信
                _clientData.client.Send(address, _sendBytes, _sendBytes.Length);
            }
            i++;
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
                if (_allData.rData.myID == -1)
                {
                    _allData.rData = initRoomData(_allData.rData);
                    _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                    playerDataList[_allData.pData.PlayerID] = _allData;

                    connectTimeList[_allData.pData.PlayerID] = 0.0f;

                    clientList[_allData.pData.PlayerID].Release();

                    return;
                }

                testS = "サーバとしてインゲーム受信";
                playerDataList[_allData.pData.PlayerID] = _allData;

                //受信カウントをリセットする
                connectTimeList[_allData.pData.PlayerID] = 0f;
            }
            else if (_allData.rData.myID == -1 && _allData.rData.isHandshaking)
            {
                testS = "サーバとしてコネクション受信";


                testNum++;

                for (int i = 1; i < playerDataList.Count; i++)
                {
                    if (playerDataList[i].rData.myID == -1)
                    {
                        AllGameData.AllData _handshakeAllData = new AllGameData.AllData();
                        _handshakeAllData.rData = initRoomData(_handshakeAllData.rData);

                        _handshakeAllData.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
                        _handshakeAllData.pData.mainPacketData.comData.myPort = startPort + i;
                        _handshakeAllData.pData.PlayerID = i;
                        _handshakeAllData.rData.myID = i;
                        _handshakeAllData.rData.isHandshaking = false;

                        playerDataList[i] = _handshakeAllData;

                        OscClient _tempClient = new OscClient(_allData.pData.mainPacketData.comData.myIP, _allData.pData.mainPacketData.comData.myPort);

                        SendValueTarget(_handshakeAllData, _tempClient);

                        clientList[i].Assign(_allData.pData.mainPacketData.comData.myIP, startPort + i);

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
                Managers.instance.playerID = _allData.rData.myID;
                roomData.myID = _allData.rData.myID;

                myNetIngameData = _allData.pData;
                roomData = _allData.rData;

                allData.pData = myNetIngameData;
                allData.rData = roomData;

                playerDataList[Managers.instance.playerID] = allData;

                clientList[0].Release();

                clientList[0].Assign(_allData.pData.mainPacketData.comData.myIP, startPort);

            }
            else
            {
                //testS = "クライアントとしてインゲーム受信";

                if (_allData.rData.myID == -1 && _allData.pData.PlayerID == 0)
                {
                    testS = "サーバが抜けました";

                    _allData.rData = initRoomData(_allData.rData);
                    _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                    playerDataList[0] = _allData;

                    connectTimeList[0] = 0.0f;

                    clientList[0].Release();

                    InitNetworkData();

                    DisPacket();

                    isOutServer = true;

                    return;
                }

                playerDataList[_allData.pData.PlayerID] = _allData;

                //受信カウントをリセットする
                connectTimeList[0] = 0f;
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
        _roomData.myID = -1;
        _roomData.myTeamNum = -1;
        _roomData.selectedCharacterID = 0;
        _roomData.ready = false;
        _roomData.gameStart = false;
        _roomData.isInData = true;
        _roomData.isHandshaking = true;

        return _roomData;
    }

    /// <summary>
    /// インゲームデータの初期化処理
    /// </summary>
    /// <param name="_ingameData">初期化したいインゲームデータ</param>
    /// <returns>インゲームデータの初期化値</returns>
    IngameData.GameData initIngameData(IngameData.GameData _ingameData)
    {
        _ingameData.play = false;
        _ingameData.start = false;
        _ingameData.end = false;
        _ingameData.startTimer = 0;
        _ingameData.endTimer = 0;

        _ingameData.roundCount = 1;
        _ingameData.roundTimer = 60;
        _ingameData.alivePlayerCountTeamA = 0;
        _ingameData.alivePlayerCountTeamB = 0;
        _ingameData.winCountTeamA = 0;
        _ingameData.winCountTeamB = 0;
        _ingameData.winner = -1;

        return _ingameData;
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
    private void MainThreadMethod()
    {

    }

    private void TimeoutChecker()
    {
        if (Managers.instance.state >= GAME_STATE.ROOM && Managers.instance.state <= GAME_STATE.IN_GAME)
        {
            if (isServer)
            {
                for (int i = 1; i < maxPlayer; i++)
                {
                    //もしプレイヤーが存在し、タイムアウト時間に達していればそのプレイヤーを初期化する
                    if (playerDataList[i].rData.myID != -1 && connectTimeList[i] > timeoutSec)
                    {
                        AllGameData.AllData _allData = new AllGameData.AllData();

                        _allData.rData = initRoomData(_allData.rData);
                        _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                        playerDataList[i] = _allData;

                        connectTimeList[i] = 0.0f;

                        clientList[i].Release();
                    }
                    else if (playerDataList[i].rData.myID != -1)
                    {
                        connectTimeList[i] += Time.deltaTime;
                    }
                }
            }
            else
            {
                //タイムアウト時間に達していれば自身を初期化させてタイトルまで戻す
                if (connectTimeList[0] > timeoutSec)
                {
                    InitNetworkData();

                    //Debug.Log("接続がタイムアウトしました");

                    DisPacket();

                    Managers.instance.ChangeScene(GAME_STATE.TITLE);
                    Managers.instance.ChangeState(GAME_STATE.TITLE);
                }
                else
                {
                    connectTimeList[0] += Time.deltaTime;
                }
            }
        }

        return;
    }

    /// <summary>
    /// クライアントのネットワーク初期化処理
    /// </summary>
    private void InitNetworkData()
    {

        if (isServer)
        {
            AllGameData.AllData _allData = new AllGameData.AllData();

            _allData.rData = initRoomData(_allData.rData);
            _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

            playerDataList[0] = _allData;

            myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);
            roomData = initRoomData(roomData);

            sendStartTimer = 0f;

            isFinishHandshake = false;

            foreach (OscClientData _client in clientList)
            {
                _client.Release();
            }

            mainServer.Dispose();
        }
        else
        {
            AllGameData.AllData _allData = new AllGameData.AllData();

            _allData.rData = initRoomData(_allData.rData);
            _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

            playerDataList[0] = _allData;

            myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);
            roomData = initRoomData(roomData);

            sendStartTimer = 0f;

            isFinishHandshake = false;

            mainServer.Dispose();
        }

        return;
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

    public bool GetIsFinishedHandshake()
    {
        return isFinishHandshake;
    }

    /// <summary>
    /// ルームから抜けた時
    /// </summary>
    public void ExitToRoom()
    {
        if (isServer)
        {
            AllGameData.AllData _data = new AllGameData.AllData();
            _data.rData = initRoomData(_data.rData);
            _data = playerDataList[Managers.instance.playerID];

            _data.rData.myID = -1;

            for (int i = 0; i < playerDataList.Count; i++)
            {
                if (playerDataList[i].rData.myID != -1)
                {
                    SendValue(_data);
                }
            }

            InitNetworkData();
        }
        else
        {
            AllGameData.AllData _data = new AllGameData.AllData();
            _data.rData = initRoomData(_data.rData);
            _data = playerDataList[Managers.instance.playerID];

            _data.rData.myID = -1;

            SendValue(_data);

            InitNetworkData();
        }

        DisPacket();

        return;
    }
}